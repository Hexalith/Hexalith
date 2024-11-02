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

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;
using Hexalith.PolymorphicSerialization;

using Microsoft.AspNetCore.Components.Authorization;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ClientCommandService : ICommandService
{
    private readonly AuthenticationStateProvider _authenticationProvider;
    private readonly HttpClient _client;
    private readonly IUserSessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCommandService"/> class.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="sessionService">The user session service.</param>
    /// <param name="authenticationProvider"></param>
    /// <param name="timeProvider">The time provider.</param>
    public ClientCommandService(
        [NotNull] HttpClient client,
        [NotNull] IUserSessionService sessionService,
        [NotNull] AuthenticationStateProvider authenticationProvider,
        [NotNull] TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(authenticationProvider);

        _client = client;
        _sessionService = sessionService;
        _authenticationProvider = authenticationProvider;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        AuthenticationState state = await _authenticationProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
        string sessionId = state.User.GetSessionId();

        string messageId = UniqueIdHelper.GenerateUniqueStringId();
        UserSession? session = await _sessionService.GetSessionAsync(sessionId).ConfigureAwait(false) ?? throw new InvalidOperationException("Session not found.");
        Metadata metadata = new(
            new MessageMetadata(
            messageId,
            string.Empty,
            1,
            new AggregateMetadata(command),
            _timeProvider.GetLocalNow()),
            new ContextMetadata(
                messageId,
                session.UserId,
                session.PartitionId,
                _timeProvider.GetLocalNow(),
                null,
                session.Id,
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
    private Task SubmitCommandAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);

        return _client.PostAsJsonAsync("api/commands", new MessageState((PolymorphicRecordBase)command, metadata), cancellationToken);
    }
}