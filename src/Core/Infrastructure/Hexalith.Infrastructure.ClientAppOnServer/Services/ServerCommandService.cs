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
using Hexalith.Infrastructure.ClientApp.Services;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ServerCommandService : ICommandService
{
    private readonly IDomainCommandProcessor _commandProcessor;
    private readonly IHttpContextAccessor _context;
    private readonly IUserSessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerCommandService"/> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="context">The HTTP context accessor.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="sessionService">The user session service.</param>
    public ServerCommandService(
        [NotNull] IDomainCommandProcessor commandProcessor,
        [NotNull] IHttpContextAccessor context,
        [NotNull] TimeProvider timeProvider,
        [NotNull] IUserSessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(commandProcessor);
        _commandProcessor = commandProcessor;
        _context = context;
        _timeProvider = timeProvider;
        _sessionService = sessionService;
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        string? sessionId = _context.HttpContext?.Session.Id;
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new InvalidOperationException("Session Id missing.");
        }

        UserSession? session = await _sessionService
            .GetSessionAsync(sessionId)
            .ConfigureAwait(false);

        if (session == null || string.IsNullOrWhiteSpace(session.Id))
        {
            throw new InvalidOperationException("Session not invalid or expired.");
        }

        if (string.IsNullOrWhiteSpace(session.UserId))
        {
            throw new InvalidOperationException("User not authenticated. User ID is empty.");
        }

        if (string.IsNullOrWhiteSpace(session.PartitionId))
        {
            throw new InvalidOperationException("Partition ID is empty.");
        }

        Metadata metadata = Metadata.CreateNew(command, session.UserId, session.PartitionId, _timeProvider.GetLocalNow());

        await _commandProcessor.SubmitAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }
}