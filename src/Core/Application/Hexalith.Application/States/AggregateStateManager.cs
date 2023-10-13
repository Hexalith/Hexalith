// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-27-2023
// ***********************************************************************
// <copyright file="AggregateStateManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class AggregateActorState.
/// Implements the <see cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />.
/// </summary>
/// <seealso cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />
public class AggregateStateManager : IAggregateStateManager
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The dispatcher.
    /// </summary>
    private readonly ICommandDispatcher _dispatcher;

    /// <summary>
    /// The event bus.
    /// </summary>
    private readonly IEventBus _eventBus;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<AggregateStateManager> _logger;

    /// <summary>
    /// The notification bus.
    /// </summary>
    private readonly INotificationBus _notificationBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateStateManager"/> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public AggregateStateManager(
        ICommandDispatcher dispatcher,
        IEventBus eventBus,
        INotificationBus notificationBus,
        IDateTimeService dateTimeService,
        ILogger<AggregateStateManager> logger)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(notificationBus);
        ArgumentNullException.ThrowIfNull(logger);
        _dateTimeService = dateTimeService;
        _logger = logger;
        _dispatcher = dispatcher;
        _eventBus = eventBus;
        _notificationBus = notificationBus;
    }

    /// <summary>
    /// Gets to do state name.
    /// </summary>
    /// <value>The name of to do state.</value>
    public virtual string CommandsStreamName => "Commands";

    /// <summary>
    /// Gets to do state name.
    /// </summary>
    /// <value>The name of to do state.</value>
    public virtual string EventsStreamName => "Events";

    /// <summary>
    /// Gets the name of the resiliency policy state.
    /// </summary>
    /// <value>The name of the resiliency policy state.</value>
    public virtual string ResiliencyPolicyStateName => nameof(ResiliencyPolicy);

    /// <summary>
    /// Gets the separator.
    /// </summary>
    /// <value>The separator.</value>
    public virtual string Separator => "-";

    /// <summary>
    /// Gets the name of the state.
    /// </summary>
    /// <value>The name of the state.</value>
    public virtual string StateName => "State";

    /// <inheritdoc/>
    public async Task AddCommandAsync(
        [NotNull] IStateStoreProvider stateProvider,
        [NotNull] IRetryCallbackManager retryManager,
        [NotNull] BaseCommand[] commands,
        [NotNull] BaseMetadata[] metadatas,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(metadatas);
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(retryManager);

        await retryManager.RegisterContinueCallbackAsync(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        MessageStore<CommandState> commandStore = new(stateProvider, CommandsStreamName);
        List<CommandState> states = [];
        for (int i = 0; i < commands.Length; i++)
        {
            BaseCommand command = commands[i];
            states.Add(new CommandState(_dateTimeService.UtcNow, command, metadatas[i]));
            _logger.LogInformation(
                "Receiving command {CommandName} for aggregate {AggregateName} with id '{AggregateId}'.",
                command.TypeName,
                command.AggregateName,
                command.AggregateId);
        }

        long version = await commandStore
            .AddAsync(
                states,
                state.CommandStreamVersion,
                cancellationToken)
            .ConfigureAwait(false);

        await PersistStateAsync(
                stateProvider,
                new AggregateState(
                    version,
                    state.EventStreamVersion,
                    state.LastCommandDone,
                    state.LastEventPublished),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IAggregate?> ContinueAsync(
        [NotNull] IStateStoreProvider stateProvider,
        [NotNull] IRetryCallbackManager retryManager,
        [NotNull] ResiliencyPolicy resiliencyPolicy,
        IAggregate? aggregate,
        [NotNull] Func<BaseEvent, IAggregate> aggregateInitializer,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(retryManager);
        ArgumentNullException.ThrowIfNull(resiliencyPolicy);
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(aggregateInitializer);

        (TimeSpan? retry, aggregate) = await ExecuteCommandsAsync(stateProvider, aggregate, aggregateInitializer, resiliencyPolicy, cancellationToken).ConfigureAwait(false);
        await PublishEventsAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        if (state.LastEventPublished < state.EventStreamVersion)
        {
            await retryManager.RegisterContinueCallbackAsync(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);
            return aggregate;
        }

        if (retry != null)
        {
            await retryManager
                .RegisterContinueCallbackAsync(
                    retry.Value <= resiliencyPolicy.InitialPeriod ? resiliencyPolicy.InitialPeriod : retry.Value,
                    cancellationToken)
                .ConfigureAwait(false);
            return null;
        }

        if (aggregate != null)
        {
            await retryManager.UnregisterContinueCallbackAsync(cancellationToken).ConfigureAwait(false);
        }

        return aggregate;
    }

    /// <inheritdoc/>
    public async Task<IAggregate?> GetAggregateAsync(
        [NotNull] IStateStoreProvider stateProvider,
        [NotNull] Func<BaseEvent, IAggregate> aggregateInitializer,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(aggregateInitializer);
        ArgumentNullException.ThrowIfNull(stateProvider);

        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        IAggregate? aggregate = default;
        for (long nextEvent = 1; nextEvent <= state.EventStreamVersion; nextEvent++)
        {
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken).ConfigureAwait(false);
            BaseEvent e = eventState.Message ?? throw new InvalidDataException($"Message is null in event stream at position {nextEvent}. IdempotencyId={eventState.IdempotencyId}.");
            aggregate = aggregate?.Apply(e) ?? aggregateInitializer(e);
        }

        return aggregate;
    }

    /// <summary>
    /// Get command count as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int64&gt; representing the asynchronous operation.</returns>
    public async Task<long> GetCommandCountAsync([NotNull] IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        return state.CommandStreamVersion;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetCommandsAsync<T>([NotNull] IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        MessageStore<CommandState> commandStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        List<T> commands = [];
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            CommandState eventState = await commandStore.GetAsync(nextEvent, cancellationToken).ConfigureAwait(false);
            if (eventState is T e)
            {
                commands.Add(e);
            }
        }

        return commands;
    }

    /// <inheritdoc/>
    public async Task<long> GetEventCountAsync([NotNull] IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        return state.EventStreamVersion;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetEventsAsync<T>([NotNull] IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        List<T> events = [];
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken).ConfigureAwait(false);
            if (eventState is T e)
            {
                events.Add(e);
            }
        }

        return events;
    }

    /// <summary>
    /// Execute commands as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="aggregateInitializer">The aggregate initializer.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">Command {nextCommand} content is null.</exception>
    /// <exception cref="System.InvalidOperationException">Command {nextCommand} metadata is null.</exception>
    protected async Task<(TimeSpan? WaitUntil, IAggregate? Aggregate)> ExecuteCommandsAsync(
        [NotNull] IStateStoreProvider stateProvider,
        IAggregate? aggregate,
        [NotNull] Func<BaseEvent, IAggregate> aggregateInitializer,
        [NotNull] ResiliencyPolicy resiliencyPolicy,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(aggregateInitializer);
        ArgumentNullException.ThrowIfNull(resiliencyPolicy);

        MessageStore<CommandState> commandStore = new(stateProvider, CommandsStreamName);
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        while (state.LastCommandDone < state.CommandStreamVersion)
        {
            long nextCommand = state.LastCommandDone + 1;
            CommandState command = await commandStore.GetAsync(nextCommand, cancellationToken).ConfigureAwait(false);
            _ = command.Message ?? throw new InvalidOperationException($"Command {nextCommand} content is null.");
            _ = command.Metadata ?? throw new InvalidOperationException($"Command {nextCommand} metadata is null.");
            ResilientCommandProcessor processor = new(
                resiliencyPolicy,
                _dispatcher,
                stateProvider,
                _logger);

            (TimeSpan? retry, IEnumerable<BaseMessage> messages) = await processor.ProcessAsync(nextCommand.ToInvariantString(), command.Message, cancellationToken).ConfigureAwait(false);
            BaseEvent[] events = messages.OfType<BaseEvent>().ToArray();
            long eventVersion = state.EventStreamVersion;
            if (events.Length != 0)
            {
                aggregate = aggregate?.Apply(events)
                    ?? new AggregateBuilder()
                        .Initializer(aggregateInitializer)
                        .Events(events)
                        .Build();
                IEnumerable<EventState> eventStates = events.Select(
                        p => new EventState(
                            _dateTimeService.UtcNow,
                            p,
                            Metadata.CreateNew(p, command.Metadata, _dateTimeService.UtcNow)));
                eventVersion = await eventStore.AddAsync(
                    eventStates,
                    state.EventStreamVersion,
                    cancellationToken).ConfigureAwait(false);
            }

            state = new AggregateState(
                    state.CommandStreamVersion,
                    eventVersion,
                    retry == null ? nextCommand : state.LastCommandDone,
                    state.LastEventPublished);
            await PersistStateAsync(
                stateProvider,
                state,
                cancellationToken).ConfigureAwait(false);
            if (retry != null)
            {
                return (retry, aggregate);
            }
        }

        return (null, aggregate);
    }

    /// <summary>
    /// Publish events as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected async Task PublishEventsAsync(
        IStateStoreProvider stateProvider,
        CancellationToken cancellationToken)
    {
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken).ConfigureAwait(false);

            await _eventBus.PublishAsync(eventState, cancellationToken).ConfigureAwait(false);
            state = new AggregateState(
                    state.CommandStreamVersion,
                    state.EventStreamVersion,
                    state.LastCommandDone,
                    nextEvent);
            await PersistStateAsync(
                stateProvider,
                state,
                cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Get state as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AggregateState&gt; representing the asynchronous operation.</returns>
    private async Task<AggregateState> GetStateAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ConditionalValue<AggregateState> result = await stateProvider.TryGetStateAsync<AggregateState>(StateName, cancellationToken).ConfigureAwait(false);
        return result.HasValue ? result.Value : new AggregateState(0, 0, 0, 0);
    }

    /// <summary>
    /// Gets the name of the task processor state.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <returns>System.String.</returns>
    private string GetTaskProcessorStateName(long version) => nameof(TaskProcessor) + version.ToInvariantString();

    /// <summary>
    /// Set state as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task PersistStateAsync(IStateStoreProvider stateProvider, AggregateState state, CancellationToken cancellationToken)
    {
        await stateProvider.SetStateAsync(StateName, state, CancellationToken.None).ConfigureAwait(false);
        await stateProvider.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}