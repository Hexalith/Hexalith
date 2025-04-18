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
using Hexalith.Application.States;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Events;
using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.States;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// The domain aggregate actor base class.
/// </summary>
public abstract partial class DomainAggregateActorBase : Actor, IRemindable, IDomainAggregateActor
{
    private readonly IDomainAggregateFactory _aggregateFactory;
    private readonly IDomainCommandDispatcher _commandDispatcher;
    private readonly TimeProvider _dateTimeService;
    private readonly TimeSpan _defaultTimerDueTime = TimeSpan.FromMilliseconds(1);
    private readonly IEventBus _eventBus;
    private readonly TimeSpan _maxTimerDueTime = new(0, 1, 0);
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
    /// <param name="resiliencyPolicyProvider">The resiliency policy provider.</param>
    /// <param name="actorStateManager">The actor state manager.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    protected DomainAggregateActorBase(
        ActorHost host,
        IDomainCommandDispatcher commandDispatcher,
        IDomainAggregateFactory aggregateFactory,
        TimeProvider dateTimeService,
        IEventBus eventBus,
        IResiliencyPolicyProvider resiliencyPolicyProvider,
        IActorStateManager? actorStateManager = null)
       : base(host)
    {
        ArgumentNullException.ThrowIfNull(commandDispatcher);
        ArgumentNullException.ThrowIfNull(aggregateFactory);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(resiliencyPolicyProvider);
        _commandDispatcher = commandDispatcher;
        _aggregateFactory = aggregateFactory;
        _dateTimeService = dateTimeService;
        _eventBus = eventBus;
        _resiliencyPolicyProvider = resiliencyPolicyProvider;
        if (actorStateManager is not null)
        {
            StateManager = actorStateManager;
        }
    }

    private MessageStore<MessageState> CommandStore
        => _commandStore
        ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.CommandStoreName);

    private MessageStore<MessageState> EventSourceStore
        => _eventStore
        ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.EventSourcingName);

    private MessageStore<MessageState> MessageStore
        => _messageStore
        ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.MessageStoreName);

    private ResiliencyPolicy ResiliencyPolicy
        => _resiliencyPolicy
        ??= _resiliencyPolicyProvider.GetPolicy(Host.ActorTypeInfo.ActorTypeName);

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
            await _eventBus.PublishAsync(eventState.MessageObject, eventState.Metadata, CancellationToken.None).ConfigureAwait(false);
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

        object command = envelope.MessageObject;
        Metadata metadata = envelope.Metadata;
        if (DaprActorHelper.ToAggregateActorName(metadata.Message.Aggregate.Name) != Host.ActorTypeInfo.ActorTypeName)
        {
            throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate name : {metadata.Message.Aggregate.Name}.");
        }

        if (metadata.AggregateGlobalId != Id.ToUnescapeString())
        {
            throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid partition key : {metadata.AggregateGlobalId}.");
        }

        List<MessageState> commandStates = [new MessageState((Polymorphic)command, metadata)];
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

        await SaveAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveStateAsync().ConfigureAwait(false);
        _ = await ProcessNextCommandAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ActorMessageEnvelope envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        (object command, Metadata metadata) = envelope.Deserialize();
        await SubmitCommandAsStateAsync(new MessageState((Polymorphic)command, metadata)).ConfigureAwait(false);
    }

    private async Task<IDomainAggregate> GetAggregateAsync(string aggregateName, CancellationToken cancellationToken)
    {
        if (_aggregate is null)
        {
            AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

            IDomainAggregate aggregate = _aggregateFactory.Create(aggregateName);
            for (int i = 1; i <= state.EventSourceCount; i++)
            {
                MessageState ev = await EventSourceStore.GetAsync(i, cancellationToken).ConfigureAwait(false);

                aggregate = aggregate.Apply(ev.MessageObject).Aggregate;
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
        string aggregateName = Host.ActorTypeInfo.ToAggregateName();
        AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

        IDomainAggregate aggregate = _aggregateFactory.Create(aggregateName);
        Metadata? metadata = null;
        for (int i = 1; i <= state.EventSourceCount; i++)
        {
            MessageState ev = await EventSourceStore.GetAsync(i, cancellationToken).ConfigureAwait(false);
            ApplyResult applyResult = aggregate.Apply(ev.MessageObject);
            if (applyResult.Failed)
            {
                throw new InvalidOperationException($"Error while applying event {ev.Metadata.Message.Name} to aggregate {aggregateName} : {applyResult.Reason}");
            }

            metadata = ev.Metadata;
            aggregate = applyResult.Aggregate;
        }

        if (!aggregate.IsInitialized() || metadata == null)
        {
            return null;
        }

        SnapshotEvent e = SnapshotEvent.Create(aggregate);
        MessageMetadata messageMetadata = MessageMetadata.Create(e, _dateTimeService.GetUtcNow());
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
            ex.Error?.GetDetailMessage(CultureInfo.InvariantCulture) ?? $"Unknown application error on {command.GetType().Name}.",
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

        Polymorphic command = commandState.MessageObject ?? throw new InvalidOperationException("The specified command state is missing associated message.");
        Metadata metadata = commandState.Metadata ?? throw new InvalidOperationException("The specified command state is missing associated metadata.");

        if (metadata.AggregateGlobalId != Id.ToUnescapeString())
        {
            throw new InvalidOperationException($"Command {metadata.Message.Name} aggregate key '{metadata.AggregateGlobalId}' is invalid: Expected : {Id.ToUnescapeString()}.");
        }

        if (string.IsNullOrWhiteSpace(metadata.Message.Aggregate.Id) || string.IsNullOrWhiteSpace(metadata.Message.Aggregate.Name))
        {
            throw new InvalidOperationException($"Command {metadata.Message.Name} aggregate Name='{metadata.Message.Aggregate.Name}' Id='{metadata.Message.Aggregate.Id}' is invalid.");
        }

        if (DaprActorHelper.ToAggregateActorName(metadata.Message.Aggregate.Name) != Host.ActorTypeInfo.ActorTypeName)
        {
            throw new InvalidOperationException($"Command {metadata.Message.Name} for '{metadata.AggregateGlobalId}' has an invalid aggregate actor name '{DaprActorHelper.ToAggregateActorName(metadata.Message.Aggregate.Name)}'. Expected : {Host.ActorTypeInfo.ActorTypeName}.");
        }

        IDomainAggregate? aggregate = await GetAggregateAsync(
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

            // Check if the command has been executed successfully and update the aggregate to the new state
            _aggregate = commandResult.Failed ? (aggregate = null) : (aggregate = commandResult.Aggregate);

            // Get aggregate events to persist in the event sourcing store
            MessageState[] aggregateMessageStates = [..
                commandResult
                .SourceEvents
                .Select(p => new MessageState(
                    (Polymorphic)p,
                    Metadata.CreateNew(p, metadata, _dateTimeService.GetUtcNow())))];

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
            _aggregate = null;
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
            _aggregate = null;
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
            MessageState[] messagesToSend = [.. commandResult.SourceEvents
                .Union(commandResult.IntegrationEvents)
                .Select(p => new MessageState((Polymorphic)p, Metadata.CreateNew(p, metadata, _dateTimeService.GetUtcNow())))];

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
            await _eventBus.PublishAsync(messageState.MessageObject, messageState.Metadata, cancellationToken).ConfigureAwait(false);
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
        TimeSpan timerWaitTime = state.PublishFailed ? _maxTimerDueTime : _defaultTimerDueTime;
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