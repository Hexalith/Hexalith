// <copyright file="IntegrationEventProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a processor for integration events that can generate and publish commands.
/// </summary>
public partial class IntegrationEventProcessor : IIntegrationEventProcessor
{
    private readonly ICommandBus _commandBus;
    private readonly TimeProvider _dateTimeService;
    private readonly IIntegrationEventDispatcher _dispatcher;
    private readonly ILogger<IntegrationEventProcessor> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEventProcessor"/> class.
    /// </summary>
    /// <param name="dispatcher">The integration event dispatcher.</param>
    /// <param name="commandBus">The command bus for publishing generated commands.</param>
    /// <param name="dateTimeService">The service providing date and time information.</param>
    /// <param name="logger">The logger for this class.</param>
    /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
    public IntegrationEventProcessor(
        IIntegrationEventDispatcher dispatcher,
        ICommandBus commandBus,
        TimeProvider dateTimeService,
        ILogger<IntegrationEventProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(commandBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(logger);

        _dispatcher = dispatcher;
        _commandBus = commandBus;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    /// <summary>
    /// Logs information when no command is generated from an event.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <param name="partitionKey">The partition key of the event.</param>
    /// <param name="correlationId">The correlation ID of the event.</param>
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "No command generated from event with name '{EventName}', partition key '{PartitionKey}' and correlation id {CorrelationId}.")]
    public partial void LogNoCommandGeneratedInformation(string? eventName, string? partitionKey, string? correlationId);

    /// <inheritdoc/>
    /// <summary>
    /// Submits an integration event for processing, potentially generating and publishing commands.
    /// </summary>
    /// <param name="baseEvent">The integration event to be processed.</param>
    /// <param name="metadata">The metadata associated with the event.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if baseEvent or metadata is null.</exception>
    public async Task SubmitAsync(object baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        // Apply the event and collect resulting commands
        List<object> commands = (await _dispatcher
                .ApplyAsync(baseEvent, metadata, cancellationToken)
                .ConfigureAwait(false))
            .SelectMany(p => p)
            .ToList();

        if (commands.Count <= 0)
        {
            LogNoCommandGeneratedInformation(metadata.Message.Name, metadata.PartitionKey, metadata.Context.CorrelationId);
        }

        // Publish each generated command
        foreach (object command in commands)
        {
            await _commandBus.PublishAsync(
                    command,
                    Metadata.CreateNew(command, metadata, _dateTimeService.GetUtcNow()),
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
