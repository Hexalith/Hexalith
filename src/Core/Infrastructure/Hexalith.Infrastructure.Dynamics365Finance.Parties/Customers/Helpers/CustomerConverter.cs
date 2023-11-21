// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-21-2023
// ***********************************************************************
// <copyright file="CustomerConverter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;

using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

/// <summary>
/// Class CustomerConverter.
/// </summary>
public static class CustomerConverter
{
    /// <summary>
    /// Converts to dynamics365financecustomer.
    /// </summary>
    /// <param name="customerRegistered">The customer registered.</param>
    /// <returns>CustomerV3.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static CustomerV3 ToDynamics365FinanceCustomer([NotNull] this CustomerRegistered customerRegistered)
    {
        ArgumentNullException.ThrowIfNull(customerRegistered);
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

    /// <summary>
    /// Converts to dynamics365financecustomer.
    /// </summary>
    /// <param name="customerChanged">The customer changed.</param>
    /// <returns>CustomerV3.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static CustomerV3 ToDynamics365FinanceCustomer([NotNull] this CustomerInformationChanged customerChanged)
    {
        ArgumentNullException.ThrowIfNull(customerChanged);
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

    /// <summary>
    /// Converts to gender.
    /// </summary>
    /// <param name="gender">The gender.</param>
    /// <returns>System.Nullable&lt;System.String&gt;.</returns>
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