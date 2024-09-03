// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-04-2024
// ***********************************************************************
// <copyright file="IAggregateActor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
    Task SubmitCommandAsync(ActorCommandEnvelope envelope);
}