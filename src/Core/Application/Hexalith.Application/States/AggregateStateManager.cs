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
    /// <param name="command">The command.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Null argument.</exception>
    public async Task AddCommandAsync(IStateStoreProvider stateProvider, BaseCommand command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(metadata.Message);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(metadata.Message.Id);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        MessageStore<CommandState> commands = new(stateProvider, CommandsStreamName);
        long version = await commands.AddAsync(
            new CommandState(
            _dateTimeService.UtcNow,
            command,
            metadata,
            null).IntoArray(),
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

    /// <summary>Do next command as an asynchronous operation.</summary>
    /// <param name="stateProvider">The state store provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Null argument.</exception>
    public async Task ExecuteCommandsAsync(IStateStoreProvider stateProvider, ResiliencyPolicy resiliencyPolicy, CancellationToken cancellationToken)
    {
        MessageStore<CommandState> commandStore = new(stateProvider, CommandsStreamName);
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        while (state.LastCommandDone < state.CommandStreamVersion)
        {
            long nextCommand = state.LastCommandDone + 1;
            CommandState command = await commandStore.GetAsync(nextCommand, cancellationToken);
            ResilientCommandProcessor processor = new(
                resiliencyPolicy,
                _dispatcher,
                stateProvider);

            (bool terminated, IEnumerable<BaseEvent> events) = await processor.ProcessAsync(nextCommand.ToInvariantString(), command.Command, cancellationToken);
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
                    terminated ? nextCommand : state.LastCommandDone,
                    state.LastEventPublished);
            await PersistStateAsync(
                stateProvider,
                state,
                cancellationToken);
            if (!terminated)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Initialize as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Null argument.</exception>
    public async Task InitializeAsync(
        IStateStoreProvider stateProvider,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ConditionalValue<AggregateState> state = await stateProvider.TryGetStateAsync<AggregateState>(StateName, cancellationToken);

        if (!state.HasValue)
        {
            // If the state does not exist, the actor has never been activated. We need to add the actor reminder.
            await registerReminder("Continue", Array.Empty<byte>(), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));
            await PersistStateAsync(stateProvider, new AggregateState(0, 0, 0, 0), cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task PublishEventsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        MessageStore<EventState> eventStore = new(stateProvider, EventsStreamName);
        AggregateState state = await GetStateAsync(stateProvider, cancellationToken);
        while (state.LastEventPublished < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventPublished + 1;
            EventState eventState = await eventStore.GetAsync(nextEvent, cancellationToken);

            await _eventBus.PublishAsync(eventState.Event, eventState.Metadata, cancellationToken);
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

    private async Task<AggregateState> GetStateAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        return await stateProvider.GetStateAsync<AggregateState>(StateName, cancellationToken);
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