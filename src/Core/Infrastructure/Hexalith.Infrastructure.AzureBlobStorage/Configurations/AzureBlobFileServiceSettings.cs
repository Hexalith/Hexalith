// <copyright file="AzureBlobFileServiceSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AzureBlobStorage.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Represents configuration settings for Azure Blob Storage file service.
/// </summary>
/// <remarks>
/// This class implements <see cref="ISettings"/> and provides configuration properties
/// for connecting to Azure Blob Storage, including both connection string and Azure AD authentication options.
/// </remarks>
public class AzureBlobFileServiceSettings : ISettings
{
    /// <summary>
    /// Gets or sets the Azure Active Directory application (client) ID.
    /// </summary>
    /// <value>
    /// The application ID used for Azure AD authentication.
    /// </value>
    public string? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the Azure Active Directory application secret.
    /// </summary>
    /// <value>
    /// The secret key used for Azure AD authentication.
    /// </value>
    public string? ApplicationSecret { get; set; }

    /// <summary>
    /// Gets or sets the Azure Blob Storage container URL.
    /// </summary>
    /// <value>
    /// The full URL to the Azure Blob Storage container.
    /// </value>
    public Uri? ContainerUrl { get; set; }

    /// <summary>
    /// Gets or sets the Azure Active Directory tenant ID.
    /// </summary>
    /// <value>
    /// The tenant ID used for Azure AD authentication.
    /// </value>
    public string? Tenant { get; set; }

    /// <summary>
    /// Gets or sets the Azure Storage connection string.
    /// </summary>
    /// <value>
    /// The connection string used to connect directly to Azure Storage.
    /// This is an alternative to Azure AD authentication.
    /// </value>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets the configuration section name for Azure Storage settings.
    /// </summary>
    /// <returns>The configuration section name in the format "Hexalith:FileService:AzureStorage".</returns>
    public static string ConfigurationName() => "Hexalith:FileService:AzureStorage";
}