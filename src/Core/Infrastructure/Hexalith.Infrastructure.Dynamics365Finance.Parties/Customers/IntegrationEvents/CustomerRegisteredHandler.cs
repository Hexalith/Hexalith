// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="CustomerRegisteredHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using System.Threading;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class CustomerRegisteredHandler.
/// Implements the <see cref="CustomerEventsHandler{CustomerRegistered}" />.
/// </summary>
/// <seealso cref="CustomerEventsHandler{CustomerRegistered}" />
public class CustomerRegisteredHandler : IntegrationEventHandler<CustomerRegistered>
{
    /// <summary>
    /// The customer base service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerBase> _customerBaseService;

    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;

    /// <summary>
    /// The external code service.
    /// </summary>
    private readonly IDynamics365FinanceClient<CustomerExternalSystemCode> _externalCodeService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<CustomerRegisteredHandler> _logger;

    /// <summary>
    /// The organization settings.
    /// </summary>
    private readonly IOptions<OrganizationSettings> _organizationSettings;

    /// <summary>
    /// The origin identifier.
    /// </summary>
    private readonly string? _originId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRegisteredHandler" /> class.
    /// </summary>
    /// <param name="customerBaseService">The customer base service.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="externalCodeService">The external code service.</param>
    /// <param name="organizationSettings">The organization settings.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public CustomerRegisteredHandler(
        IDynamics365FinanceClient<CustomerBase> customerBaseService,
        IDynamics365FinanceClient<CustomerV3> customerService,
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCodeService,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<CustomerRegisteredHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(customerBaseService);
        ArgumentNullException.ThrowIfNull(customerService);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(organizationSettings);
        ArgumentNullException.ThrowIfNull(logger);

        ArgumentException.ThrowIfNullOrEmpty(organizationSettings.Value.DefaultOriginId);

        _customerBaseService = customerBaseService;
        _customerService = customerService;
        _externalCodeService = externalCodeService;
        _organizationSettings = organizationSettings;
        _logger = logger;
        _originId = _organizationSettings.Value.DefaultOriginId;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerRegistered @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        if (@event.OriginId.Equals(_originId, StringComparison.OrdinalIgnoreCase))
        {
            return [];
        }

        // Find the customer in Dynamics 365 for Finance
        string? hexalithExternalCodeCustomer = await FindExternalCodeAsync(@event.CompanyId, nameof(Hexalith), @event.AggregateId, cancellationToken)
                .ConfigureAwait(false);
        string? originExternalCodeCustomer = await FindExternalCodeAsync(@event.CompanyId, @event.OriginId, @event.Id, cancellationToken)
                .ConfigureAwait(false);
        string tempName = $"[{@event.AggregateId}]";
        string? customerAccount = await FindAsync(@event.CompanyId, tempName, cancellationToken)
                .ConfigureAwait(false);
        CustomerAccountKey customerKey;
        if (string.IsNullOrWhiteSpace(customerAccount))
        {
            // No incompleted customer found in Dynamics 365 Finance.
            if (string.IsNullOrWhiteSpace(hexalithExternalCodeCustomer) && string.IsNullOrWhiteSpace(originExternalCodeCustomer))
            {
                // The external codes are not mapped to an existing Dynamics 365 Finance customer, create a new one.
                CustomerV3Create create = @event.ToDynamics365FinanceCustomerCreate(tempName);
                CustomerV3 customer = await _customerService
                        .PostAsync(create, cancellationToken)
                        .ConfigureAwait(false);
                customerAccount = customer.CustomerAccount;
                customerKey = new(@event.CompanyId, customerAccount);
            }
            else
            {
                // At least one external code is mapped to an existing customer, throw an error.
                throw new InvalidOperationException($"Customer {@event.AggregateId} already exists in Dynamics 365 for Finance company {@event.CompanyId} : {hexalithExternalCodeCustomer} {originExternalCodeCustomer}");
            }
        }
        else
        {
            // An incompleted customer was found in Dynamics 365 Finance.
            customerKey = new(@event.CompanyId, customerAccount);
        }

        DateTimeOffset? birthDate = @event.Contact?.Person?.BirthDate;
        if (string.IsNullOrWhiteSpace(originExternalCodeCustomer))
        {
            _ = await _externalCodeService
                    .PostAsync(
                        new CustomerExternalSystemCode(
                            null,
                            @event.CompanyId,
                            @event.OriginId,
                            customerAccount,
                            @event.Id),
                        cancellationToken).ConfigureAwait(false);
        }

        if (string.IsNullOrWhiteSpace(hexalithExternalCodeCustomer))
        {
            _ = await _externalCodeService
                    .PostAsync(
                        new CustomerExternalSystemCode(
                        null,
                        @event.CompanyId,
                        nameof(Hexalith),
                        customerAccount,
                        @event.AggregateId),
                        cancellationToken).ConfigureAwait(false);
        }

        await _customerBaseService.PatchAsync(
            customerKey,
            new CustomerBase(
                @event.CompanyId,
                @event.Id,
                null,
                @event.Name,
                @event.Contact?.Person?.Title,
                birthDate?.Day,
                birthDate?.Month,
                birthDate?.Year),
            cancellationToken).ConfigureAwait(false);
        return [];
    }

    /// <summary>
    /// Find as an asynchronous operation.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="tempName">Name of the temporary.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;string?&gt; representing the asynchronous operation.</returns>
    private async Task<string?> FindAsync(string companyId, string tempName, CancellationToken cancellationToken)
    {
        IEnumerable<CustomerBase> result = await _customerBaseService.GetAsync(
                    new CustomerByNameFilter(companyId, tempName),
                    cancellationToken).ConfigureAwait(false);
        return result.FirstOrDefault()?.CustomerAccount;
    }

    /// <summary>
    /// Find external code as an asynchronous operation.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;string?&gt; representing the asynchronous operation.</returns>
    private async Task<string?> FindExternalCodeAsync(string companyId, string originId, string externalId, CancellationToken cancellationToken)
    {
        IEnumerable<CustomerExternalSystemCode> externalCodes = await _externalCodeService
             .GetAsync(
                 new CustomerExternalCodeFilter(
                 companyId,
                 originId,
                 externalId),
                 cancellationToken).ConfigureAwait(false);
        return externalCodes.FirstOrDefault()?.CustomerAccountNumber;
    }
}