// <copyright file="DummyPartiesDomainHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Parties;

using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;

public static class DummyPartiesDomainHelper
{
    public static Contact DummyContact()
        => new(
            DummyPerson(),
            DummyPostalAddress(),
            "jdoe@mymail.com",
            "+33321563",
            "+33652952");

    public static CustomerInformationChanged DummyCustomerInformationChanged()
        => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456",
            "My customer changed",
            DummyContact(),
            "WH2",
            "GRP2",
            new DateTimeOffset(2003, 10, 25, 11, 16, 35, TimeSpan.FromHours(3)));

    public static CustomerRegistered DummyCustomerRegistered()
        => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456",
            "My customer",
            DummyContact(),
            "WH1",
            "GRP1",
            new DateTimeOffset(2002, 11, 5, 10, 6, 25, TimeSpan.Zero));

    public static IntercompanyDropshipDeliveryForCustomerDeselected DummyIntercompanyDropshipDeliveryForCustomerDeselected()
            => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456");

    public static IntercompanyDropshipDeliveryForCustomerSelected DummyIntercompanyDropshipDeliveryForCustomerSelected()
        => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456");

    public static Person DummyPerson()
                => new(
            "John Doe",
            "John",
            "Doe",
            new DateTimeOffset(2001, 12, 25, 0, 0, 0, TimeSpan.Zero),
            Gender.Female);

    public static PostalAddress DummyPostalAddress()
        => new(
            "Primary",
            "Primary address",
            "31",
            "Coventry street",
            "25669",
            "67008",
            "London",
            "GB",
            "LDN",
            "London City",
            "GBR",
            "Great Britain",
            "GB");
}