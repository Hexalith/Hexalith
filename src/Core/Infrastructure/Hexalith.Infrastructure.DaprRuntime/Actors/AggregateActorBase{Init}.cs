// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActorBase{Init}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Abstractions;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.States;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public abstract partial class AggregateActorBase : Actor, IRemindable, IAggregateActor
{
    private readonly IAggregateFactory _aggregateFactory;
    private readonly ICommandBus _commandBus;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IDateTimeService _dateTimeService;
    private readonly TimeSpan _defaultTimerDueTime = TimeSpan.FromMilliseconds(1);
    private readonly IEventBus _eventBus;
    private readonly ActorHost _host;
    private readonly TimeSpan _maxTimerDueTime = new(0, 1, 0);
    private readonly INotificationBus _notificationBus;
    private readonly IRequestBus _requestBus;
    private readonly IResiliencyPolicyProvider _resiliencyPolicyProvider;

    [Obsolete]
    private IAggregate? _aggregate;

    private MessageStore<CommandState>? _commandStore;
    private MessageStore<EventState>? _eventSourceStore;

    [Obsolete]
    private MessageStore<MessageState>? _messageStore;

    private ResiliencyPolicy? _resiliencyPolicy;
    private AggregateActorState? _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActorBase"/> class.
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
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected AggregateActorBase(
        ActorHost host,
        ICommandDispatcher commandDispatcher,
        IAggregateFactory aggregateFactory,
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

    private MessageStore<CommandState> CommandStore
        => _commandStore ??= new MessageStore<CommandState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.CommandStoreName);

    private MessageStore<EventState> EventSourceStore
        => _eventSourceStore ??= new MessageStore<EventState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.EventSourcingName);

    [Obsolete]
    private MessageStore<MessageState> MessageStore
        => _messageStore ??= new MessageStore<MessageState>(
            new ActorStateStoreProvider(StateManager),
            ActorConstants.MessageStoreName);

    private ResiliencyPolicy ResiliencyPolicy
                    => _resiliencyPolicy ??= _resiliencyPolicyProvider.GetPolicy(Host.ActorTypeInfo.ActorTypeName);

    /// <summary>
    /// Gets the name of the aggregate actor.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>string.</returns>
    [Obsolete]
    public static string GetAggregateActorName(string aggregateName) => aggregateName + nameof(Aggregate);

    /// <inheritdoc/>
    public async Task ClearCommandsAsync()
    {
        AggregateActorState state = await GetAggregateStateAsync(CancellationToken.None).ConfigureAwait(false);
        state.CommandCount = 0;
        state.LastCommandProcessed = 0;
        await SaveAggregateStateAsync(CancellationToken.None).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [Obsolete]
    public async Task<EventState?> GetSnapshotEventAsync() => await GetSnapshotEventAsync(CancellationToken.None).ConfigureAwait(false);

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
    [Obsolete]
    public async Task SendSnapshotEventAsync()
    {
        EventState? eventState = await GetSnapshotEventAsync(CancellationToken.None)
            .ConfigureAwait(false);
        if (eventState is not null)
        {
            await _eventBus.PublishAsync(eventState, CancellationToken.None).ConfigureAwait(false);
        }
    }

    [Obsolete]
    private async Task<IAggregate> GetAggregateAsync(string aggregateName, CancellationToken cancellationToken)
    {
        if (_aggregate is null)
        {
            AggregateActorState state = await GetAggregateStateAsync(cancellationToken).ConfigureAwait(false);

            _aggregate = _aggregateFactory.Create(aggregateName);
            for (int i = 1; i <= state.EventSourceCount; i++)
            {
                EventState ev = await EventSourceStore.GetAsync(i, cancellationToken).ConfigureAwait(false);
                if (ev.Message is null)
                {
                    throw new InvalidOperationException($"Event {i} for {Id} is null.");
                }

                if (ev.Message.AggregateId != Id.ToString())
                {
                    throw new InvalidOperationException($"Event {i} for {Id} has an invalid aggregate id : {ev.Message.AggregateId}.");
                }

                (_aggregate, _) = _aggregate.Apply(ev.Message);
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

    [Obsolete]
    private async Task<EventState?> GetSnapshotEventAsync(CancellationToken cancellationToken)
    {
        string aggregateName = Host.ActorTypeInfo.ActorTypeName.Split(nameof(Aggregate)).First();
        IAggregate aggregate = await GetAggregateAsync(aggregateName, cancellationToken).ConfigureAwait(false);
        if (!aggregate.IsInitialized())
        {
            return null;
        }

        SnapshotEvent e = new(aggregate);
        return new EventState(
            _dateTimeService.UtcNow,
            e,
            new Metadata(
                new MessageMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    _dateTimeService.UtcNow,
                    e),
                new ContextMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    "system",
                    _dateTimeService.UtcNow,
                    null,
                    null),
                null));
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