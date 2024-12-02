// <copyright file="ServerCommandService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ServerCommandService : ICommandService
{
    private readonly IDomainCommandProcessor _commandProcessor;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerCommandService"/> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="sessionService">The session service.</param>
    public ServerCommandService(
        [NotNull] IDomainCommandProcessor commandProcessor,
        [NotNull] TimeProvider timeProvider,
        [NotNull] ISessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(commandProcessor);
        _commandProcessor = commandProcessor;
        _timeProvider = timeProvider;
        _sessionService = sessionService;
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ClaimsPrincipal user, object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(user.Identity?.Name))
        {
            throw new InvalidOperationException("User name empty.");
        }

        SessionInformation session = await _sessionService.GetAsync(user.Identity.Name, cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(session.PartitionId))
        {
            throw new InvalidOperationException("Partition not set. PartitionId is empty.");
        }

        Metadata metadata = Metadata.CreateNew(command, user.Identity.Name, session.PartitionId, _timeProvider.GetLocalNow());

        await _commandProcessor.SubmitAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }
}