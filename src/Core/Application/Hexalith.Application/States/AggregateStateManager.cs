// <copyright file="AggregateStateManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System;
using System.Linq;
using System.Threading;

using Hexalith.Application.Abstractions.Aggregates;
using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Application.Abstractions.Tasks;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

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
    /// Initializes a new instance of the <see cref="AggregateStateManager"/> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="eventBus">The event bus.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <exception cref="System.ArgumentNullException">Null argument.</exception>
    public AggregateStateManager(
        ICommandDispatcher dispatcher,
        IEventBus eventBus,
        IDateTimeService dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _dateTimeService = dateTimeService;
        _dispatcher = dispatcher;
        _eventBus = eventBus;
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
    /// <exception cref="System.ArgumentNullException">Argument null.</exception>
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
        await SetReminderAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), registerReminder);
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
    /// Initialize as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Null argument.</exception>
    public async Task ContinueAsync(
        IStateStoreProvider stateProvider,
        ResiliencyPolicy resiliencyPolicy,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        TimeSpan? retry = await ExecuteCommandsAsync(stateProvider, resiliencyPolicy, cancellationToken);
        await PublishEventsAsync(stateProvider, cancellationToken);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        if (state.LastEventPublished < state.EventStreamVersion)
        {
            await SetReminderAsync(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), registerReminder);
            return;
        }

        if (retry != null)
        {
            await SetReminderAsync(retry.Value.Add(TimeSpan.FromMilliseconds(100)), TimeSpan.FromMinutes(1), registerReminder);
        }
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

    /// <summary>Do next command as an asynchronous operation.</summary>
    /// <param name="stateProvider">The state store provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Null argument.</exception>
    protected async Task<TimeSpan?> ExecuteCommandsAsync(
        IStateStoreProvider stateProvider,
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

            (TimeSpan? retry, IEnumerable<BaseEvent> events) = await processor.ProcessAsync(nextCommand.ToInvariantString(), command.Message, cancellationToken);
            long eventVersion = state.EventStreamVersion;
            if (events.Any())
            {
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
                return retry;
            }
        }

        return null;
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

    private static async Task SetReminderAsync(
        TimeSpan next,
        TimeSpan retry,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder)
    {
        await registerReminder("Continue", Array.Empty<byte>(), next, retry);
    }

    private async Task<AggregateState> GetStateAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ConditionalValue<AggregateState> result = await stateProvider.TryGetStateAsync<AggregateState>(StateName, cancellationToken);
        return result.HasValue ? result.Value : new AggregateState(0, 0, 0, 0);
    }

    private string GetTaskProcessorStateName(long version)
    {
        return nameof(TaskProcessor) + version.ToInvariantString();
    }

    /// <summary>
    /// Set state as an asynchronous operation.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task PersistStateAsync(IStateStoreProvider stateProvider, AggregateState state, CancellationToken cancellationToken)
    {
        await stateProvider.SetStateAsync(StateName, state, CancellationToken.None);
        await stateProvider.SaveChangesAsync(cancellationToken);
    }
}