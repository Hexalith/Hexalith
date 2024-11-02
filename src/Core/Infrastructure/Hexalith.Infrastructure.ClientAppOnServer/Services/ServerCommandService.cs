// <copyright file="ServerCommandService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ServerCommandService : ICommandService
{
    private readonly IDomainCommandProcessor _commandProcessor;
    private readonly ISessionIdService _sessionIdService;
    private readonly IUserSessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerCommandService"/> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="sessionIdService">The session ID service.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="sessionService">The session service.</param>
    public ServerCommandService(
        [NotNull] IDomainCommandProcessor commandProcessor,
        [NotNull] ISessionIdService sessionIdService,
        [NotNull] TimeProvider timeProvider,
        [NotNull] IUserSessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(commandProcessor);
        ArgumentNullException.ThrowIfNull(sessionIdService);
        _commandProcessor = commandProcessor;
        _sessionIdService = sessionIdService;
        _timeProvider = timeProvider;
        _sessionService = sessionService;
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        _ = UniqueIdHelper.GenerateUniqueStringId();
        string sessionId = await _sessionIdService.GetSessionIdAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Session ID not found.");
        UserSession? session = await _sessionService.GetSessionAsync(sessionId).ConfigureAwait(false);
        if (session == null || string.IsNullOrWhiteSpace(session.Id))
        {
            throw new InvalidOperationException("Session not found or expired.");
        }

        if (string.IsNullOrWhiteSpace(session.UserId))
        {
            throw new InvalidOperationException("User not authenticated. User ID is empty.");
        }

        Metadata metadata = Metadata.CreateNew(command, session.UserId, session.PartitionId, _timeProvider.GetLocalNow());

        await _commandProcessor.SubmitAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }
}