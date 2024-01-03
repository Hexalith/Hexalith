// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using Hexalith.Infrastructure.DaprRuntime.Handlers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Logistics partner catalog item aggregate actor interface <see cref="BspkSalesInvoice" />.
/// Extends the <see cref="IActor" />.
/// </summary>
public abstract partial class AggregateActor : Actor, IAggregateActor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActor" /> class.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    protected AggregateActor(
        ActorHost host)
        : base(host) => ArgumentNullException.ThrowIfNull(host);

    /// <summary>
    /// Logs the processing commands information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Actor {ActorType} ({ActorId}) is processing commands.")]
    public static partial void LogProcessingCommandsInformation(ILogger logger, string actorId, string actorType);

    /// <inheritdoc/>
    public async Task ProcessCommandsAsync()
    {
        LogProcessingCommandsInformation(Logger, Id.ToString(), Host.ActorTypeInfo.ActorTypeName);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ActorCommandEnvelope envelope) => await Task.CompletedTask.ConfigureAwait(false);
}