// <copyright file="DomainAggregateActorBase.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.States;
using Hexalith.PolymorphicSerialization;

using Microsoft.Extensions.Logging;

/// <summary>
/// The domain aggregate actor base class.
/// </summary>
public abstract partial class DomainAggregateActorBase : Actor, IRemindable, IDomainAggregateActor
{
    private const string _actorSuffix = "Aggregate";
    private readonly IDomainAggregateFactory _aggregateFactory;
    private readonly ICommandBus _commandBus;
    private readonly IDomainCommandDispatcher _commandDispatcher;
    private readonly TimeProvider _dateTimeService;
    private readonly TimeSpan _defaultTimerDueTime = TimeSpan.FromMilliseconds(1);
    private readonly IEventBus _eventBus;
    private readonly ActorHost _host;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly TimeSpan _maxTimerDueTime = new(0, 1, 0);
    private readonly IRequestBus _requestBus;
    private readonly IResiliencyPolicyProvider _resiliencyPolicyProvider;
    private IDomainAggregate? _aggregate;
    private MessageStore<MessageState>? _commandStore;
    private MessageStore<MessageState>? _eventStore;
    private MessageStore<MessageState>? _messageStore;
    private ResiliencyPolicy? _resiliencyPolicy;
    private AggregateActorState? _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainAggregateActorBase"/> class.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="commandDispatcher">The command dispatcher.</param>
    /// <param name="aggregateFactory">The aggregate factory.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="commandBus">The command bus.</param>
    /// <param name="requestBus">The request bus.</param>
    /// <param name="resiliencyPolicyProvider">The resiliency policy provider.</param>
    /// <param name="actorStateManager">The actor state manager.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    protected DomainAggregateActorBase(
        ActorHost host,
        IDomainCommandDispatcher commandDispatcher,
        IDomainAggregateFactory aggregateFactory,
        TimeProvider dateTimeService,
        IEventBus eventBus,
        ICommandBus commandBus,
        IRequestBus requestBus,
        IResiliencyPolicyProvider resiliencyPolicyProvider,
        IActorStateManager? actorStateManager = null)
       : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(commandDispatcher);
        ArgumentNullException.ThrowIfNull(aggregateFactory);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(commandBus);
        ArgumentNullException.ThrowIfNull(requestBus);
        ArgumentNullException.ThrowIfNull(resiliencyPolicyProvider);
        _host = host;
        _commandDispatcher = commandDispatcher;
        _aggregateFactory = aggregateFactory;
        _dateTimeService = dateTimeService;
        _eventBus = eventBus;
        _commandBus = commandBus;
        _requestBus = requestBus;
        _resiliencyPolicyProvider = resiliencyPolicyProvider;
        if (actorStateManager is not null)
        {
            StateManager = actorStateManager;
        }
    }

    private MessageStore<MessageState> CommandStore
#if EXPERIMENTAL_FEATURES
        => field
#else
        => _commandStore
#endif
        ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.CommandStoreName);

    private MessageStore<MessageState> EventSourceStore
#if EXPERIMENTAL_FEATURES
        => field
#else
        => _eventStore
#endif
        ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.EventSourcingName);

    private MessageStore<MessageState> MessageStore
#if EXPERIMENTAL_FEATURES
        => field
#else
        => _messageStore
#endif
        ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.MessageStoreName);

    private ResiliencyPolicy ResiliencyPolicy
#if EXPERIMENTAL_FEATURES
        => field
#else
        => _resiliencyPolicy
