// <copyright file="ApiKeyAuthenticationSchemeOptions.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Authentications;

using Microsoft.AspNetCore.Authentication;

/// <summary>
/// Options for API key authentication.
/// </summary>
public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets the API key against which to authenticate.
    /// </summary>
    public string? ApiKey { get; set; }
}