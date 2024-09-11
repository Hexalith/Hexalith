// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-04-2024
// ***********************************************************************
// <copyright file="IAggregateActor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using Dapr.Actors;

using Hexalith.Application.States;

/// <summary>
/// Interface IAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IAggregateActor : IActor
{
    Task ClearCommandsAsync();

    /// <summary>
    /// Gets the snapshot event asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;EventState&gt;&gt;.</returns>
    Task<EventState?> GetSnapshotEventAsync();

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
    [Obsolete("Use SubmitCommandAsync(ActorMessageEnvelope envelope) instead.", false)]
    Task SubmitCommandAsync(ActorCommandEnvelope envelope);

    /// <summary>
    /// Submits the command asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>Task.</returns>
    Task SubmitCommandAsync(ActorMessageEnvelope envelope);
}