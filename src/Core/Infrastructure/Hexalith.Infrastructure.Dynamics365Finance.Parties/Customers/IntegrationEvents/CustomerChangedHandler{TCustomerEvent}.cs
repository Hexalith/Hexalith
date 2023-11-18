// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-18-2023
// ***********************************************************************
// <copyright file="CustomerChangedHandler{TCustomerEvent}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

/// <summary>
/// Class CustomerChangedHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <typeparam name="TCustomerEvent">The type of the t customer event.</typeparam>
/// <seealso cref="IntegrationEventHandler`1" />
public abstract class CustomerChangedHandler<TCustomerEvent> : IntegrationEventHandler<TCustomerEvent>
    where TCustomerEvent : CustomerEvent
{
    private readonly IDynamics365FinanceClient<CustomerV3> _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerChangedHandler{TCustomerEvent}"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected CustomerChangedHandler(IDynamics365FinanceClient<CustomerV3> customerService)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        _customerService = customerService;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseCommand>> ApplyAsync(TCustomerEvent @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        CustomerV3 customer = @event switch
        {
            CustomerInformationChanged changed => ToCustomer(changed),
            CustomerRegistered registered => ToCustomer(registered),
            _ => throw new ArgumentException($"Event {@event.GetType().Name} is not a valid customer event. Expected : {nameof(CustomerRegistered)}; {nameof(CustomerInformationChanged)}.", nameof(@event)),
        };
        _ = await _customerService
            .PostAsync(customer, CancellationToken.None)
            .ConfigureAwait(false);
        return [];
    }

    private static CustomerV3 ToCustomer(CustomerRegistered customerRegistered)
    {
        CustomerV3 customerV3 = new(customerRegistered.CompanyId)
        {
            AddressCity = customerRegistered.Contact.PostalAddress?.City,
            AddressCountryRegionId = customerRegistered.Contact.PostalAddress?.CountyId,
            AddressState = customerRegistered.Contact.PostalAddress?.StateId,
            AddressStreet = customerRegistered.Contact.PostalAddress?.Street,
            AddressZipCode = customerRegistered.Contact.PostalAddress?.ZipCode,
            AddressDescription = customerRegistered.Contact.PostalAddress?.Name,
            PersonFirstName = customerRegistered.Contact.Person?.FirstName,
            PersonLastName = customerRegistered.Contact.Person?.LastName,
            PersonGender = ToGender(customerRegistered.Contact.Person?.Gender),
            PrimaryContactPhoneExtension = customerRegistered.Contact?.Mobile ?? customerRegistered.Contact?.Phone,
            PrimaryContactPhoneIsMobile = customerRegistered.Contact?.Mobile == null ? "No" : "Yes",
            CustomerGroupId = "30",
            SalesCurrencyCode = "EUR",
            PartyType = "Person",
        };
        return customerV3;
    }

    private static CustomerV3 ToCustomer(CustomerInformationChanged customerChanged)
    {
        CustomerV3 customerV3 = new(customerChanged.CompanyId)
        {
            AddressCity = customerChanged.Contact.PostalAddress?.City,
            AddressCountryRegionId = customerChanged.Contact.PostalAddress?.CountyId,
            AddressState = customerChanged.Contact.PostalAddress?.StateId,
            AddressStreet = customerChanged.Contact.PostalAddress?.Street,
            AddressZipCode = customerChanged.Contact.PostalAddress?.ZipCode,
            AddressDescription = customerChanged.Contact.PostalAddress?.Name,
            PersonFirstName = customerChanged.Contact.Person?.FirstName,
            PersonLastName = customerChanged.Contact.Person?.LastName,
            PersonGender = ToGender(customerChanged.Contact.Person?.Gender),
            PrimaryContactPhoneExtension = customerChanged.Contact?.Mobile ?? customerChanged.Contact?.Phone,
            PrimaryContactPhoneIsMobile = customerChanged.Contact?.Mobile == null ? "No" : "Yes",
            CustomerGroupId = "30",
            SalesCurrencyCode = "EUR",
            PartyType = "Person",
        };
        return customerV3;
    }

    private static string? ToGender(Gender? gender)
    {
        return gender switch
        {
            Gender.Male => nameof(Gender.Male),
            Gender.Female => nameof(Gender.Female),
            null => null,
            _ => nameof(Gender.Other),
        };
    }
}