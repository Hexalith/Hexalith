// <copyright file="IDomainAggregateActor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using Dapr.Actors;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Interface IAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IDomainAggregateActor : IActor
{
    /// <summary>
    /// Clear all commands.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ClearCommandsAsync();

    /// <summary>
    /// Gets the snapshot event asynchronous.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
    /// <returns>Task&lt;System.Nullable&lt;EventState&gt;&gt;.</returns>
    Task SendSnapshotEventAsync();

    /// <summary>
    /// Submits the command asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>Task.</returns>
    Task SubmitCommandAsync(ActorMessageEnvelope envelope);
}