// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActorBase{Loggers}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public abstract partial class AggregateActorBase : Actor, IRemindable, IAggregateActor
{
    /// <summary>
    /// Logs the accepted command information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="commandId">The command identifier.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    [LoggerMessage(
                    EventId = 2,
                Level = LogLevel.Information,
                Message = "Accepted command {CommandType} ({CommandId}) for aggregate {AggregateName}:{AggregateId}.")]
    public static partial void LogAcceptedCommandInformation(ILogger logger, string commandType, string commandId, string aggregateName, string aggregateId);

    /// <summary>
    /// Logs the no commands to submit warning.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    [LoggerMessage(
                EventId = 3,
                Level = LogLevel.Warning,
                Message = "The command envelope submitted to {ActorType} ({ActorId}), has no commands to process.")]
    public static partial void LogNoCommandsToSubmitWarning(ILogger logger, string actorId, string actorType);

    /// <summary>
    /// Logs the processed command information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="commandId">The command identifier.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    [LoggerMessage(
                    EventId = 4,
                Level = LogLevel.Information,
                Message = "Processed command {CommandType} ({CommandId}) for aggregate {AggregateName}:{AggregateId}.")]
    public static partial void LogProcessedCommandInformation(ILogger logger, string commandType, string commandId, string aggregateName, string aggregateId);

    /// <summary>
    /// Logs the processing callback error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="ex">The ex.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    /// <param name="commandCount">The command count.</param>
    /// <param name="commandProcessed">The command processed.</param>
    [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Error,
            Message = "Actor {ActorType} ({ActorId}) failed processing {CommandProcessed}/{CommandCount} command in a timer or reminder callback. Resetting state.")]
    public static partial void LogProcessingCallbackError(ILogger logger, Exception ex, string actorId, string actorType, long commandCount, long commandProcessed);

    /// <summary>
    /// Logs the processing commands information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    /// <param name="commandCount">The command count.</param>
    /// <param name="currentCommandProcessed">The current command processed.</param>
    [LoggerMessage(
                EventId = 1,
            Level = LogLevel.Information,
            Message = "Actor {ActorType} ({ActorId}) is processing command {CurrentCommandProcessed} on a total of {CommandCount}")]
    public static partial void LogProcessingCommandsInformation(ILogger logger, string actorId, string actorType, long commandCount, long currentCommandProcessed);
}