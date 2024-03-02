// <copyright file="Dynamics365FinanceIntegrationEventProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application;
using Hexalith.Application.Commands;
using Hexalith.Application.Errors;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class Dynamics365FinanceIntegrationEventDispatcher.
/// Implements the <see cref="Hexalith.Infrastructure.Dynamics365Finance.Dispatchers.IDynamics365FinanceIntegrationEventProcessor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.Dynamics365Finance.Dispatchers.IDynamics365FinanceIntegrationEventProcessor" />
public partial class Dynamics365FinanceIntegrationEventProcessor : DependencyInjectionEventDispatcher, IDynamics365FinanceIntegrationEventProcessor
{
    /// <summary>
    /// The command processor.
    /// </summary>
    private readonly ICommandBus _commandBus;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    private readonly ILogger _logger;
    private readonly INotificationBus _notificationBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceIntegrationEventProcessor"/> class.
    /// </summary>
    /// <param name="commandBus">The command processor.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public Dynamics365FinanceIntegrationEventProcessor(
        ICommandBus commandBus,
        IDateTimeService dateTimeService,
        IServiceProvider serviceProvider,
        INotificationBus notificationBus,
        ILogger<Dynamics365FinanceIntegrationEventProcessor> logger)
        : base(serviceProvider, logger)
    {
        ArgumentNullException.ThrowIfNull(commandBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(notificationBus);
        _commandBus = commandBus;
        _dateTimeService = dateTimeService;
        _notificationBus = notificationBus;
        _logger = logger;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Dispatching event with name '{EventName}', identifier '{AggregateId}' on AggregateName '{AggregateName}'.")]
    public partial void LogDispatchingEventDebugInformation(string? eventName, string? aggregateName, string? aggregateId);

    /// <inheritdoc/>
    public async Task SubmitAsync(Dynamics365BusinessEventBase @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        try
        {
            LogDispatchingEventDebugInformation(@event.BusinessEventId, @event.AggregateName, @event.AggregateId);
            List<BaseCommand> commands = (await ApplyAsync(@event, cancellationToken)
                .ConfigureAwait(false))
                .SelectMany(p => p)
                .Union(@event.ToCommands())
                .ToList();
            foreach (BaseCommand cmd in commands)
            {
                await _commandBus.PublishAsync(
                    cmd,
                    Metadata.CreateNew(cmd, @event, _dateTimeService.UtcNow),
                    cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            EventDispatchFailed error = new(@event, ex);
            error.LogApplicationErrorDetails(_logger, ex);
            ApplicationErrorException appException = new(error);
            ApplicationExceptionNotification notification = new(
                @event.Message.Id,
                @event.AggregateName,
                @event.AggregateId,
                appException);
            DateTimeOffset date = _dateTimeService.UtcNow;
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                await _notificationBus.PublishAsync(
                    notification,
                    new Metadata(
                        UniqueIdHelper.GenerateUniqueStringId(),
                        notification,
                        date,
                        new ContextMetadata(
                            @event.EventId ?? UniqueIdHelper.GenerateDateTimeId(),
                            @event.InitiatingUserAzureActiveDirectoryObjectId ?? ApplicationConstants.SystemUser,
                            date,
                            null,
                            null),
                        null),
                    cancellationToken).ConfigureAwait(false);
            }
            catch
            {
            }
#pragma warning restore CA1031 // Do not catch general exception types

            throw appException;
        }
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(IEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (baseEvent is Dynamics365BusinessEventBase businessEvent)
        {
            await SubmitAsync(businessEvent, cancellationToken).ConfigureAwait(false);
            return;
        }

        throw new ApplicationErrorException(new EventNotSupportedByDispatcher(nameof(Dynamics365FinanceIntegrationEventProcessor)));
    }
}