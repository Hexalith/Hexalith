// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="DomainAggregateActorBase{Init}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.MessageMetadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprRuntime.Abstractions;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;
using Hexalith.Infrastructure.DaprRuntime.States;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public abstract partial class DomainAggregateActorBase : Actor, IRemindable, IDomainAggregateActor
{
    private const string ActorSuffix = "Aggregate";
    private readonly IDomainAggregateFactory _aggregateFactory;
    private readonly ICommandBus _commandBus;
    private readonly IDomainCommandDispatcher _commandDispatcher;
    private readonly IDateTimeService _dateTimeService;
    private readonly TimeSpan _defaultTimerDueTime = TimeSpan.FromMilliseconds(1);
    private readonly IEventBus _eventBus;
    private readonly ActorHost _host;
    private readonly TimeSpan _maxTimerDueTime = new(0, 1, 0);
    private readonly INotificationBus _notificationBus;
    private readonly IRequestBus _requestBus;
    private readonly IResiliencyPolicyProvider _resiliencyPolicyProvider;

    private IDomainAggregate? _aggregate;

    private MessageStore<Application.MessageMetadatas.MessageState>? _commandStore;
    private MessageStore<Application.MessageMetadatas.MessageState>? _eventSourceStore;
    private MessageStore<Application.MessageMetadatas.MessageState>? _messageStore;

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
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="commandBus">The command bus.</param>
    /// <param name="requestBus">The request bus.</param>
    /// <param name="resiliencyPolicyProvider">The resiliency policy provider.</param>
    /// <param name="actorStateManager">The actor state manager.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    protected DomainAggregateActorBase(
        ActorHost host,
        IDomainCommandDispatcher commandDispatcher,
        IDomainAggregateFactory aggregateFactory,
        IDateTimeService dateTimeService,
        IEventBus eventBus,
        INotificationBus notificationBus,
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
        ArgumentNullException.ThrowIfNull(notificationBus);
        ArgumentNullException.ThrowIfNull(commandBus);
        ArgumentNullException.ThrowIfNull(requestBus);
        ArgumentNullException.ThrowIfNull(resiliencyPolicyProvider);
        _host = host;
        _commandDispatcher = commandDispatcher;
        _aggregateFactory = aggregateFactory;
        _dateTimeService = dateTimeService;
        _eventBus = eventBus;
        _notificationBus = notificationBus;
        _commandBus = commandBus;
        _requestBus = requestBus;
        _resiliencyPolicyProvider = resiliencyPolicyProvider;
        if (actorStateManager is not null)
        {
            StateManager = actorStateManager;
        }
    }

    private MessageStore<Application.MessageMetadatas.MessageState> CommandStore
        => _commandStore ??= new MessageStore<Application.MessageMetadatas.MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.CommandStoreName);

    private MessageStore<Application.MessageMetadatas.MessageState> EventSourceStore
        => _eventSourceStore ??= new MessageStore<Application.MessageMetadatas.MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.EventSourcingName);

    private MessageStore<Application.MessageMetadatas.MessageState> MessageStore
        => _messageStore ??= new MessageStore<Application.MessageMetadatas.MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.MessageStoreName);

    private ResiliencyPolicy ResiliencyPolicy
                    => _resiliencyPolicy ??= _resiliencyPolicyProvider.GetPolicy(Host.ActorTypeInfo.ActorTypeName);

    /// <summary>
    /// Gets the name of the aggregate actor.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>string.</returns>
    public static string GetAggregateActorName(string aggregateName) => aggregateName + ActorSuffix;

    /// <inheritdoc/>
    public async Task ClearCommandsAsync()
    {
        AggregateActorState state = await GetAggregateStateAsync(CancellationToken.None).ConfigureAwait(false);
        state.CommandCount = 0;
        state.LastCommandProcessed = 0;
        await SaveAggregateStateAsync(CancellationToken.None).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Application.MessageMetadatas.MessageState?> GetSnapshotEventAsync() => await GetSnapshotEventAsync(CancellationToken.None).ConfigureAwait(false);

    /// <inheritdoc/>
    public Task ProcessCallbackAsync() => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<bool> ProcessNextCommandAsync() => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task PublishCallbackAsync() => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<bool> PublishNextMessageAsync() => throw new NotImplementedException();

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
            await _eventBus.PublishAsync(eventState, CancellationToken.None).ConfigureAwait(false);
        }
    }

    private async Task<IDomainAggregate> GetAggregateAsync(string aggregateName, CancellationToken cancellationToken)
    {
        if (_aggregate is null)
        {
            AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

            _aggregate = _aggregateFactory.Create(aggregateName);
            for (int i = 1; i <= state.EventSourceCount; i++)
            {
                Application.MessageMetadatas.MessageState ev = await EventSourceStore.GetAsync(i, cancellationToken).ConfigureAwait(false);
                if (ev.Message is null)
                {
                    throw new InvalidOperationException($"Event {i} for {Id} is null.");
                }

                if (ev.Metadata.Message.Aggregate.Id != Id.ToString())
                {
                    throw new InvalidOperationException($"Event {i} for {Id} has an invalid aggregate id : {ev.Metadata.Message.Aggregate.Id}.");
                }

                _ = _aggregate.Apply(ev.Message);
            }
        }

        return _aggregate;
    }

    private async Task<AggregateActorState> GetAggregateStateAsync(CancellationToken cancellationToken)
    {
        if (_state is null)
        {
            Dapr.Actors.Runtime.ConditionalValue<AggregateActorState> result = await StateManager
                .TryGetStateAsync<AggregateActorState>(ActorConstants.AggregateStateStoreName, cancellationToken)
                .ConfigureAwait(false);
            _state = result.HasValue ? result.Value : new AggregateActorState();
        }

        return _state;
    }

    private async Task<Application.MessageMetadatas.MessageState?> GetSnapshotEventAsync(CancellationToken cancellationToken)
    {
        string aggregateName = Host.ActorTypeInfo.ActorTypeName.Split(nameof(ActorSuffix)).First();
        IDomainAggregate aggregate = await GetAggregateAsync(aggregateName, cancellationToken).ConfigureAwait(false);
        if (!aggregate.IsInitialized())
        {
            return null;
        }

        AggregateSnapshotEvent e = new(aggregate);
        Application.MessageMetadatas.MessageMetadata messageMetadata = new(e, _dateTimeService.UtcNow);
        return new MessageState(
            e,
            new Application.MessageMetadatas.Metadata(
                messageMetadata,
                new Application.MessageMetadatas.ContextMetadata(
                    messageMetadata.Id,
                    "system",
                    messageMetadata.CreatedDate,
                    null,
                    null,
                    [])));
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
}