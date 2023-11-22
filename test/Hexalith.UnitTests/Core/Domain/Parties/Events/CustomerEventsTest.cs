// <copyright file="CustomerEventsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Parties.Events;

using FluentAssertions;

using Hexalith.Domain.Events;

public class CustomerEventsTest
{
    [Fact]
    public void CustomerInformationChangedCheckAggregateId()
    {
        CustomerInformationChanged e = DummyPartiesDomainHelper
            .DummyCustomerInformationChanged();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void CustomerInformationChangedShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper
            .DummyCustomerInformationChanged()
            .Should()
            .BeDataContractSerializable();

    [Fact]
    public void CustomerRegisteredCheckAggregateId()
    {
        CustomerRegistered e = DummyPartiesDomainHelper
            .DummyCustomerRegistered();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void CustomerRegisteredShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper
            .DummyCustomerRegistered()
            .Should()
            .BeDataContractSerializable();

    [Fact]
    public void IntercompanyDropshipDeliveryForCustomerDeselectedCheckAggregateId()
    {
        IntercompanyDropshipDeliveryForCustomerDeselected e = DummyPartiesDomainHelper
            .DummyIntercompanyDropshipDeliveryForCustomerDeselected();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void IntercompanyDropshipDeliveryForCustomerDeselectedShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper
            .DummyIntercompanyDropshipDeliveryForCustomerDeselected()
            .Should()
            .BeDataContractSerializable();

    [Fact]
    public void IntercompanyDropshipDeliveryForCustomerSelectedCheckAggregateId()
    {
        IntercompanyDropshipDeliveryForCustomerSelected e = DummyPartiesDomainHelper
            .DummyIntercompanyDropshipDeliveryForCustomerSelected();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void IntercompanyDropshipDeliveryForCustomerSelectedShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper
            .DummyIntercompanyDropshipDeliveryForCustomerSelected()
            .Should()
            .BeDataContractSerializable();
}