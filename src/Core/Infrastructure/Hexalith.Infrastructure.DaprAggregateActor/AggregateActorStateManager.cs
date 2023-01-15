// <copyright file="AggregateActorStateManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprAggregateActor;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Aggregates;
using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.Serialization;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading;

/// <summary>
/// Class AggregateActorState.
/// Implements the <see cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />.
/// </summary>
/// <seealso cref="Christofle.Infrastructure.Dynamics365Finance.DaprCloud.AggregateActors.IAggregateActorState" />
public class AggregateActorStateManager : IAggregateStateManager
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ICommandDispatcher _dispatcher;
    private Stack<CommandState>? _toDo;
    private Stack<EventState>? _toPublish;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActorStateManager"/> class.
    /// </summary>
    /// <param name="stateProvider">The state manager.</param>
    /// <param name="dispatcher">The handler.</param>
    /// <param name="dateTimeService">The date time service.</param>
    public AggregateActorStateManager(
        ICommandDispatcher dispatcher,
        IDateTimeService dateTimeService)
    {
        _dateTimeService = Guard.Against.Null(dateTimeService);
        _dispatcher = Guard.Against.Null(dispatcher);
    }

    /// <summary>
    /// Gets the name of the command state.
    /// </summary>
    /// <value>The name of the command state.</value>
    public virtual string CommandStateName => "Command";

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
    /// Gets converts to do.
    /// </summary>
    /// <value>To do.</value>
    /// <exception cref="System.InvalidOperationException">Commands to do state is not initialized.</exception>
    public Stack<CommandState> ToDo => _toDo ?? throw new InvalidOperationException("Commands to do state is not initialized.");

    /// <summary>
    /// Gets converts to dostatename.
    /// </summary>
    /// <value>The name of to do state.</value>
    public virtual string ToDoStateName => nameof(ToDo);

    /// <summary>
    /// Gets converts to publish.
    /// </summary>
    /// <value>To publish.</value>
    /// <exception cref="System.InvalidOperationException">Events to publish state is not initialized.</exception>
    public Stack<EventState> ToPublish => _toPublish ?? throw new InvalidOperationException("Events to publish state is not initialized.");

    /// <summary>
    /// Gets converts to publishstatename.
    /// </summary>
    /// <value>The name of to publish state.</value>
    public virtual string ToPublishStateName => nameof(ToPublish);

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
        _ = Guard.Against.Null(stateProvider);
        _ = Guard.Against.Null(metadata);
        _ = Guard.Against.Null(metadata.Message);
        _ = Guard.Against.NullOrWhiteSpace(metadata.Message.Id);
        _ = Guard.Against.Null(command);
        ToDo.Push(
            new CommandState(
            _dateTimeService.UtcNow,
            metadata.Message.Id,
            Serialize<BaseCommand>(command),
            Serialize<Metadata>(metadata),
            null));
        await stateProvider.SetStateAsync<CommandState[]>(ToDoStateName, ToDo.ToArray(), cancellationToken);
    }

    /// <summary>
    /// Polymorphic deserialization of the message.
    /// </summary>
    /// <typeparam name="T">Type to deserialize.</typeparam>
    /// <param name="json">The JSON serialized message.</param>
    /// <returns>The message instance.</returns>
    /// <exception cref="System.Runtime.Serialization.SerializationException">Could not deserialize object from json : " + json.</exception>
    public virtual T Deserialize<T>(string json)
    {
        _ = Guard.Against.NullOrWhiteSpace(json);
        T? message = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
        return message ?? throw new SerializationException("Could not deserialize object from json : " + json);
    }

    /// <summary>
    /// Do next command as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public async Task ExecuteCommandsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(stateProvider);
        while (ToDo.Count > 0)
        {
            CommandState command = ToDo.Pop();
            BaseCommand cmd = Deserialize<BaseCommand>(command.Message);
            Metadata meta = Deserialize<Metadata>(command.Metadata);
            await ExecuteCommandAsync(cmd, meta, cancellationToken);
            _ = stateProvider.SetStateAsync<EventState[]>(ToPublishStateName, ToPublish.ToArray(), cancellationToken);
            _ = stateProvider.SetStateAsync<CommandState[]>(ToDoStateName, ToDo.ToArray(), cancellationToken);
            _ = stateProvider.SetStateAsync<CommandState>(GetCommandStateName(meta.Message.Id), new CommandState(command, _dateTimeService.UtcNow), cancellationToken);
            await stateProvider.SaveChangesAsync(cancellationToken);
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
        _ = Guard.Against.Null(stateProvider);
        ConditionalValue<CommandState[]> commands = await stateProvider.TryGetStateAsync<CommandState[]>(ToDoStateName, cancellationToken);
        _toDo = commands.HasValue ? new Stack<CommandState>(commands.Value) : new Stack<CommandState>();
        ConditionalValue<EventState[]> events = await stateProvider.TryGetStateAsync<EventState[]>(ToPublishStateName, cancellationToken);
        _toPublish = events.HasValue ? new Stack<EventState>(events.Value) : new Stack<EventState>();

        // If the command state does not exist, the actor has never been activated. We need to add the actor reminder.
        if (!commands.HasValue)
        {
            await registerReminder("Continue", Array.Empty<byte>(), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30));
        }
    }

    /// <inheritdoc/>
    public Task PublishEventsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Polymorphic serialization of the message to JSON.
    /// </summary>
    /// <typeparam name="T">Type to deserialize.</typeparam>
    /// <param name="message">The message to serialize.</param>
    /// <returns>The JSON string.</returns>
    public virtual string Serialize<T>(T message)
    {
        return JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
    }

    private async Task ExecuteCommandAsync(ICommand command, IMetadata metadata, CancellationToken cancellationToken)
    {
        IEnumerable<BaseEvent> events = await _dispatcher.DoAsync(command, cancellationToken);
        foreach (BaseEvent e in events)
        {
            Metadata em = Metadata.CreateNew(e, metadata, _dateTimeService.UtcNow);
            ToPublish.Push(new EventState(
                _dateTimeService.UtcNow,
                em.Message.Id,
                Serialize<BaseEvent>(e),
                Serialize<Metadata>(em),
                null));
        }
    }

    /// <summary>
    /// Gets the name of the command state.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    private string GetCommandStateName(string id)
    {
        return CommandStateName + Separator + id;
    }
}