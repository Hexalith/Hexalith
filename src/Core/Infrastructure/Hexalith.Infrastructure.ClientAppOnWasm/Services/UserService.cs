// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Hexalith.Infrastructure.ClientApp.Services;

using Microsoft.AspNetCore.Components.Authorization;

/// <summary>
/// Service to handle user-related operations.
/// </summary>
public class UserService : IUserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="authenticationStateProvider">The authentication state provider.</param>
    public UserService([NotNull] AuthenticationStateProvider authenticationStateProvider)
    {
        ArgumentNullException.ThrowIfNull(authenticationStateProvider);
        _authenticationStateProvider = authenticationStateProvider;
    }

    /// <inheritdoc/>
    public async Task<string> GetUserIdAsync(CancellationToken cancellationToken)
    {
        // Get the user ID from the authentication state.
        AuthenticationState state = await _authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
        return state.User?.FindFirst("sub")?.Value
        ?? throw new InvalidOperationException("User ID not found in the authentication state.");
    }
}