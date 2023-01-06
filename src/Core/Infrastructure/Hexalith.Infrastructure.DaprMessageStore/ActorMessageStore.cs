// <copyright file="ActorMessageStore.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprMessageStore;

using Ardalis.GuardClauses;

using Dapr.Actors.Runtime;

using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Serialization;

using System.Data;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// This class is used to store messages in a Dapr actor state.
/// </summary>
/// <typeparam name="TMessage">The storage message type.
/// Must have the JSON polymorphic base class attribute <see cref="JsonPolymorphicBaseClassAttribute"/> or one of it's base classes must have it.
/// </typeparam>
public class ActorMessageStore<TMessage>
    where TMessage : class
{
    private static readonly JsonPolymorphicBaseClassAttribute? _jsonPolymorphic = Attribute
        .GetCustomAttribute(typeof(TMessage), typeof(JsonPolymorphicBaseClassAttribute))
            as JsonPolymorphicBaseClassAttribute;

    private readonly IActorStateManager _stateManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorMessageStore{TMessage}"/> class.
    /// </summary>
    /// <param name="stateManager">The actor state manager.</param>
    /// <param name="streamName">The stream name.</param>
    public ActorMessageStore(IActorStateManager stateManager, string streamName)
    {
        _stateManager = Guard.Against.Null(stateManager);
        _ = Guard.Against.Null(
            _jsonPolymorphic,
            message: $"The {nameof(JsonPolymorphicBaseClassAttribute)} must be set on {typeof(TMessage).FullName} class or one of it's base classes.");
        StreamName = streamName;
    }

    /// <summary>
    /// Gets the streamName.
    /// </summary>
    public string StreamName { get; }

    /// <summary>
    /// Add new messages to the stream.
    /// </summary>
    /// <param name="messages">List of messages.</param>
    /// <param name="expectedVersion">Expected version for concurrency check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The new version of the stream.</returns>
    /// <exception cref="DBConcurrencyException">Thrown if expected version does not match stream version.</exception>
    public async Task<long> AddAsync(
        IEnumerable<TMessage> messages,
        long expectedVersion,
        CancellationToken cancellationToken)
    {
        _ = Guard.Against.Negative(expectedVersion);
        _ = Guard.Against.Null(messages);

        long version = await GetVersionAsync(cancellationToken)
            .ConfigureAwait(false);

        if (expectedVersion != version)
        {
            throw new DBConcurrencyException(
                $"Stream version mismatch: Expected version={expectedVersion.ToString(CultureInfo.InvariantCulture)}; Actual version={version.ToString(CultureInfo.InvariantCulture)}.");
        }

        foreach (TMessage e in messages)
        {
            await _stateManager.AddStateAsync(
                GetMessageStateName(++version),
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
    /// Polymorphic deserialization of the message.
    /// </summary>
    /// <param name="json">The JSON serialized message.</param>
    /// <returns>The message instance.</returns>
    public virtual TMessage Deserialize(string json)
    {
        _ = Guard.Against.NullOrWhiteSpace(json);
        TMessage? @message = JsonSerializer.Deserialize<TMessage>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
        return @message ?? throw new SerializationException("Could not deserialize object from json : " + json);
    }

    /// <summary>
    /// Get messages from stream.
    /// </summary>
    /// <param name="fromVersion">First message to retreive.</param>
    /// <param name="toVersion">Last message to retreive.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<IEnumerable<TMessage>> GetAsync(long fromVersion, long toVersion, CancellationToken cancellationToken)
    {
        _ = Guard.Against.AgainstExpression(p => p > 0, fromVersion, "From version should be greater than 0.");
        _ = Guard.Against.AgainstExpression(p => p >= fromVersion, toVersion, "To version should be greater or equal than from version.");

        List<TMessage> messages = new();

        while (fromVersion <= toVersion)
        {
            ConditionalValue<string> result = await _stateManager.TryGetStateAsync<string>(GetMessageStateName(fromVersion++), cancellationToken)
                            .ConfigureAwait(false);
            if (!result.HasValue)
            {
                throw new MessageStoreItemNotFoundException(version: fromVersion, StreamName, message: null, innerException: null);
            }

            messages.Add(Deserialize(result.Value));
        }

        return messages;
    }

    /// <summary>
    /// Get all messages.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All messages in the stream.</returns>
    public async Task<(IEnumerable<TMessage> Messages, long Version)> GetAsync(CancellationToken cancellationToken)
    {
        long version = await GetVersionAsync(cancellationToken)
            .ConfigureAwait(false);
        IEnumerable<TMessage> messages = await GetAsync(1, version, cancellationToken)
            .ConfigureAwait(false);
        return (messages, version);
    }

    /// <summary>
    /// The message state name. Used to store the message data.
    /// </summary>
    /// <param name="version">The stream item version number.</param>
    /// <returns>The message state name. Default is the version number.</returns>
    public virtual string GetMessageStateName(long version)
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
    /// Polymorphic serialization of the message to JSON.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <returns>The JSON string.</returns>
    public virtual string Serialize(TMessage @message)
    {
        return JsonSerializer.Serialize(@message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
    }
}
