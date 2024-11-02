// <copyright file="SessionIdService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Services;

using Microsoft.JSInterop;

/// <summary>
/// Service for retrieving session IDs from client-side cookies in a WASM application.
/// </summary>
public class SessionIdService : ISessionIdService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly string _sessionCookieName;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionIdService"/> class.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime instance.</param>
    /// <param name="sessionCookieName">The name of the session cookie.</param>
    public SessionIdService(IJSRuntime jsRuntime, string sessionCookieName)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(sessionCookieName);
        _jsRuntime = jsRuntime;
        _sessionCookieName = sessionCookieName;
    }

    /// <summary>
    /// Asynchronously gets the session ID from the client-side cookie.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a result of the session ID as a string.</returns>
    public async Task<string?> GetSessionIdAsync()
    {
        string cookieValue = await GetCookieValueAsync(_sessionCookieName);
        return cookieValue;
    }

    /// <summary>
    /// Asynchronously gets the value of a specified cookie.
    /// </summary>
    /// <param name="cookieName">The name of the cookie.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a result of the cookie value as a string.</returns>
    private async Task<string> GetCookieValueAsync(string cookieName) => await _jsRuntime.InvokeAsync<string>("getCookie", cookieName);
}