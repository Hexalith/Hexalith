// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-04-2024
// ***********************************************************************
// <copyright file="IAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using Dapr.Actors;

using Hexalith.Infrastructure.DaprRuntime.Handlers;

/// <summary>
/// Interface IAggregateActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IAggregateActor : IActor
{
    /// <summary>
    /// Continues the workflow asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    public Task ContinueProcessingWorkflowAsync();

    /// <summary>
    /// Processes the commands asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    public Task<bool> ProcessNextCommandAsync();

    /// <summary>
    /// Publishes the messages asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    public Task<bool> PublishNextMessageAsync();

    /// <summary>
    /// Submits the command asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>Task.</returns>
    public Task SubmitCommandAsync(ActorCommandEnvelope envelope);
}