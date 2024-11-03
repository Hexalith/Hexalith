﻿// <copyright file="AzureBlobStorageFileService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AzureBlobStorage.Services;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Hexalith.Application.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.AzureBlobStorage.Configurations;

using Microsoft.Extensions.Options;

/// <summary>
/// Provides file storage operations using Azure Blob Storage.
/// </summary>
/// <remarks>
/// This service implements the <see cref="IFileService"/> interface to provide file storage capabilities
/// using Azure Blob Storage as the underlying storage mechanism.
/// </remarks>
public class AzureBlobStorageFileService(IOptions<AzureBlobFileServiceSettings> settings) : IFileService
{
    private readonly string? _containerName;
    private readonly IOptions<AzureBlobFileServiceSettings> _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    private BlobServiceClient? _blobServiceClient;

    /// <summary>
    /// Gets the BlobServiceClient instance, creating it if it doesn't exist.
    /// </summary>
    /// <returns>The initialized BlobServiceClient.</returns>
    private BlobServiceClient BlobServiceClient => _blobServiceClient ??= CreateBlobServiceClient();

    /// <summary>
    /// Downloads a file from Azure Blob Storage.
    /// </summary>
    /// <param name="fileId">The unique identifier of the file to download.</param>
    /// <param name="writeStream">The stream where the file content will be written.</param>
    /// <returns>A dictionary containing the blob's metadata tags.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fileId"/> or <paramref name="writeStream"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fileId"/> is empty or consists only of white-space characters.</exception>
    /// <exception cref="Azure.RequestFailedException">Thrown when a failure occurs while interacting with Azure Storage or the blob is not found.</exception>
    public async Task<IDictionary<string, string>> DownloadAsync(string fileId, Stream writeStream)
    {
        BlobContainerClient containerClient = BlobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileId);

        _ = await blobClient.DownloadToAsync(writeStream);

        Azure.Response<GetBlobTagResult> tags = await blobClient.GetTagsAsync();
        return tags.Value.Tags;
    }

    /// <summary>
    /// Uploads a file to Azure Blob Storage.
    /// </summary>
    /// <param name="fileId">The unique identifier for the file in blob storage.</param>
    /// <param name="readStream">The stream containing the file content to upload.</param>
    /// <param name="tags">Optional metadata tags to associate with the blob.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fileId"/> or <paramref name="readStream"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fileId"/> is empty or consists only of white-space characters.</exception>
    /// <exception cref="Azure.RequestFailedException">Thrown when a failure occurs while interacting with Azure Storage.</exception>
    public async Task UploadAsync(string fileId, Stream readStream, IDictionary<string, string> tags)
    {
        BlobContainerClient containerClient = BlobServiceClient.GetBlobContainerClient(_containerName);
        _ = await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        BlobClient blobClient = containerClient.GetBlobClient(fileId);
        _ = await blobClient.UploadAsync(readStream, true);

        if (tags != null && tags.Count > 0)
        {
            _ = await blobClient.SetTagsAsync(tags);
        }
    }

    private BlobServiceClient CreateBlobServiceClient()
    {
        if (_blobServiceClient is not null)
        {
            return _blobServiceClient;
        }

        if (_settings.Value.ContainerUrl == null)
        {
            SettingsException<AzureBlobFileServiceSettings>.ThrowIfNullOrWhiteSpace(_settings.Value.ConnectionString);
            _blobServiceClient = new(_settings.Value.ConnectionString);
        }
        else
        {
            SettingsException<AzureBlobFileServiceSettings>.ThrowIfNullOrWhiteSpace(_settings.Value.ApplicationSecret);
            SettingsException<AzureBlobFileServiceSettings>.ThrowIfNullOrWhiteSpace(_settings.Value.ApplicationId);
            SettingsException<AzureBlobFileServiceSettings>.ThrowIfNullOrWhiteSpace(_settings.Value.Tenant);
            ClientSecretCredential credential = new(_settings.Value.Tenant, _settings.Value.ApplicationId, _settings.Value.ApplicationSecret);
            _blobServiceClient = new(_settings.Value.ContainerUrl, credential);
        }

        return _blobServiceClient;
    }
}