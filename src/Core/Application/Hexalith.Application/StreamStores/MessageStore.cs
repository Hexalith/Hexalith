// <copyright file="MessageStore.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Hexalith.Application.States;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// This class is used to store messages state.
/// </summary>
/// <typeparam name="TMessage">The storage message type.
/// Must have the JSON polymorphic base class attribute <see cref="JsonPolymorphicBaseClassAttribute" /> or one of it's base classes must have it.</typeparam>
public class MessageStore<TMessage>
    where TMessage : IIdempotent
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
    /// <exception cref="System.ArgumentNullException">Argument is null.</exception>
    public MessageStore([NotNull] IStateStoreProvider stateManager, [NotNull] string streamName)
    {
        ArgumentNullException.ThrowIfNull(stateManager);
        ArgumentException.ThrowIfNullOrWhiteSpace(streamName);
        _stateManager = stateManager;
        StreamName = streamName;
    }

    /// <summary>
    /// Gets the streamName.
    /// </summary>
    /// <value>The name of the stream.</value>
    public string StreamName { get; }

    /// <summary>
    /// Add as an asynchronous operation.
    /// </summary>
    /// <param name="messages">The messages.</param>
    /// <param name="expectedVersion">The expected version.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int64&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Argument is null.</exception>
    /// <exception cref="System.Data.DBConcurrencyException">Stream version mismatch: Expected version={expectedVersion.ToString(CultureInfo.InvariantCulture)}; Actual version={version.ToString(CultureInfo.InvariantCulture)}.</exception>
    /// <exception cref="StreamStores.DuplicateIdempotencyIdException">The idempotent Id already exists in the stream.</exception>
    public async Task<long> AddAsync(
        [NotNull] IEnumerable<TMessage> messages,
        long expectedVersion,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(messages);
        if (expectedVersion < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedVersion), "Version cannot be negative.");
        }

        long version = await GetVersionAsync(cancellationToken)
            .ConfigureAwait(false);

        if (expectedVersion != version)
        {
            throw new DBConcurrencyException(
                $"Stream version mismatch: Expected version={expectedVersion.ToString(CultureInfo.InvariantCulture)}; Actual version={version.ToString(CultureInfo.InvariantCulture)}.");
        }

        foreach (TMessage m in messages)
        {
            ConditionalValue<long> result = await _stateManager.TryGetStateAsync<long>(
                  GetMessageStateName(m.IdempotencyId),
                  cancellationToken)
                  .ConfigureAwait(false);
            if (result.HasValue)
            {
                throw new DuplicateIdempotencyIdException(m.IdempotencyId, m);
            }

            await _stateManager.AddStateAsync(
                  GetMessageStateName(m.IdempotencyId),
                  ++version,
                  cancellationToken)
                  .ConfigureAwait(false);
            await _stateManager.AddStateAsync(
                GetStreamItemStateName(version),
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
        if (fromVersion < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fromVersion), "Version cannot be negative or zero.");
        }

        if (toVersion < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(toVersion), "Version cannot be negative or zero.");
        }

        if (toVersion < fromVersion)
        {
            throw new ArgumentException("To version should be greater or equal than from version.", nameof(toVersion));
        }

        List<TMessage> messages = [];

        while (fromVersion <= toVersion)
        {
            ConditionalValue<TMessage> result = await _stateManager
                .TryGetStateAsync<TMessage>(GetStreamItemStateName(fromVersion++), cancellationToken)
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
        if (version < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(version), "Version cannot be negative or zero.");
        }

        ConditionalValue<TMessage> result = await _stateManager
                .TryGetStateAsync<TMessage>(GetStreamItemStateName(version), cancellationToken)
                .ConfigureAwait(false);
        return result.HasValue
            ? result.Value
            : throw new MessageStoreItemNotFoundException(version: version, StreamName, message: null, innerException: null);
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
        if (version < 1)
        {
            return (Enumerable.Empty<TMessage>(), 0);
        }

        IEnumerable<TMessage> messages = await GetAsync(1, version, cancellationToken)
            .ConfigureAwait(false);
        return (messages, version);
    }

    /// <summary>
    /// The message state name. Used to store the message data and id.
    /// </summary>
    /// <param name="id">The stream item version number.</param>
    /// <returns>The message state name. Default is the version number.</returns>
    public virtual string GetMessageStateName(string id) => GetStreamStateName() + "Id-" + id;

    /// <summary>
    /// The stream item version and identifier.
    /// </summary>
    /// <param name="version">The stream item version number.</param>
    /// <returns>The message state name. Default is the version number.</returns>
    public virtual string GetStreamItemStateName(long version) => GetStreamStateName() + version.ToInvariantString();

    /// <summary>
    /// The stream state name. Used to store version.
    /// </summary>
    /// <returns>The stream state name. Default is 'Stream'.</returns>
    public virtual string GetStreamStateName() => StreamName + "Stream";

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