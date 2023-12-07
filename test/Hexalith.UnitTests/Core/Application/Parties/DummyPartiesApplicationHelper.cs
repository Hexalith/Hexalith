// <copyright file="DummyPartiesApplicationHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Parties;

using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.ValueObjets;
using Hexalith.UnitTests.Core.Domain.Parties;

public static class DummyPartiesApplicationHelper
{
    public static ChangeCustomerInformation DummyChangeCustomerInformation()
        => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456",
            "My customer changed",
            PartyType.Organisation,
            DummyPartiesDomainHelper.DummyContact(),
            "WH2",
            "COM5",
            "GRP2",
            "EUR",
            new DateTimeOffset(2003, 10, 25, 11, 16, 35, TimeSpan.FromHours(3)));

    public static DeselectIntercompanyDropshipDeliveryForCustomer DummyDeselectIntercompanyDropshipDeliveryForCustomer()
            => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456");

    public static RegisterCustomer DummyRegisterCustomer()
            => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456",
            "My customer",
            PartyType.Person,
            DummyPartiesDomainHelper.DummyContact(),
            "WH1",
            "COM6",
            "GRP1",
            "YEN",
            new DateTimeOffset(2002, 11, 5, 10, 6, 25, TimeSpan.Zero));

    public static SelectIntercompanyDropshipDeliveryForCustomer DummySelectIntercompanyDropshipDeliveryForCustomer()
        => new(
            "PART1",
            "Company1",
            "ORIG1",
            "Cust123456");
}