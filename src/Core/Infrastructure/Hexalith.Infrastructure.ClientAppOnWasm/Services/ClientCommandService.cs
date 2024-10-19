// <copyright file="ClientCommandService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ClientCommandService : IClientCommandService
{
    private readonly HttpClient _client;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCommandService"/> class.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="sessionService">The session service.</param>
    public ClientCommandService([NotNull] HttpClient client, [NotNull] TimeProvider timeProvider, [NotNull] IUserService userService, [NotNull] ISessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
        _timeProvider = timeProvider;
        _userService = userService;
        _sessionService = sessionService;
    }

    /// <inheritdoc/>
    public Task SendCommandAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);

        return _client.PostAsJsonAsync("api/commands", new MessageState((PolymorphicRecordBase)command, metadata), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SendCommandAsync(object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string messageId = UniqueIdHelper.GenerateUniqueStringId();
        string userId = await _userService.GetUserIdAsync(cancellationToken).ConfigureAwait(false);
        string sessionId = await _sessionService.GetSessionIdAsync(cancellationToken).ConfigureAwait(false);
        string partitionId = await _sessionService.GetPartitionIdAsync(cancellationToken).ConfigureAwait(false);
        Metadata metadata = new(
            new MessageMetadata(
            messageId,
            string.Empty,
            1,
            new AggregateMetadata(command),
            _timeProvider.GetLocalNow()),
            new ContextMetadata(messageId, userId, partitionId, null, null, sessionId, []));

        await SendCommandAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }
}