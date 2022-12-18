// <copyright file="ActorEventStore.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprEventStore;

using Ardalis.GuardClauses;

using Dapr.Actors.Runtime;

using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Serialization;

using System.Data;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// This class is used to store events in a Dapr actor state.
/// </summary>
/// <typeparam name="TEvent">The storage event type.
/// Must have the JSON polymorphic base class attribute <see cref="JsonPolymorphicBaseClassAttribute"/> or one of it's base classes must have it.
/// </typeparam>
public class ActorEventStore<TEvent>
    where TEvent : class
{
    private static readonly JsonPolymorphicBaseClassAttribute? _jsonPolymorphic = Attribute
        .GetCustomAttribute(typeof(TEvent), typeof(JsonPolymorphicBaseClassAttribute))
            as JsonPolymorphicBaseClassAttribute;

    private readonly IActorStateManager _stateManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorEventStore{TEvent}"/> class.
    /// </summary>
    /// <param name="stateManager">The actor state manager.</param>
    /// <param name="streamName">The stream name.</param>
    public ActorEventStore(IActorStateManager stateManager, string streamName)
    {
        _stateManager = Guard.Against.Null(stateManager);
        _ = Guard.Against.Null(
            _jsonPolymorphic,
            message: $"The {nameof(JsonPolymorphicBaseClassAttribute)} must be set on {typeof(TEvent).FullName} class or one of it's base classes.");
        StreamName = streamName;
    }

    /// <summary>
    /// Gets the streamName.
    /// </summary>
    public string StreamName { get; }

    /// <summary>
    /// Add new events to the stream.
    /// </summary>
    /// <param name="events">List of events.</param>
    /// <param name="expectedVersion">Expected version for concurrency check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The new version of the stream.</returns>
    /// <exception cref="DBConcurrencyException">Thrown if expected version does not match stream version.</exception>
    public async Task<long> AddAsync(
        IEnumerable<TEvent> events,
        long expectedVersion,
        CancellationToken cancellationToken)
    {
        _ = Guard.Against.Negative(expectedVersion);
        _ = Guard.Against.Null(events);

        long version = await GetVersionAsync(cancellationToken)
            .ConfigureAwait(false);

        if (expectedVersion != version)
        {
            throw new DBConcurrencyException(
                $"Stream version mismatch: Expected version={expectedVersion.ToString(CultureInfo.InvariantCulture)}; Actual version={version.ToString(CultureInfo.InvariantCulture)}.");
        }

        foreach (TEvent e in events)
        {
            await _stateManager.AddStateAsync(
                GetEventStateName(++version),
                Serialize(e),
                cancellationToken)
                .ConfigureAwait(false);
        }

        if (expectedVersion == 0)
        {
            // If version is 0, the stream has not been initialized, we need to add new version header.
            await _stateManager.AddStateAsync(
                GetStreamStateName(),
                version,
                cancellationToken)
                .ConfigureAwait(false);
            return version;
        }

        // Update the stream header with the new version.
        await _stateManager
            .SetStateAsync(GetStreamStateName(), version, cancellationToken)
            .ConfigureAwait(false);
        return version;
    }

    /// <summary>
    /// Polymorphic deserialization of the event.
    /// </summary>
    /// <param name="json">The JSON serialized event.</param>
    /// <returns>The event instance.</returns>
    public virtual TEvent Deserialize(string json)
    {
        _ = Guard.Against.NullOrWhiteSpace(json);
        TEvent? @event = JsonSerializer.Deserialize<TEvent>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
        return @event ?? throw new SerializationException("Could not deserialize object from json : " + json);
    }

    /// <summary>
    /// Get events from stream.
    /// </summary>
    /// <param name="fromVersion">First event to retreive.</param>
    /// <param name="toVersion">Last event to retreive.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<IEnumerable<TEvent>> GetAsync(long fromVersion, long toVersion, CancellationToken cancellationToken)
    {
        _ = Guard.Against.AgainstExpression(p => p > 0, fromVersion, "From version should be greater than 0.");
        _ = Guard.Against.AgainstExpression(p => p >= fromVersion, toVersion, "To version should be greater or equal than from version.");

        List<TEvent> events = new();

        while (fromVersion <= toVersion)
        {
            ConditionalValue<string> result = await _stateManager.TryGetStateAsync<string>(GetEventStateName(fromVersion++), cancellationToken)
                            .ConfigureAwait(false);
            if (!result.HasValue)
            {
                throw new EventStoreItemNotFoundException(version: fromVersion, StreamName, message: null, innerException: null);
            }

            events.Add(Deserialize(result.Value));
        }

        return events;
    }

    /// <summary>
    /// Get all events.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All events in the stream.</returns>
    public async Task<(IEnumerable<TEvent> Events, long Version)> GetAsync(CancellationToken cancellationToken)
    {
        long version = await GetVersionAsync(cancellationToken)
            .ConfigureAwait(false);
        IEnumerable<TEvent> events = await GetAsync(1, version, cancellationToken)
            .ConfigureAwait(false);
        return (events, version);
    }

    /// <summary>
    /// The event state name. Used to store the event data.
    /// </summary>
    /// <param name="version">The stream item version number.</param>
    /// <returns>The event state name. Default is the version number.</returns>
    public virtual string GetEventStateName(long version)
    {
        return GetStreamStateName() + version.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// The stream state name. Used to store version.
    /// </summary>
    /// <returns>The stream state name. Default is 'Stream'.</returns>
    public virtual string GetStreamStateName()
    {
        return StreamName + "Stream";
    }

    /// <summary>
    /// Get version.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The actual stream version..</returns>
    public async Task<long> GetVersionAsync(CancellationToken cancellationToken)
    {
        ConditionalValue<long> result = await _stateManager
            .TryGetStateAsync<long>(GetStreamStateName(), cancellationToken)
            .ConfigureAwait(false);
        return result.HasValue ? result.Value : 0L;
    }

    /// <summary>
    /// Polymorphic serialization of the event to JSON.
    /// </summary>
    /// <param name="event">The event to serialize.</param>
    /// <returns>The JSON string.</returns>
    public virtual string Serialize(TEvent @event)
    {
        return JsonSerializer.Serialize<TEvent>(@event, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
    }
}
