// <copyright file="IntegrationEventProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Errors;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Errors;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class Dynamics365FinanceIntegrationEventDispatcher.
/// Implements the <see cref="Hexalith.Dynamics365Finance.Dispatchers.IDynamics365FinanceIntegrationEventProcessor" />.
/// </summary>
/// <seealso cref="Hexalith.Dynamics365Finance.Dispatchers.IDynamics365FinanceIntegrationEventProcessor" />
public partial class IntegrationEventProcessor : IIntegrationEventProcessor
{
    /// <summary>
    /// The command processor.
    /// </summary>
    private readonly ICommandBus _commandBus;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly TimeProvider _dateTimeService;

    private readonly IIntegrationEventDispatcher _dispatcher;
    private readonly ILogger<IntegrationEventProcessor> _logger;
    private readonly INotificationBus _notificationBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEventProcessor"/> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="commandBus">The command bus.</param>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public IntegrationEventProcessor(
        IIntegrationEventDispatcher dispatcher,
        ICommandBus commandBus,
        INotificationBus notificationBus,
        TimeProvider dateTimeService,
        ILogger<IntegrationEventProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(commandBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(notificationBus);
        ArgumentNullException.ThrowIfNull(logger);

        _dispatcher = dispatcher;
        _commandBus = commandBus;
        _dateTimeService = dateTimeService;
        _notificationBus = notificationBus;
        _logger = logger;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "No command generated from event with name '{EventName}', identifier '{AggregateId}' and CorrelationId '{CorrelationId}'.")]
    public partial void LogNoCommandGeneratedInformation(string? eventName, string? aggregateId, string? correlationId);

    /// <inheritdoc/>
    public async Task SubmitAsync(IEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
        try
        {
            List<BaseCommand> commands = (await _dispatcher
                    .ApplyAsync(baseEvent, cancellationToken)
                    .ConfigureAwait(false))
                .SelectMany(p => p)
                .ToList();
            if (commands.Count <= 0)
            {
                LogNoCommandGeneratedInformation(metadata.Message.Name, baseEvent.AggregateId, metadata.Context.CorrelationId);
            }

            foreach (BaseCommand command in commands)
            {
                await _commandBus.PublishAsync(
                        command,
                        Metadata.CreateNew(command, metadata, _dateTimeService.GetUtcNow()),
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            if (ex is ApplicationErrorException)
            {
                throw;
            }

            ApplicationErrorException appException = new(new EventDispatchFailed(baseEvent, ex), ex);
            ApplicationExceptionNotification notification = new(
                baseEvent.AggregateName,
                baseEvent.AggregateId,
                appException);
            DateTimeOffset date = _dateTimeService.GetUtcNow();
            try
            {
                await _notificationBus.PublishAsync(
                    notification,
                    Metadata.CreateNew(notification, metadata, date),
                    cancellationToken)
                    .ConfigureAwait(false);
            }
            catch
            {
                throw appException;
            }

            throw appException;
        }
    }
}