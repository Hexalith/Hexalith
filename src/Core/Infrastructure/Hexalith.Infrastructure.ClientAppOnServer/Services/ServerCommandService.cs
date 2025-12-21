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

using FluentValidation;
using FluentValidation.Results;

using Hexalith.Application.Commands;
using Hexalith.Commons.Metadatas;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;

using Microsoft.Extensions.Logging;
using Hexalith.Applications.Commands;

/// <summary>
/// Represents a service for sending commands asynchronously.
/// </summary>
public partial class ServerCommandService : ICommandService
{
    private readonly IDomainCommandProcessor _commandProcessor;
    private readonly ILogger<ServerCommandService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerCommandService"/> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="sessionService">The session service.</param>
    /// <param name="logger">The logger.</param>
    public ServerCommandService(
        [NotNull] IDomainCommandProcessor commandProcessor,
        [NotNull] IServiceProvider serviceProvider,
        [NotNull] TimeProvider timeProvider,
        [NotNull] ISessionService sessionService,
        [NotNull] ILogger<ServerCommandService> logger)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(commandProcessor);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _commandProcessor = commandProcessor;
        _serviceProvider = serviceProvider;
        _timeProvider = timeProvider;
        _sessionService = sessionService;
        _logger = logger;
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

        // Get the IValidator<T> for the command type
        Type validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
        if (_serviceProvider.GetService(validatorType) is not IValidator validator)
        {
            LogMissingCommandValidatorWarning(_logger, command.GetType().Name);
        }
        else
        {
            ValidationResult validationResult = validator.Validate(new ValidationContext<object>(command));
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        Metadata metadata = command.CreateMetadata(user.Identity.Name, session.PartitionId, _timeProvider.GetLocalNow());

        await _commandProcessor.SubmitAsync(command, metadata, cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(
        0,
        LogLevel.Warning,
        "No validator found for command type {CommandType}.",
        EventName = "MissingCommandValidator")]
    private static partial void LogMissingCommandValidatorWarning(ILogger logger, string commandType);
}