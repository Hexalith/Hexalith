// <copyright file="IApplicationSecurityContext.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AzureActiveDirectory;

/// <summary>
/// Interface for the application security context.
/// </summary>
public interface IApplicationSecurityContext
{
    /// <summary>
    /// Gets the application security token.
    /// </summary>
    /// <param name="scopes">Security scopes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The access token.</returns>
    Task<string> AcquireTokenAsync(string[] scopes, CancellationToken cancellationToken);
}