#endif
        ??= _resiliencyPolicyProvider.GetPolicy(Host.ActorTypeInfo.ActorTypeName);

    /// <summary>
    /// Gets the name of the aggregate actor.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>string.</returns>
    public static string GetAggregateActorName(string aggregateName) => aggregateName + _actorSuffix;

    /// <summary>
    /// Logs the accepted command information.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <param name="commandId">The command identifier.</param>
    /// <param name="partitionKey">Key of the aggregate.</param>
    [LoggerMessage(
                    EventId = 2,
                    Level = LogLevel.Information,
                    Message = "Accepted command {CommandType} ({CommandId}) for aggregate key {PartitionKey}.")]
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
        EventId = 5,
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
            EventId = 6,
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

    /// <inheritdoc/>
    public async Task ClearCommandsAsync()
    {
        AggregateActorState state = await GetAggregateStateAsync(CancellationToken.None).ConfigureAwait(false);
        state.CommandCount = 0;
        state.LastCommandProcessed = 0;
        await SaveAggregateStateAsync(CancellationToken.None).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<MessageState?> GetSnapshotEventAsync() => await GetSnapshotEventAsync(CancellationToken.None).ConfigureAwait(false);

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

    /// <inheritdoc/>
    public async Task PublishCallbackAsync() => await PublishNextMessageAsync().ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<bool> PublishNextMessageAsync()
    {
        CancellationToken cancellationToken = CancellationToken.None;
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        if (state.LastMessagePublished < state.MessageCount)
        {
            MessageState messageState = await MessageStore
                .GetAsync(state.LastMessagePublished + 1, cancellationToken)
                .ConfigureAwait(false);
            await PublishNextEmittedMessageAsync(
                    messageState,
                    state.LastMessagePublished + 1,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        await SetPublishCallbackAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveStateAsync()
            .ConfigureAwait(false);

        return state.LastMessagePublished < state.MessageCount;
    }

    /// <inheritdoc/>
    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        if (reminderName == ActorConstants.ProcessReminderName)
        {
            await ProcessCallbackAsync().ConfigureAwait(false);
        }

        if (reminderName == ActorConstants.PublishReminderName)
        {
            await PublishCallbackAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task SendSnapshotEventAsync()
    {
        MessageState? eventState = await GetSnapshotEventAsync(CancellationToken.None)
            .ConfigureAwait(false);
        if (eventState is not null)
        {
            await _eventBus.PublishAsync(eventState.Message, eventState.Metadata, CancellationToken.None).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsJsonAsync(string envelope)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(envelope);

        ActorMessageEnvelope? e = JsonSerializer.Deserialize<ActorMessageEnvelope>(envelope, PolymorphicHelper.DefaultJsonSerializerOptions)
            ?? throw new InvalidOperationException("The specified command envelope is invalid : " + envelope);
        await SubmitCommandAsync(e).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsStateAsync(MessageState envelope)
    {
        CancellationToken cancellationToken = CancellationToken.None;
        ArgumentNullException.ThrowIfNull(envelope);

        object command = envelope.Message;
        Metadata metadata = envelope.Metadata;
        if (GetAggregateActorName(metadata.Message.Aggregate.Name) != Host.ActorTypeInfo.ActorTypeName)
        {
            throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate name : {metadata.Message.Aggregate.Name}.");
        }

        if (metadata.AggregateGlobalId != Id.ToString())
        {
            throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid partition key : {metadata.AggregateGlobalId}.");
        }

        List<MessageState> commandStates = [new MessageState((PolymorphicRecordBase)command, metadata)];
        LogAcceptedCommandInformation(
            Logger,
            metadata.Message.Name,
            metadata.Message.Id,
            metadata.AggregateGlobalId);

        AggregateActorState state = await GetAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        state.CommandCount = await CommandStore
            .AddAsync(commandStates, state.CommandCount, cancellationToken)
            .ConfigureAwait(false);

        await SetProcessCallbackAsync(cancellationToken)
           .ConfigureAwait(false);
        await SetPublishCallbackAsync(cancellationToken)
           .ConfigureAwait(false);
        await SaveAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveStateAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ActorMessageEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        (object command, Metadata metadata) = envelope.Deserialize();
        await SubmitCommandAsStateAsync(new MessageState((PolymorphicRecordBase)command, metadata)).ConfigureAwait(false);
    }

    [LoggerMessage(
        EventId = 5,
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
        EventId = 7,
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
            EventId = 8,
            Level = LogLevel.Error,
            Message = "Unhandled command processing error. Command handlers should throw application error exceptions. Processing command number {CommandSequence}, type '{CommandType}', correlationId '{CorrelationId}' for aggregate key '{PartitionKey}'.")]
    private static partial void LogUnhandledCommandProcessingError(
        ILogger logger,
        Exception exception,
        long commandSequence,
        string commandType,
        string partitionKey,
        string correlationId);

    private async Task<IDomainAggregate> GetAggregateAsync(string aggregateName, CancellationToken cancellationToken)
    {
        if (_aggregate is null)
        {
            AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

            IDomainAggregate aggregate = _aggregateFactory.Create(aggregateName);
            for (int i = 1; i <= state.EventSourceCount; i++)
            {
                MessageState ev = await EventSourceStore.GetAsync(i, cancellationToken).ConfigureAwait(false);

                aggregate = aggregate.Apply(ev.Message).Aggregate;
            }

            _aggregate = aggregate;
        }

        return _aggregate;
    }

    private async Task<AggregateActorState> GetAggregateStateAsync(CancellationToken cancellationToken)
    {
        if (_state is null)
        {
            ConditionalValue<AggregateActorState> result = await StateManager
                .TryGetStateAsync<AggregateActorState>(ActorConstants.AggregateStateStoreName, cancellationToken)
                .ConfigureAwait(false);
            _state = result.HasValue ? result.Value : new AggregateActorState();
        }

        return _state;
    }

    private async Task<MessageState?> GetSnapshotEventAsync(CancellationToken cancellationToken)
    {
        string aggregateName = Host.ActorTypeInfo.ActorTypeName.Split(nameof(_actorSuffix)).First();
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

        IDomainAggregate aggregate = _aggregateFactory.Create(aggregateName);
        Metadata? metadata = null;
        for (int i = 1; i <= state.EventSourceCount; i++)
        {
            MessageState ev = await EventSourceStore.GetAsync(i, cancellationToken).ConfigureAwait(false);
            ApplyResult applyResult = aggregate.Apply(ev.Message);
            metadata = ev.Metadata;
            aggregate = applyResult.Aggregate;
        }

        if (!aggregate.IsInitialized() || metadata == null)
        {
            return null;
        }

        AggregateSnapshotEvent e = new(aggregate);
        MessageMetadata messageMetadata = new(e, _dateTimeService.GetUtcNow());
        return new MessageState(
            e,
            new Metadata(
                messageMetadata,
                new ContextMetadata(
                    messageMetadata.Id,
                    "system",
                    metadata.Context.PartitionId,
                    messageMetadata.CreatedDate,
                    null,
                    null,
                    [])));
    }

    private async Task<TaskProcessor> GetTaskProcessorAsync(long commandSequence, CancellationToken cancellationToken)
    {
        Dapr.Actors.Runtime.ConditionalValue<TaskProcessor> taskProcessorState = await StateManager
            .TryGetStateAsync<TaskProcessor>(GetTaskProcessorStateName(commandSequence), cancellationToken)
            .ConfigureAwait(false);
        if (taskProcessorState.HasValue)
        {
            return taskProcessorState.Value;
        }

        TaskProcessor task = new(_dateTimeService.GetUtcNow(), ResiliencyPolicy);
        return task.Start();
    }

    private string GetTaskProcessorStateName(long commandSequence) => $"{nameof(TaskProcessor)}-{commandSequence}";

    private async Task<TaskProcessor> HandleCommandProcessingErrorAsync(
        TaskProcessor taskProcessor,
        long commandSequence,
        string correlationId,
        object command,
        Metadata metadata,
        ApplicationErrorException ex,
        CancellationToken cancellationToken)
    {
        _ = taskProcessor.Failure;
        taskProcessor = taskProcessor.Fail(ex);
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        if (taskProcessor.Status == TaskProcessorStatus.Canceled)
        {
            state.LastCommandProcessed = commandSequence;
            state.RetryOnFailurePeriod = null;
            state.RetryOnFailureDateTime = null;
        }
        else
        {
            state.RetryOnFailurePeriod = taskProcessor.RetryPeriod;
            state.RetryOnFailureDateTime = _dateTimeService.GetUtcNow() + taskProcessor.RetryPeriod;
            LogTaskProcessorRetryInformation(
                Logger,
                metadata.Message.Name,
                commandSequence,
                correlationId,
                metadata.AggregateGlobalId,
                (taskProcessor.Failure?.Count ?? 0) + 1,
                state.RetryOnFailurePeriod.Value,
                state.RetryOnFailureDateTime.Value);
        }

        LogApplicationErrorWarning(
            Logger,
            commandSequence,
            metadata.Message.Name,
            metadata.AggregateGlobalId,
            correlationId,
            ex.Error?.GetDetailMessage(CultureInfo.InvariantCulture) ?? "Unknown application error.",
            ex.Error?.GetTechnicalMessage(CultureInfo.InvariantCulture));
        return taskProcessor;
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
        MessageState commandState = await CommandStore
            .GetAsync(commandNumber, CancellationToken.None)
            .ConfigureAwait(false);

        object command = commandState.Message ?? throw new InvalidOperationException("The specified command state is missing associated message.");
        Metadata metadata = commandState.Metadata ?? throw new InvalidOperationException("The specified command state is missing associated metadata.");

        if (metadata.AggregateGlobalId != Id.ToUnescapeString())
        {
            throw new InvalidOperationException($"Command {metadata.Message.Name} aggregate key '{metadata.AggregateGlobalId}' is invalid: Expected : {Id.ToUnescapeString()}.");
        }

        if (GetAggregateActorName(metadata.Message.Aggregate.Name) != Host.ActorTypeInfo.ActorTypeName)
        {
            throw new InvalidOperationException($"Command {metadata.Message.Name} for '{metadata.AggregateGlobalId}' has an invalid aggregate actor name '{GetAggregateActorName(metadata.Message.Aggregate.Name)}'. Expected : {Host.ActorTypeInfo.ActorTypeName}.");
        }

        IDomainAggregate aggregate = await GetAggregateAsync(
                metadata.Message.Aggregate.Name,
                CancellationToken.None)
            .ConfigureAwait(false);

        ExecuteCommandResult? commandResult = null;
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
            commandResult = await _commandDispatcher
                    .DoAsync(command, metadata, aggregate, cancellationToken)
                    .ConfigureAwait(false);

            // Get aggregate events to persist in the event sourcing store
            MessageState[] aggregateMessageStates = commandResult.SourceEvents
                .Select(p => new MessageState((PolymorphicRecordBase)p, Metadata.CreateNew(p, metadata, _dateTimeService.GetUtcNow())))
                .ToArray();

            // Persist events and messages
            state.EventSourceCount = await EventSourceStore
                .AddAsync(aggregateMessageStates, state.EventSourceCount, cancellationToken)
                .ConfigureAwait(false);
            taskProcessor = taskProcessor.Complete();
            state.LastCommandProcessed = commandNumber;
            LogProcessedCommandInformation(
                Logger,
                metadata.Message.Name,
                metadata.Message.Id,
                metadata.AggregateGlobalId);
        }
        catch (ApplicationErrorException ex)
        {
            taskProcessor = await HandleCommandProcessingErrorAsync(
                taskProcessor,
                commandNumber,
                metadata.Context.CorrelationId,
                command,
                metadata,
                ex,
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            LogUnhandledCommandProcessingError(
                Logger,
                e,
                commandNumber,
                metadata.Message.Name,
                metadata.AggregateGlobalId,
                metadata.Context.CorrelationId);

            throw;
        }

        await PersistTaskProcessorAsync(commandNumber, taskProcessor, cancellationToken).ConfigureAwait(false);
        if (commandResult is not null)
        {
            // Get integration messages to persist in the message store
            MessageState[] messagesToSend = commandResult.SourceEvents
                .Union(commandResult.IntegrationEvents)
                .Select(p => new MessageState((PolymorphicRecordBase)p, Metadata.CreateNew(p, metadata, _dateTimeService.GetUtcNow())))
                .ToArray();

            if (messagesToSend.Length > 0)
            {
                state.MessageCount = await MessageStore
                    .AddAsync(messagesToSend, state.MessageCount, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }

    private async Task PublishNextEmittedMessageAsync(MessageState messageState, long sequence, CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        state.PublishFailed = false;
        try
        {
            await _eventBus.PublishAsync(messageState.Message, messageState.Metadata, cancellationToken).ConfigureAwait(false);
            state.LastMessagePublished = sequence;
            return;
        }
        catch (Exception ex)
        {
            state.PublishFailed = true;
            LogPublishError(
                Logger,
                ex,
                sequence,
                messageState.Metadata?.Message.Id,
                messageState.Metadata?.Context.CorrelationId,
                Id.ToString(),
                Host.ActorTypeInfo.ActorTypeName,
                ex.FullMessage());
        }
    }

    private async Task SaveAggregateStateAsync(CancellationToken cancellationToken)
    {
        if (_state is not null)
        {
            await StateManager
                .SetStateAsync(ActorConstants.AggregateStateStoreName, _state, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task SetProcessCallbackAsync(CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

        TimeSpan timerWaitTime = state.RetryOnFailurePeriod ?? _defaultTimerDueTime;
        TimeSpan reminderWaitTime = state.RetryOnFailurePeriod ?? _maxTimerDueTime;
        reminderWaitTime = reminderWaitTime < _maxTimerDueTime ? _maxTimerDueTime : reminderWaitTime;

        if (state.LastCommandProcessed < state.CommandCount)
        {
            if (state.ProcessReminderDueTime == null)
            {
                _ = await RegisterReminderAsync(ActorConstants.ProcessReminderName, null, reminderWaitTime, reminderWaitTime).ConfigureAwait(false);
                state.ProcessReminderDueTime = reminderWaitTime;
            }

            _ = await RegisterTimerAsync(ActorConstants.ProcessTimerName, nameof(ProcessCallbackAsync), null, timerWaitTime, TimeSpan.FromMilliseconds(-1)).ConfigureAwait(false);
        }
        else
        {
            if (state.ProcessReminderDueTime is not null)
            {
                await UnregisterReminderAsync(ActorConstants.ProcessReminderName).ConfigureAwait(false);
                state.ProcessReminderDueTime = null;
            }
        }
    }

    private async Task SetPublishCallbackAsync(CancellationToken cancellationToken)
    {
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);
        TimeSpan timerWaitTime = timerWaitTime = state.PublishFailed ? _maxTimerDueTime : _defaultTimerDueTime;
        TimeSpan reminderWaitTime = _maxTimerDueTime;

        if (state.LastMessagePublished < state.MessageCount)
        {
            if (state.PublishReminderDueTime == null)
            {
                _ = await RegisterReminderAsync(ActorConstants.PublishReminderName, null, reminderWaitTime, reminderWaitTime).ConfigureAwait(false);
                state.PublishReminderDueTime = reminderWaitTime;
            }

            _ = await RegisterTimerAsync(ActorConstants.PublishTimerName, nameof(PublishCallbackAsync), null, timerWaitTime, TimeSpan.FromMilliseconds(-1)).ConfigureAwait(false);
        }
        else
        {
            if (state.PublishReminderDueTime is not null)
            {
                await UnregisterReminderAsync(ActorConstants.PublishReminderName).ConfigureAwait(false);
                state.PublishReminderDueTime = null;
            }
        }
    }
}