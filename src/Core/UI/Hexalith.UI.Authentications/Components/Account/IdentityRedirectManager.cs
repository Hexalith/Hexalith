// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="IdentityRedirectManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.UI.Authentications.Components.Account;

using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Class IdentityRedirectManager. This class cannot be inherited.
/// </summary>
internal sealed class IdentityRedirectManager(NavigationManager navigationManager)
{
    /// <summary>
    /// The status cookie name.
    /// </summary>
    public const string StatusCookieName = "Identity.StatusMessage";

    /// <summary>
    /// The status cookie builder.
    /// </summary>
    private static readonly CookieBuilder _statusCookieBuilder = new()
    {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    /// <summary>
    /// Gets the current path.
    /// </summary>
    /// <value>The current path.</value>
    private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

    /// <summary>
    /// Redirects to.
    /// </summary>
    /// <param name="uri">The URI.</param>
    /// <exception cref="InvalidOperationException">null.</exception>
    [DoesNotReturn]
    public void RedirectTo(string? uri)
    {
        uri ??= string.Empty;

        // Prevent open redirects.
        if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
        {
            uri = navigationManager.ToBaseRelativePath(uri);
        }

        // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
        // So as long as this is called from a statically rendered Identity component, the InvalidOperationException is never thrown.
        navigationManager.NavigateTo(uri);
        throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
    }

    /// <summary>
    /// Redirects to.
    /// </summary>
    /// <param name="uri">The URI.</param>
    /// <param name="queryParameters">The query parameters.</param>
    [DoesNotReturn]
    public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
    {
        string uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        string newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
        RedirectTo(newUri);
    }

    /// <summary>
    /// Redirects to current page.
    /// </summary>
    [DoesNotReturn]
    public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

    /// <summary>
    /// Redirects to current page with status.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="context">The context.</param>
    [DoesNotReturn]
    public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
        => RedirectToWithStatus(CurrentPath, message, context);

    /// <summary>
    /// Redirects to with status.
    /// </summary>
    /// <param name="uri">The URI.</param>
    /// <param name="message">The message.</param>
    /// <param name="context">The context.</param>
    [DoesNotReturn]
    public void RedirectToWithStatus(string uri, string message, HttpContext context)
    {
        context.Response.Cookies.Append(StatusCookieName, message, _statusCookieBuilder.Build(context));
        RedirectTo(uri);
    }
}