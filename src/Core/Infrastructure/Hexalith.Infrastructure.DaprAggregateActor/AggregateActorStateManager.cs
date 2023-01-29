// <copyright file="AggregateActorStateManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprAggregateActor;

using System.Linq;
using System.Threading;

using Hexalith.Application.Abstractions.Aggregates;
using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Application.StreamStores;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class AggregateActorState.
/// Implements the <see cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />.
/// </summary>
/// <seealso cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />
public class AggregateActorStateManager : IAggregateStateManager
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ICommandDispatcher _dispatcher;
    private MessageStore<CommandState>? _commands;
    private MessageStore<EventState>? _events;
    private AggregateActorState? _state;
    private IStateStoreProvider? _stateProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActorStateManager"/> class.
    /// </summary>
    /// <param name="dispatcher">The handler.</param>
    /// <param name="dateTimeService">The date time service.</param>
    public AggregateActorStateManager(
        ICommandDispatcher dispatcher,
        IDateTimeService dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _dateTimeService = dateTimeService;
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Gets to do state name.
    /// </summary>
    /// <value>The name of to do state.</value>
    public virtual string CommandsStreamName => nameof(Commands);

    /// <summary>
    /// Gets to do state name.
    /// </summary>
    /// <value>The name of to do state.</value>
    public virtual string EventsStreamName => nameof(Events);

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

    private MessageStore<CommandState> Commands => _commands ?? throw new InvalidOperationException("The command state store is not initialized.");

    private MessageStore<EventState> Events => _events ?? throw new InvalidOperationException("The event state store is not initialized.");

    private AggregateActorState State => _state ??= new AggregateActorState(0, 0, 0, 0);

    private IStateStoreProvider StateProvider => _stateProvider ?? throw new InvalidOperationException("The actor state provider is not initialized.");

    /// <summary>
    /// Add command as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="command">The command.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task AddCommandAsync(IStateStoreProvider stateProvider, BaseCommand command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(metadata.Message);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(metadata.Message.Id);
        _ = await Commands.AddAsync(
            new CommandState(
            _dateTimeService.UtcNow,
            metadata.Message.Id,
            command,
            metadata,
            null).IntoArray(),
            State.CommandStreamVersion,
            cancellationToken);
        await SetStateAsync(State.IncrementCommandVersion(), cancellationToken);
    }

    /// <summary>
    /// Do next command as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public async Task ExecuteCommandsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        while (State.NextEventToPublish != 0 && State.NextCommandToDo <= State.CommandStreamVersion)
        {
            CommandState command = await Commands.GetAsync(State.NextCommandToDo, cancellationToken);
            await ExecuteCommandAsync(command.Command, command.Metadata, cancellationToken);
            await SetStateAsync(State.IncrementNextCommandToDo(), cancellationToken);
        }
    }

    /// <summary>
    /// Initialize as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="registerReminder">The add reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(IStateStoreProvider stateProvider, Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        _stateProvider = stateProvider;
        _commands = new MessageStore<CommandState>(StateProvider, CommandsStreamName);
        _events = new MessageStore<EventState>(StateProvider, EventsStreamName);

        ConditionalValue<AggregateActorState> state = await StateProvider.TryGetStateAsync<AggregateActorState>(StateName, cancellationToken);

        // If the state does not exist, the actor has never been activated. We need to add the actor reminder.
        if (state.HasValue)
        {
            await registerReminder("Continue", Array.Empty<byte>(), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));
            _state = state.Value;
        }
    }

    /// <inheritdoc/>
    public Task PublishEventsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteCommandAsync(ICommand command, IMetadata metadata, CancellationToken cancellationToken)
    {
        IEnumerable<BaseEvent> events = await _dispatcher.DoAsync(command, cancellationToken);
        IEnumerable<EventState> eventStates = events.Select(
            p => new EventState(
                _dateTimeService.UtcNow,
                metadata.Message.Id,
                p,
                Metadata.CreateNew(p, metadata, _dateTimeService.UtcNow)));

        long version = await Events.AddAsync(
            eventStates,
            State.EventStreamVersion,
            cancellationToken);
        await SetStateAsync(State.WithEventVersion(version), cancellationToken);
    }

    private async Task SetStateAsync(AggregateActorState state, CancellationToken cancellationToken)
    {
        await StateProvider.SetStateAsync(StateName, state, CancellationToken.None);
        await StateProvider.SaveChangesAsync(cancellationToken);
        _state = state;
    }
}