// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 12-06-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-06-2023
// ***********************************************************************
// <copyright file="CustomerConverter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;

using FluentAssertions;

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;
using Hexalith.UnitTests.Core.Domain.Parties;

/// <summary>
/// Class CustomerConverterTest.
/// </summary>
public class CustomerConverterTest
{
    /// <summary>
    /// Defines the test method CustomerInformationChangedCheckAggregateId.
    /// </summary>
    [Fact]
    public void CustomerInformationChangedCheckAggregateId()
    {
        CustomerInformationChanged e = DummyPartiesDomainHelper
            .DummyCustomerInformationChanged();
        CustomerV3 customer = e.ToDynamics365FinanceCustomer(e.Id);
        CustomerInformationChanged newEvent = customer.ToCustomerChangedEvent(
            e.PartitionId,
            e.OriginId,
            e.Date,
            e.Contact.PostalAddress.PostBox,
            e.Contact.PostalAddress.StateName,
            e.Contact.PostalAddress.CountryName,
            e.Contact.PostalAddress.CountryIso2);
        _ = newEvent.Should().BeEquivalentTo(e);
    }
}