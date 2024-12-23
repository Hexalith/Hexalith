// <copyright file="ClientCommandService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ClientCommandService : ICommandService
{
    private readonly HttpClient _client;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCommandService"/> class.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="sessionService">The user session service.</param>
    /// <param name="timeProvider">The time provider.</param>
    public ClientCommandService(
        [NotNull] HttpClient client,
        [NotNull] ISessionService sessionService,
        [NotNull] TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
        _sessionService = sessionService;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ClaimsPrincipal user, object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(user);

        string? userName = user.Identity?.Name;
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new InvalidOperationException("User is authenticated but user ID is not found.");
        }

        string messageId = UniqueIdHelper.GenerateUniqueStringId();
        SessionInformation session = await _sessionService.GetAsync(userName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Session not found or expired.");
        Metadata metadata = new(
            new MessageMetadata(
            messageId,
            string.Empty,
            1,
            AggregateMetadata.Create(command),
            _timeProvider.GetLocalNow()),
            new ContextMetadata(
                messageId,
                userName,
                session.PartitionId,
                _timeProvider.GetLocalNow(),
                null,
                session.SessionId,
                []));

        await SubmitCommandAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Submits a command asynchronously with the provided metadata.
    /// </summary>
    /// <param name="command">The command to submit.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task SubmitCommandAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        if (command is not PolymorphicRecordBase recordBase)
        {
            throw new ArgumentException($"The command should be of type {nameof(PolymorphicRecordBase)}");
        }

        _ = await _client.PostAsJsonAsync("api/command/submit", new MessageState(recordBase, metadata), cancellationToken);
    }
}