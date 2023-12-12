// <copyright file="CustomerEventHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Bspk.Customers.Infrastructure.IntegrationEventHandlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

/// <summary>
/// Class CustomerEventHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <typeparam name="TCustomerEvent">The type of the t customer event.</typeparam>
/// <seealso cref="IntegrationEventHandler`1" />
public abstract partial class CustomerEventsHandler<TCustomerEvent> : IntegrationEventHandler<TCustomerEvent>
    where TCustomerEvent : IEvent
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerEventsHandler{TCustomerEvent}" /> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected CustomerEventsHandler(
        IDynamics365FinanceClient<CustomerV3> customerService,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(logger);
        _customerService = customerService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(TCustomerEvent @event, CancellationToken cancellationToken)
        => @event is CustomerEvent customerEvent ? await ApplyCustomerEventAsync(customerEvent, cancellationToken).ConfigureAwait(false) : [];

    /// <summary>
    /// Apply customer event as an asynchronous operation.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Collections.Generic.IEnumerable.<Hexalith.Application.Commands.BaseCommand>&gt; representing the asynchronous operation.</returns>
    protected async Task<IEnumerable<BaseCommand>> ApplyCustomerEventAsync(CustomerEvent @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        LogManageCustomerEvent(
            @event.AggregateId,
            @event.TypeName,
            @event.AggregateName,
            @event.AggregateId);

        Task task = @event switch
        {
            CustomerRegistered registered => _customerService
                .PostAsync(registered.ToDynamics365FinanceCustomer(), CancellationToken.None),
            CustomerInformationChanged changed => _customerService
                .PatchAsync(new CustomerAccountKey(@event.CompanyId, @event.Id), changed.ToDynamics365FinanceCustomer(), CancellationToken.None),
            _ => Task.CompletedTask,
        };
        await task.ConfigureAwait(false);
        return [];
    }

    /// <summary>
    /// Logs the manage customer event.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    [LoggerMessage(
      EventId = 1,
      Level = LogLevel.Information,
      Message = "Handling customer event : MessageId={messageId}; MessageType={messageType}; AggregateName={aggregateName}; AggregateId={aggregateId}",
      EventName = "HandleCustomerEvent")]
    protected partial void LogManageCustomerEvent(string messageId, string messageType, string aggregateName, string aggregateId);
}