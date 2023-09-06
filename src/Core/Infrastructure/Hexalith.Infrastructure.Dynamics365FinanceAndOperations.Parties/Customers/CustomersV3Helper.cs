// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 09-06-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-06-2023
// ***********************************************************************
// <copyright file="CustomersV3Helper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Parties.Customers;

using Hexalith.Domain.Events;

/// <summary>
/// Class CustomersV3Helper.
/// </summary>
public static class CustomersV3Helper
{
    /// <summary>
    /// Converts to Dynamics 365 for finance customer v3.
    /// </summary>
    /// <param name="ev">The ev.</param>
    /// <returns>CustomerV3.</returns>
    public static CustomerV3 ToCustomerV3(this CustomerInformationChanged ev)
    {
        return new CustomerV3(ev.CompanyId, ev.Id)
        {
            AddressCountryRegionId = ev.Contact.PostalAddress?.CountryId,
            AddressCounty = ev.Contact.PostalAddress?.CountyId,
            AddressCity = ev.Contact.PostalAddress?.City,
            AddressDescription = ev.Contact.PostalAddress?.Description,
            AddressState = ev.Contact.PostalAddress?.StateName,
            AddressStreet = ev.Contact.PostalAddress?.Street,
            AddressStreetNumber = ev.Contact.PostalAddress?.StreetNumber,
            AddressZipCode = ev.Contact.PostalAddress?.ZipCode,
            PersonFirstName = ev.Contact.Person?.FirstName,
            PersonLastName = ev.Contact.Person?.LastName,
            PersonGender = ev.Contact.Person?.Gender.ToString(),
            PrimaryContactEmail = ev.Contact.Email,
            PrimaryContactPhoneExtension = string.IsNullOrWhiteSpace(ev.Contact.Mobile) ? ev.Contact.Phone : ev.Contact.Mobile,
            PrimaryContactPhoneIsMobile = string.IsNullOrWhiteSpace(ev.Contact.Mobile) ? "No" : "Yes",
            WarehouseId = ev.WarehouseId,
            CommissionSalesGroupId = ev.CommissionSalesGroupId,
        };
    }
}