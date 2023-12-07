// <copyright file="CustomerRegisteredTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Parties.Events;

using FluentAssertions;

using Hexalith.Domain.Events;
using Hexalith.TestMocks;

public class CustomerRegisteredTest : PolymorphicSerializationTestBase<CustomerRegistered, BaseEvent>
{
    [Fact]
    public void CustomerRegisteredCheckAggregateId()
    {
        CustomerRegistered e = DummyPartiesDomainHelper
            .DummyCustomerRegistered();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    public override object ToSerializeObject() => DummyPartiesDomainHelper.DummyCustomerRegistered();
}