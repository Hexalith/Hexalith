// <copyright file="ILoginRedirectUrlService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.DaprIdentityStore.Services;

/// <summary>
/// Interface for a service that provides login redirect URLs.
/// </summary>
public interface ILoginRedirectUrlService
{
    /// <summary>
    /// Gets the redirect URL for login.
    /// </summary>
    /// <param name="returnLocation">The return URL to redirect to after login. If null, a default URL is used.</param>
    /// <returns>The redirect URL as a <see cref="Uri"/>.</returns>
    string GetRedirect(string returnLocation) => new($"Account/Login?returnUrl={Uri.EscapeDataString(returnLocation)}");
}