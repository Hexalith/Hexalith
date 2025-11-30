// <copyright file="SessionManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Manages user sessions by handling authentication tokens and user sign-out.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SessionManager"/> class.
/// </remarks>
/// <param name="tokenProvider">The access token provider.</param>
/// <param name="navigationManager">The navigation manager.</param>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="logger">The logger.</param>
public class SessionManager(
    IAccessTokenProvider tokenProvider,
    NavigationManager navigationManager,
    HttpClient httpClient,
    ILogger<SessionManager> logger) : ISessionManager
{
    private static readonly Uri SignOutUri = new("api/session/signout", UriKind.Relative);
    private static readonly Uri ValidateSessionUri = new("api/session/validate", UriKind.Relative);

    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<SessionManager> _logger = logger;
    private readonly NavigationManager _navigationManager = navigationManager;
    private readonly IAccessTokenProvider _tokenProvider = tokenProvider;

    /// <inheritdoc/>
    /// <summary>
    /// Gets the current session asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current <see cref="ClaimsPrincipal"/>.</returns>
    public async Task<ClaimsPrincipal> GetCurrentSessionAsync()
    {
        try
        {
            AccessTokenResult tokenResult = await _tokenProvider.RequestAccessToken().ConfigureAwait(false);
            if (!tokenResult.TryGetToken(out AccessToken? token))
            {
                Log.AccessTokenUnavailable(_logger);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Value);

                using HttpResponseMessage response = await _httpClient
                        .GetAsync(ValidateSessionUri)
                        .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    ClaimsPrincipal principal = await GetUserPrincipalFromTokenAsync(token.Value).ConfigureAwait(false);
                    return principal;
                }

                Log.SessionValidationFailed(_logger, response.StatusCode);
            }
        }
        catch (AccessTokenNotAvailableException atnae)
        {
            Log.AccessTokenNotAvailableException(_logger, atnae);
            atnae.Redirect();
        }
        catch (HttpRequestException ex)
        {
            Log.SessionHttpError(_logger, ex);
        }
        catch (SecurityTokenException ex)
        {
            Log.TokenParsingFailure(_logger, ex);
        }
        catch (ArgumentException ex)
        {
            Log.TokenParsingFailure(_logger, ex);
        }

        return new ClaimsPrincipal(new ClaimsIdentity());
    }

    /// <inheritdoc/>
    /// <summary>
    /// Signs out the current user asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous sign-out operation.</returns>
    public async Task SignOutAsync()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient
                .PostAsync(SignOutUri, null)
                .ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Log.SignOutRequestFailed(_logger, response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            Log.SignOutHttpError(_logger, ex);
        }
        catch (OperationCanceledException ex)
        {
            Log.SignOutHttpError(_logger, ex);
        }
        finally
        {
            _navigationManager.NavigateTo("authentication/logout");
        }
    }

    /// <summary>
    /// Gets the user principal from the token asynchronously.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ClaimsPrincipal"/>.</returns>
    private static Task<ClaimsPrincipal> GetUserPrincipalFromTokenAsync(string token)
    {
        JsonWebTokenHandler handler = new();
        JsonWebToken jwtToken = handler.ReadJsonWebToken(token);

        ClaimsIdentity identity = new(jwtToken.Claims, "Bearer");
        return Task.FromResult(new ClaimsPrincipal(identity));
    }

    private static class Log
    {
        private static readonly Action<ILogger, Exception?> AccessTokenUnavailableMessage =
            LoggerMessage.Define(
                LogLevel.Warning,
                new EventId(1, nameof(AccessTokenUnavailable)),
                "Access token not available. User might not be authenticated or token request failed.");

        private static readonly Action<ILogger, HttpStatusCode, Exception?> SessionValidationFailedMessage =
            LoggerMessage.Define<HttpStatusCode>(
                LogLevel.Warning,
                new EventId(2, nameof(SessionValidationFailed)),
                "Session validation failed with status code {StatusCode}.");

        private static readonly Action<ILogger, Exception?> AccessTokenNotAvailableExceptionMessage =
            LoggerMessage.Define(
                LogLevel.Warning,
                new EventId(3, nameof(AccessTokenNotAvailableException)),
                "Access token not available. Redirecting to authentication.");

        private static readonly Action<ILogger, Exception?> SessionHttpErrorMessage =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(4, nameof(SessionHttpError)),
                "HTTP error while retrieving current session.");

        private static readonly Action<ILogger, Exception?> TokenParsingFailureMessage =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(5, nameof(TokenParsingFailure)),
                "Failed to parse access token while retrieving current session.");

        private static readonly Action<ILogger, HttpStatusCode, Exception?> SignOutRequestFailedMessage =
            LoggerMessage.Define<HttpStatusCode>(
                LogLevel.Warning,
                new EventId(6, nameof(SignOutRequestFailed)),
                "Sign-out HTTP request failed with status code {StatusCode}.");

        private static readonly Action<ILogger, Exception?> SignOutHttpErrorMessage =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(7, nameof(SignOutHttpError)),
                "HTTP error while signing out the current user.");

        internal static void AccessTokenUnavailable(ILogger logger) => AccessTokenUnavailableMessage(logger, null);

        internal static void SessionValidationFailed(ILogger logger, HttpStatusCode statusCode) => SessionValidationFailedMessage(logger, statusCode, null);

        internal static void AccessTokenNotAvailableException(ILogger logger, Exception exception) => AccessTokenNotAvailableExceptionMessage(logger, exception);

        internal static void SessionHttpError(ILogger logger, Exception exception) => SessionHttpErrorMessage(logger, exception);

        internal static void TokenParsingFailure(ILogger logger, Exception exception) => TokenParsingFailureMessage(logger, exception);

        internal static void SignOutRequestFailed(ILogger logger, HttpStatusCode statusCode) => SignOutRequestFailedMessage(logger, statusCode, null);

        internal static void SignOutHttpError(ILogger logger, Exception exception) => SignOutHttpErrorMessage(logger, exception);
    }
}