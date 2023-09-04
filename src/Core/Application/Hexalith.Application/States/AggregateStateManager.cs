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

/// <summary>
/// Class AggregateActorState.
/// Implements the <see cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />.
/// </summary>
/// <seealso cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />
public class AggregateStateManager : IAggregateStateManager
{
    private const string ReminderName = "Continue";

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
    /// <exception cref="System.ArgumentNullException">Null arguments.</exception>
    public AggregateStateManager(
        ICommandDispatcher dispatcher,
        IEventBus eventBus,
        INotificationBus notificationBus,
        IDateTimeService dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(notificationBus);
        _dateTimeService = dateTimeService;
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

    /// <summary>
    /// Add command as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="commands">The commands.</param>
    /// <param name="metadatas">The metadatas.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Null arguments.</exception>
    public async Task AddCommandAsync(
        IStateStoreProvider stateProvider,
        BaseCommand[] commands,
        BaseMetadata[] metadatas,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(metadatas);
        ArgumentNullException.ThrowIfNull(commands);
        await SetReminderAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), registerReminder);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        MessageStore<CommandState> commandStore = new(stateProvider, CommandsStreamName);
        List<CommandState> states = new();
        for (int i = 0; i < commands.Length; i++)
        {
            states.Add(new CommandState(_dateTimeService.UtcNow, commands[i], metadatas[i]));
        }

        long version = await commandStore.AddAsync(
            states,
            state.CommandStreamVersion,
            cancellationToken);
        await PersistStateAsync(
            stateProvider,
            new AggregateState(
                version,
                state.EventStreamVersion,
                state.LastCommandDone,
                state.LastEventPublished),
            cancellationToken);
    }

    /// <summary>
    /// Continue as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="aggregateInitializer">The aggregate initializer.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="removeReminder">The remove reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IAggregate&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Null.</exception>
    public async Task<IAggregate?> ContinueAsync(
        IStateStoreProvider stateProvider,
        ResiliencyPolicy resiliencyPolicy,
        IAggregate? aggregate,
        Func<BaseEvent, IAggregate> aggregateInitializer,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        Func<string, Task> removeReminder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        (TimeSpan? retry, aggregate) = await ExecuteCommandsAsync(stateProvider, aggregate, aggregateInitializer, resiliencyPolicy, cancellationToken);
        await PublishEventsAsync(stateProvider, cancellationToken);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        if (state.LastEventPublished < state.EventStreamVersion)
        {
            await SetReminderAsync(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), registerReminder);
            return aggregate;
        }

        if (state.LastCommandDone < state.CommandStreamVersion)
        {
            if (retry != null)
            {
                await SetReminderAsync(retry.Value.Add(TimeSpan.FromSeconds(1)), TimeSpan.FromMinutes(1), registerReminder);
            }
            else
            {
                await SetReminderAsync(resiliencyPolicy.MaximumExponentialPeriod, resiliencyPolicy.MaximumExponentialPeriod, registerReminder);
            }

            return aggregate;
        }

        await RemoveReminderAsync(removeReminder);
        return aggregate;
    }

    /// <inheritdoc/>
    public async Task<IAggregate?> GetAggregateAsync(IStateStoreProvider stateProvider, Func<BaseEvent, IAggregate> aggregateInitializer, CancellationToken cancellationToken)
    {
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        IAggregate? aggregate = default;
        for (long nextEvent = 1; nextEvent <= state.EventStreamVersion; nextEvent++)
        {
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken);
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
    public async Task<long> GetCommandCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        return state.CommandStreamVersion;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetCommandsAsync<T>(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        MessageStore<CommandState> commandStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        List<T> commands = new();
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            CommandState eventState = await commandStore.GetAsync(nextEvent, cancellationToken);
            if (eventState is T e)
            {
                commands.Add(e);
            }
        }

        return commands;
    }

    /// <inheritdoc/>
    public async Task<long> GetEventCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        return state.EventStreamVersion;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetEventsAsync<T>(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        List<T> events = new();
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken);
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
        IStateStoreProvider stateProvider,
        IAggregate? aggregate,
        Func<BaseEvent, IAggregate> aggregateInitializer,
        ResiliencyPolicy resiliencyPolicy,
        CancellationToken cancellationToken)
    {
        MessageStore<CommandState> commandStore = new(stateProvider, CommandsStreamName);
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        while (state.LastCommandDone < state.CommandStreamVersion)
        {
            long nextCommand = state.LastCommandDone + 1;
            CommandState command = await commandStore.GetAsync(nextCommand, cancellationToken);
            _ = command.Message ?? throw new InvalidOperationException($"Command {nextCommand} content is null.");
            _ = command.Metadata ?? throw new InvalidOperationException($"Command {nextCommand} metadata is null.");
            ResilientCommandProcessor processor = new(
                resiliencyPolicy,
                _dispatcher,
                stateProvider);

            (TimeSpan? retry, IEnumerable<BaseMessage> messages) = await processor.ProcessAsync(nextCommand.ToInvariantString(), command.Message, cancellationToken);
            BaseEvent[] events = messages.OfType<BaseEvent>().ToArray();
            long eventVersion = state.EventStreamVersion;
            if (events.Any())
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
                    cancellationToken);
            }

            state = new AggregateState(
                    state.CommandStreamVersion,
                    eventVersion,
                    retry == null ? nextCommand : state.LastCommandDone,
                    state.LastEventPublished);
            await PersistStateAsync(
                stateProvider,
                state,
                cancellationToken);
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
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken);

            await _eventBus.PublishAsync(eventState, cancellationToken);
            state = new AggregateState(
                    state.CommandStreamVersion,
                    state.EventStreamVersion,
                    state.LastCommandDone,
                    nextEvent);
            await PersistStateAsync(
                stateProvider,
                state,
                cancellationToken);
        }
    }

    private static async Task RemoveReminderAsync(Func<string, Task> removeReminder) => await removeReminder(ReminderName);

    /// <summary>
    /// Set reminder as an asynchronous operation.
    /// </summary>
    /// <param name="next">The next.</param>
    /// <param name="retry">The retry.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private static async Task SetReminderAsync(
        TimeSpan next,
        TimeSpan retry,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder) => await registerReminder(ReminderName, Array.Empty<byte>(), next, retry);

    /// <summary>
    /// Get state as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AggregateState&gt; representing the asynchronous operation.</returns>
    private async Task<AggregateState> GetStateAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ConditionalValue<AggregateState> result = await stateProvider.TryGetStateAsync<AggregateState>(StateName, cancellationToken);
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
        await stateProvider.SetStateAsync(StateName, state, CancellationToken.None);
        await stateProvider.SaveChangesAsync(cancellationToken);
    }
}