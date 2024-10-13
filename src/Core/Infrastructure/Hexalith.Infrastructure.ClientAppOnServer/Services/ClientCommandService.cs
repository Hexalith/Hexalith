// <copyright file="ClientCommandService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.MessageMetadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public class ClientCommandService : IClientCommandService
{
    private readonly IDomainCommandProcessor _commandProcessor;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCommandService"/> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="sessionService">The session service.</param>
    public ClientCommandService(
        [NotNull] IDomainCommandProcessor commandProcessor,
        [NotNull] TimeProvider timeProvider,
        [NotNull] IUserService userService,
        [NotNull] ISessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(commandProcessor);
        _commandProcessor = commandProcessor;
        _timeProvider = timeProvider;
        _userService = userService;
        _sessionService = sessionService;
    }

    /// <inheritdoc/>
    public Task SendCommandAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);

        return _commandProcessor.SubmitAsync(command, metadata, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SendCommandAsync(object command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string messageId = UniqueIdHelper.GenerateUniqueStringId();
        string userId = await _userService.GetUserIdAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new InvalidOperationException("User not authenticated. User ID is empty.");
        }

        string sessionId = await _sessionService.GetSessionIdAsync(cancellationToken).ConfigureAwait(false);
        string partitionId = await _sessionService.GetPartitionIdAsync(cancellationToken).ConfigureAwait(false);
        Metadata metadata = new(
            new MessageMetadata(command, _timeProvider.GetLocalNow()),
            new ContextMetadata(messageId, userId, partitionId, null, null, sessionId, []));

        await SendCommandAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }
}