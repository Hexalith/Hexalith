// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActorBase{Process}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Errors;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Errors;
using Hexalith.Infrastructure.DaprRuntime.Actors;

using Microsoft.Extensions.Logging;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public abstract partial class AggregateActorBase
{
    private readonly TimeSpan _minimumDuplicateNotificationPeriod = TimeSpan.FromMinutes(15);

    /// <inheritdoc/>
    public async Task ProcessCallbackAsync()
    {
        try
        {
            _ = await ProcessNextCommandAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            LogProcessingCallbackError(
                Logger,
                ex,
                Id.GetId(),
                Host.ActorTypeInfo.ActorTypeName,
                (_state?.LastCommandProcessed ?? 0L) + 1L,
                _state?.CommandCount ?? 0L);
            _state = null;
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ProcessNextCommandAsync()
    {
        CancellationToken cancellationToken = CancellationToken.None;
        await ProcessNextSubmittedCommandAsync(cancellationToken)
            .ConfigureAwait(false);

        await SetProcessCallbackAsync(cancellationToken)
           .ConfigureAwait(false);
        await SetPublishCallbackAsync(cancellationToken)
           .ConfigureAwait(false);
        await SaveAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveStateAsync()
            .ConfigureAwait(false);

        AggregateActorState state = await GetAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);

        // Returns true if there are unprocessed commands remaining
        return state.LastCommandProcessed < state.CommandCount;
    }

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Warning,
        Message = "Application error while processing command number {CommandSequence}, type '{CommandType}', correlationId '{CorrelationId}' for aggregate '{AggregateName}/{AggregateId}' : {ErrorMessage}\n{TechnicalErrorMessage}")]
    private static partial void LogApplicationErrorWarning(
        ILogger logger,
        long commandSequence,
        string commandType,
        string aggregateName,
        string aggregateId,
        string correlationId,
        string errorMessage,
        string? technicalErrorMessage);

    [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Information,
        Message = "The command number {Sequence} '{MessageType}' cannot be processed on aggregate {AggregateName} with id {AggregateId}. The retry attempt {NextRetryNumber} will be executed in {RetryDueTime} at {NextRetryDateTime}. CorrelationId={CorrelationId}.")]
    private static partial void LogTaskProcessorRetryInformation(
        ILogger logger,
        string messageType,
        long sequence,
        string correlationId,
        string aggregateName,
        string aggregateId,
        int nextRetryNumber,
        TimeSpan retryDueTime,
        DateTimeOffset nextRetryDateTime);

    [LoggerMessage(
            EventId = 8,
        Level = LogLevel.Error,
        Message = "Unhandled command processing error. Command handlers should throw application error exceptions. Processing command number {CommandSequence}, type '{CommandType}', correlationId '{CorrelationId}' for aggregate '{AggregateName}/{AggregateId}'.")]
    private static partial void LogUnhandledCommandProcessingError(
        ILogger logger,
        Exception exception,
        long commandSequence,
        string commandType,
        string aggregateName,
        string aggregateId,
        string correlationId);

    private async Task<TaskProcessor> GetTaskProcessorAsync(long commandSequence, CancellationToken cancellationToken)
    {
        Dapr.Actors.Runtime.ConditionalValue<TaskProcessor> taskProcessorState = await StateManager
            .TryGetStateAsync<TaskProcessor>(GetTaskProcessorStateName(commandSequence), cancellationToken)
            .ConfigureAwait(false);
        if (taskProcessorState.HasValue)
        {
            return taskProcessorState.Value;
        }

        TaskProcessor task = new(_dateTimeService.UtcNow, ResiliencyPolicy);
        return task.Start();
    }

    private string GetTaskProcessorStateName(long commandSequence) => $"{nameof(TaskProcessor)}-{commandSequence}";

    private async Task<(IEnumerable<BaseMessage> Messages, TaskProcessor Processor)> HandleCommandProcessingErrorAsync(
        TaskProcessor taskProcessor,
        long commandSequence,
        string correlationId,
        BaseCommand command,
        ApplicationErrorException ex,
        CancellationToken cancellationToken)
    {
        TaskProcessingFailure? previousFailure = taskProcessor.Failure;
        taskProcessor = taskProcessor.Fail(ex);
        IEnumerable<BaseMessage> messages = [];
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        if (taskProcessor.Status == TaskProcessorStatus.Canceled)
        {
            state.LastCommandProcessed = commandSequence;
            state.RetryOnFailurePeriod = null;
            state.RetryOnFailureDateTime = null;
            messages = [new ApplicationExceptionNotification(
                command.AggregateName,
                command.AggregateId,
                ex)];
        }
        else
        {
            if (previousFailure == null || _dateTimeService.UtcNow.Subtract(previousFailure.Date) > _minimumDuplicateNotificationPeriod)
            {
                messages = [new ApplicationExceptionNotification(
                command.AggregateName,
                command.AggregateId,
                ex)];
            }

            state.RetryOnFailurePeriod = taskProcessor.RetryPeriod;
            state.RetryOnFailureDateTime = _dateTimeService.UtcNow + taskProcessor.RetryPeriod;
            LogTaskProcessorRetryInformation(
                Logger,
                command.TypeName,
                commandSequence,
                correlationId,
                command.AggregateName,
                command.AggregateId,
                (taskProcessor.Failure?.Count ?? 0) + 1,
                state.RetryOnFailurePeriod.Value,
                state.RetryOnFailureDateTime.Value);
        }

        LogApplicationErrorWarning(
            Logger,
            commandSequence,
            command.TypeName,
            command.AggregateName,
            command.AggregateId,
            correlationId,
            ex.Error?.GetDetailMessage(CultureInfo.InvariantCulture) ?? "Unknown application error.",
            ex.Error?.GetTechnicalMessage(CultureInfo.InvariantCulture));
        return (messages, taskProcessor);
    }

    private async Task PersistTaskProcessorAsync(long commandSequence, TaskProcessor taskProcessor, CancellationToken cancellationToken)
    {
        await StateManager.SetStateAsync(
                GetTaskProcessorStateName(commandSequence),
                taskProcessor,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task ProcessNextSubmittedCommandAsync(CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken)
           .ConfigureAwait(false);

        if (state.LastCommandProcessed >= state.CommandCount)
        {
            // All commands have been processed
            return;
        }

        long commandNumber = state.LastCommandProcessed + 1;

        LogProcessingCommandsInformation(Logger, Id.ToString(), Host.ActorTypeInfo.ActorTypeName, state.CommandCount, commandNumber);

        // Get the next command to process
        CommandState commandState = await CommandStore
            .GetAsync(commandNumber, CancellationToken.None)
            .ConfigureAwait(false);

        BaseCommand command = commandState.Message ?? throw new InvalidOperationException("The specified command state is missing associated message.");
        BaseMetadata metadata = commandState.Metadata ?? throw new InvalidOperationException("The specified command state is missing associated metadata.");

        if (command.AggregateId != Id.ToString())
        {
            throw new InvalidOperationException($"Command {command.TypeName} aggregate identifier '{commandState.Message.AggregateId}' is invalid: Expected : {Id}.");
        }

        if (GetAggregateActorName(command.AggregateName) != Host.ActorTypeInfo.ActorTypeName)
        {
            throw new InvalidOperationException($"Command {command.TypeName} for '{commandState.Message.AggregateId}' has an invalid aggregate actor name '{GetAggregateActorName(commandState.Message.AggregateId)}'. Expected : {Host.ActorTypeInfo.ActorTypeName}.");
        }

        IAggregate aggregate = await GetAggregateAsync(
                commandState.Message.AggregateName,
                CancellationToken.None)
            .ConfigureAwait(false);

        IEnumerable<BaseMessage> messages;
        IEnumerable<BaseMessage> integrationMessages = [];
        TaskProcessor taskProcessor = await GetTaskProcessorAsync(
            commandNumber,
            CancellationToken.None)
        .ConfigureAwait(false);

        if (taskProcessor.Status == TaskProcessorStatus.Suspended)
        {
            taskProcessor = taskProcessor.Continue();
        }

        if (taskProcessor.Status == TaskProcessorStatus.Suspended)
        {
            // The task processor is suspended, we can't process the command now
            return;
        }

        try
        {
            state.RetryOnFailurePeriod = null;
            state.RetryOnFailureDateTime = null;

            // Execute the commands
            messages = await _commandDispatcher
                    .DoAsync(command, aggregate, cancellationToken)
                    .ConfigureAwait(false);

            // Get aggregate events to persist in the event sourcing store
            BaseEvent[] aggregateEvents = messages
                .OfType<BaseEvent>()
                .Where(p => p.AggregateId == command.AggregateId && p.AggregateName == aggregate.AggregateName)
                .ToArray();

            // Apply events to aggregate
            (_aggregate, integrationMessages) = aggregate.Apply(aggregateEvents);
            EventState[] aggregateEventStates = aggregateEvents
                .Select(p => new EventState(_dateTimeService.UtcNow, p, Metadata.CreateNew(p, metadata)))
                .ToArray();

            // Persist events and messages
            state.EventSourceCount = await EventSourceStore
                .AddAsync(aggregateEventStates, state.EventSourceCount, cancellationToken)
                .ConfigureAwait(false);
            taskProcessor = taskProcessor.Complete();
            state.LastCommandProcessed = commandNumber;
            LogProcessedCommandInformation(
                Logger,
                command.TypeName,
                metadata.Message.Id,
                command.AggregateName,
                command.AggregateId);
        }
        catch (ApplicationErrorException ex)
        {
            (messages, taskProcessor) = await HandleCommandProcessingErrorAsync(
                taskProcessor,
                commandNumber,
                metadata.Context.CorrelationId,
                command,
                ex,
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            LogUnhandledCommandProcessingError(
                Logger,
                e,
                commandNumber,
                command.TypeName,
                command.AggregateName,
                command.AggregateId,
                metadata.Context.CorrelationId);

            throw;
        }

        await PersistTaskProcessorAsync(commandNumber, taskProcessor, cancellationToken).ConfigureAwait(false);

        // Get integration messages to persist in the message store
        MessageState[] messagesToSend = messages
            .OfType<BaseMessage>()
            .Where(p => !p.IsPrivateToAggregate)
            .Union(integrationMessages)
            .Select(p => new MessageState(_dateTimeService.UtcNow, p, Metadata.CreateNew(p, metadata)))
            .ToArray();

        if (messagesToSend.Length > 0)
        {
            state.MessageCount = await MessageStore.AddAsync(messagesToSend, state.MessageCount, cancellationToken).ConfigureAwait(false);
        }
    }
}