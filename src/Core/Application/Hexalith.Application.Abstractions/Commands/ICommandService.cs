// <copyright file="ICommandService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

/// <summary>
/// Represents a service for submitting commands.
/// </summary>
public interface ICommandService
{
    /// <summary>
    /// Submits a command asynchronously.
    /// </summary>
    /// <param name="user">The user submitting the command.</param>
    /// <param name="command">The command to submit.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SubmitCommandAsync(ClaimsPrincipal user, object command, CancellationToken cancellationToken);

    /// <summary>
    /// Submits multiple commands asynchronously.
    /// </summary>
    /// <param name="user">The user submitting the commands.</param>
    /// <param name="commands">The commands to submit.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    async Task SubmitCommandsAsync([NotNull] ClaimsPrincipal user, [NotNull] IEnumerable<object> commands, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(commands);

        await Task.WhenAll(commands.Select(command => SubmitCommandAsync(user, command, cancellationToken)));
    }
}