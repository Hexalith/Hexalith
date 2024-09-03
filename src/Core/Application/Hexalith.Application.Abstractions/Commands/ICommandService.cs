// <copyright file="ICommandService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Hexalith.Application.States;

/// <summary>
/// Represents a service for submitting commands.
/// </summary>
public interface ICommandService
{
    /// <summary>
    /// Submits a command asynchronously.
    /// </summary>
    /// <param name="command">The command to submit.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SubmitCommandAsync(CommandState command, CancellationToken cancellationToken);
}