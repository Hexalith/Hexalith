// <copyright file="AzureActiveDirectoryApplicationSecurityContextConfiguration.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AzureActiveDirectory.Configurations;

/// <summary>
/// Azure Active Directory Application Configuration.
/// </summary>
public class AzureActiveDirectoryApplicationSecurityContextConfiguration
{
    /// <summary>
    /// Gets or sets azure Active Directory Application identifier (ClientId).
    /// </summary>
    public string? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets azure Active Directory Application secret.
    /// </summary>
    public string? ApplicationSecret { get; set; }

    /// <summary>
    /// Gets or sets microsoft Azure Active Directory tenant identifier (for example: yourdomain.com).
    /// </summary>
    public string? Tenant { get; set; }
}