// <copyright file="IFileService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

using System.Threading.Tasks;

/// <summary>
/// Defines a service interface for handling file operations in the application.
/// This service provides methods for uploading and downloading files with associated metadata.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Downloads a file asynchronously from the storage system.
    /// </summary>
    /// <param name="containerName">The name of the container where the file is stored.</param>
    /// <param name="fileId">The unique identifier of the file to download. This should match an ID previously used during upload.</param>
    /// <param name="writeStream">The stream where the downloaded file data will be written. The stream should be writable.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing a collection of tags associated with the downloaded file.</returns>
    /// <remarks>
    /// The stream will not be closed by this method. The caller is responsible for managing the stream's lifecycle.
    /// </remarks>
    Task<IDictionary<string, string>> DownloadAsync(string containerName, string fileId, Stream writeStream, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a file asynchronously to the storage system.
    /// </summary>
    /// <param name="containerName">The name of the container where the file is stored.</param>
    /// <param name="fileId">The unique identifier for the file. This ID can be used later to retrieve the file.</param>
    /// <param name="readStream">The stream containing the file data to upload. The stream should be readable and positioned at the beginning of the content.</param>
    /// <param name="tags">A collection of tags associated with the file for categorization and metadata purposes.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous upload operation.</returns>
    /// <remarks>
    /// The stream will not be closed by this method. The caller is responsible for managing the stream's lifecycle.
    /// </remarks>
    Task UploadAsync(string containerName, string fileId, Stream readStream, IDictionary<string, string> tags, CancellationToken cancellationToken);
}