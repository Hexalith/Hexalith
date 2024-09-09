// <copyright file="IClientCommandService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public interface IClientCommandService
{
    /// <summary>
    /// Sends a command asynchronously.
    /// </summary>
    /// <param name="command">The command to send.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendCommandAsync(object command, Metadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a command asynchronously.
    /// </summary>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendCommandAsync(object command, CancellationToken cancellationToken);
}