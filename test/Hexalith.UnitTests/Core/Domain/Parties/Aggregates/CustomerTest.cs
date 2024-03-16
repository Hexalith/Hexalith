// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 12-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="CustomerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Domain.Parties.Aggregates;

using FluentAssertions;

using Hexalith.Domain.Aggregates;
using Hexalith.TestMocks;

/// <summary>
/// Class CustomerInformationChangedTest.
/// Implements the <see cref="Hexalith.TestMocks.PolymorphicSerializationTestBase{Hexalith.Domain.Events.CustomerInformationChanged, Hexalith.Domain.Events.BaseEvent}" />.
/// </summary>
/// <seealso cref="Hexalith.TestMocks.PolymorphicSerializationTestBase{Hexalith.Domain.Events.CustomerInformationChanged, Hexalith.Domain.Events.BaseEvent}" />
public class CustomerTest : SerializationTestBase
{
    /// <summary>
    /// Defines the test method CustomerRegisteredCheckAggregateId.
    /// </summary>
    [Fact]
    public void CustomerCheckAggregateId()
    {
        Customer c = DummyPartiesDomainHelper.DummyCustomer();
        _ = c.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    /// <summary>
    /// Converts to serialize object.
    /// </summary>
    /// <returns>System.Object.</returns>
    public override object ToSerializeObject() => DummyPartiesDomainHelper.DummyCustomerInformationChanged();
}