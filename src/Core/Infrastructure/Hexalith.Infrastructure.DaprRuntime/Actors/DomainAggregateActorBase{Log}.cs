// <copyright file="DomainAggregateActorBase{Log}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;

using Microsoft.Extensions.Logging;

/// <summary>
/// The domain aggregate actor base class.
/// </summary>
public abstract partial class DomainAggregateActorBase
{
    /// <summary>
    /// Logs the accepted command information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="commandId">The command identifier.</param>
    /// <param name="partitionKey">Key of the aggregate.</param>
    [LoggerMessage(
                    EventId = 1,
                    Level = LogLevel.Information,
                    Message = "Accepted command '{CommandType}' ({CommandId}) for aggregate key {PartitionKey}.")]
    public static partial void LogAcceptedCommandInformation(ILogger logger, string commandType, string commandId, string partitionKey);

    /// <summary>
    /// Logs the invalid publish message error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="sequence">The sequence.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "Message {Sequence} Id={MessageId} Type={MessageType} CorrelationId={CorrelationId} AggregateId={AggregateId} is invalid and cannot be published.")]
    public static partial void LogInvalidPublishMessageError(
            ILogger logger,
            long sequence,
            string? messageId,
            string? messageType,
            string? correlationId,
            string? aggregateId);

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
    /// <param name="partitionKey"></param>
    [LoggerMessage(
                    EventId = 4,
                    Level = LogLevel.Information,
                    Message = "Processed command {CommandType} ({CommandId}) for aggregate {PartitionKey}.")]
    public static partial void LogProcessedCommandInformation(ILogger logger, string commandType, string commandId, string partitionKey);

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
            EventId = 5,
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
                EventId = 6,
                Level = LogLevel.Information,
                Message = "Actor {ActorType} ({ActorId}) is processing command {CurrentCommandProcessed} on a total of {CommandCount}")]
    public static partial void LogProcessingCommandsInformation(ILogger logger, string actorId, string actorType, long commandCount, long currentCommandProcessed);

    /// <summary>
    /// Logs the publish error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="messageSequence">The message sequence.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="actorId">The actor identifier.</param>
    /// <param name="actorType">Type of the actor.</param>
    /// <param name="errorMessage">The error message.</param>
    [LoggerMessage(
            EventId = 7,
            Level = LogLevel.Warning,
            Message = "Publish message {MessageSequence} (Id={MessageId}; CorrelationId={CorrelationId}) operation failed on actor {ActorType}/{ActorId}. Error : {ErrorMessage}")]
    public static partial void LogPublishError(
        ILogger logger,
        Exception exception,
        long messageSequence,
        string? messageId,
        string? correlationId,
        string actorId,
        string actorType,
        string errorMessage);

    [LoggerMessage(
        EventId = 8,
        Level = LogLevel.Warning,
        Message = "Application error while processing command number {CommandSequence}, type '{CommandType}', correlationId '{CorrelationId}' for aggregate key '{PartitionKey}' : {ErrorMessage}\n{TechnicalErrorMessage}")]
    private static partial void LogApplicationErrorWarning(
        ILogger logger,
        long commandSequence,
        string commandType,
        string partitionKey,
        string correlationId,
        string errorMessage,
        string? technicalErrorMessage);

    [LoggerMessage(
        EventId = 9,
        Level = LogLevel.Information,
        Message = "The command number {Sequence} '{MessageType}' cannot be processed on aggregate key {PartitionKey}. The retry attempt {NextRetryNumber} will be executed in {RetryDueTime} at {NextRetryDateTime}. CorrelationId={CorrelationId}.")]
    private static partial void LogTaskProcessorRetryInformation(
        ILogger logger,
        string messageType,
        long sequence,
        string correlationId,
        string partitionKey,
        int nextRetryNumber,
        TimeSpan retryDueTime,
        DateTimeOffset nextRetryDateTime);

    [LoggerMessage(
            EventId = 10,
            Level = LogLevel.Error,
            Message = "Unhandled command processing error. Command handlers should throw application error exceptions. Processing command number {CommandSequence}, type '{CommandType}', correlationId '{CorrelationId}' for aggregate key '{PartitionKey}'.")]
    private static partial void LogUnhandledCommandProcessingError(
        ILogger logger,
        Exception exception,
        long commandSequence,
        string commandType,
        string partitionKey,
        string correlationId);
}