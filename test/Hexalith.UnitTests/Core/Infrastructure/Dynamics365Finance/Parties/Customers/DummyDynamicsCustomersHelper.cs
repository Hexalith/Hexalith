// <copyright file="DummyDynamicsCustomersHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Parties.Customers;

using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

public static class DummyDynamicsCustomersHelper
{
    public static CustomerV3 DummyCustomerV3 => new(
        "FRRT",
        "Cust123456",
        "W/\"JzQwOzE2MjY0NjU5MzI7MTYyNjQ2NTkzMyc=\"",
        "30",
        "Person",
        "John Doe",
        "EUR",
        "WH2",
        "COM5",
        "Primary address",
        "125",
        "Rue de Madrid",
        "IDF",
        "IDF",
        "Paris",
        "75008",
        "FRA",
        "FR",
        "+33659564520",
        "Yes",
        "test@hotmail.tst",
        "Doe",
        "John",
        "Male");

    public static CustomerV3Create DummyCustomerV3Create => new()
    {
        DataAreaId = "FRRT",
        AddressCountryRegionId = "FRA",
        AddressCity = "Paris",
        AddressDescription = "Test",
        AddressStreet = "Rue de Madrid",
        AddressStreetNumber = "125",
        AddressZipCode = "75008",
        CommissionSalesGroupId = "COM5",
        CustomerGroupId = "30",
        OrganizationName = "John Doe",
        PartyType = "Person",
        PersonFirstName = "John",
        PersonLastName = "Doe",
        PersonGender = "Male",
        PersonPersonalTitle = "Mr",
        PrimaryContactEmail = "test@hotmail.tst",
        PrimaryContactPhoneExtension = "+33659564520",
        PrimaryContactPhoneIsMobile = "Yes",
        SalesCurrencyCode = "EUR",
        WarehouseId = "WH2",
        PersonBirthDay = 22,
        PersonBirthMonth = 1,
        PersonBirthYear = 1964,
    };
}