// <copyright file="MessageStore.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System.Data;
using System.Globalization;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.States;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Serialization;

/// <summary>
/// This class is used to store messages state.
/// </summary>
/// <typeparam name="TMessage">The storage message type.
/// Must have the JSON polymorphic base class attribute <see cref="JsonPolymorphicBaseClassAttribute" /> or one of it's base classes must have it.</typeparam>
public class MessageStore<TMessage>
    where TMessage : class
{
    /// <summary>
    /// The state manager.
    /// </summary>
    private readonly IStateStoreProvider _stateManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageStore{TMessage}" /> class.
    /// </summary>
    /// <param name="stateManager">The actor state manager.</param>
    /// <param name="streamName">The stream name.</param>
    public MessageStore(IStateStoreProvider stateManager, string streamName)
    {
        _stateManager = Guard.Against.Null(stateManager);
        StreamName = streamName;
    }

    /// <summary>
    /// Gets the streamName.
    /// </summary>
    /// <value>The name of the stream.</value>
    public string StreamName { get; }

    /// <summary>
    /// Add new messages to the stream.
    /// </summary>
    /// <param name="messages">List of messages.</param>
    /// <param name="expectedVersion">Expected version for concurrency check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The new version of the stream.</returns>
    /// <exception cref="System.Data.DBConcurrencyException">Stream version mismatch: Expected version={expectedVersion.ToString(CultureInfo.InvariantCulture)}; Actual version={version.ToString(CultureInfo.InvariantCulture)}.</exception>
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

        foreach (TMessage m in messages)
        {
            await _stateManager.AddStateAsync(
                GetMessageStateName(++version),
                m,
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
    /// Get messages from stream.
    /// </summary>
    /// <param name="fromVersion">First message to retrieve.</param>
    /// <param name="toVersion">Last message to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    /// <exception cref="Hexalith.Application.StreamStores.MessageStoreItemNotFoundException">Item not found.</exception>
    public async Task<IEnumerable<TMessage>> GetAsync(long fromVersion, long toVersion, CancellationToken cancellationToken)
    {
        _ = Guard.Against.AgainstExpression(p => p > 0, fromVersion, "From version should be greater than 0.");
        _ = Guard.Against.AgainstExpression(p => p >= fromVersion, toVersion, "To version should be greater or equal than from version.");

        List<TMessage> messages = new();

        while (fromVersion <= toVersion)
        {
            ConditionalValue<TMessage> result = await _stateManager
                .TryGetStateAsync<TMessage>(GetMessageStateName(fromVersion++), cancellationToken)
                .ConfigureAwait(false);
            if (!result.HasValue)
            {
                throw new MessageStoreItemNotFoundException(version: fromVersion, StreamName, message: null, innerException: null);
            }

            messages.Add(result.Value);
        }

        return messages;
    }

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TMessage&gt; representing the asynchronous operation.</returns>
    /// <exception cref="Hexalith.Application.StreamStores.MessageStoreItemNotFoundException">Item not found.</exception>
    public async Task<TMessage> GetAsync(long version, CancellationToken cancellationToken)
    {
        _ = Guard.Against.NegativeOrZero(version);

        ConditionalValue<TMessage> result = await _stateManager
                .TryGetStateAsync<TMessage>(GetMessageStateName(version), cancellationToken)
                .ConfigureAwait(false);
        return !result.HasValue
            ? throw new MessageStoreItemNotFoundException(version: version, StreamName, message: null, innerException: null)
            : result.Value;
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
}