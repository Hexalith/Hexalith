// <copyright file="IAggregateActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Interface IAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
[Obsolete("This interface is not used anymore. Use Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors.IDomainAggregateActor instead.", false)]
public interface IAggregateActor : IActor
{
    Task ClearCommandsAsync();

    /// <summary>
    /// Gets the snapshot event asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;MessageState&gt;&gt;.</returns>
    Task<MessageState?> GetSnapshotEventAsync();

    /// <summary>
    /// Processes the callback asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task ProcessCallbackAsync();

    /// <summary>
    /// Processes the commands asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task<bool> ProcessNextCommandAsync();

    /// <summary>
    /// Publishes the callback.
    /// </summary>
    /// <returns>Task.</returns>
    Task PublishCallbackAsync();

    /// <summary>
    /// Publishes the messages asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task<bool> PublishNextMessageAsync();

    /// <summary>
    /// Gets the snapshot event asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;MessageState&gt;&gt;.</returns>
    Task SendSnapshotEventAsync();

    /// <summary>
    /// Submits the command asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>Task.</returns>
    Task SubmitCommandAsync(ActorMessageEnvelope envelope);
}