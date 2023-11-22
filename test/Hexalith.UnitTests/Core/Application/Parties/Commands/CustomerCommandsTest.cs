// <copyright file="CustomerCommandsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Parties.Commands;

using FluentAssertions;

using Hexalith.Application.Parties.Commands;
using Hexalith.UnitTests.Core.Application.Parties;

public class CustomerCommandsTest
{
    [Fact]
    public void ChangeCustomerInformationCheckAggregateId()
    {
        ChangeCustomerInformation e = DummyPartiesApplicationHelper
            .DummyChangeCustomerInformation();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void ChangeCustomerInformationShouldBeDataContractSerializable()
        => DummyPartiesApplicationHelper
            .DummyChangeCustomerInformation()
            .Should()
            .BeDataContractSerializable();

    [Fact]
    public void DeselectIntercompanyDropshipDeliveryForCustomerCheckAggregateId()
    {
        DeselectIntercompanyDropshipDeliveryForCustomer e = DummyPartiesApplicationHelper
            .DummyDeselectIntercompanyDropshipDeliveryForCustomer();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void DeselectIntercompanyDropshipDeliveryForCustomerShouldBeDataContractSerializable()
        => DummyPartiesApplicationHelper
            .DummyDeselectIntercompanyDropshipDeliveryForCustomer()
            .Should()
            .BeDataContractSerializable();

    [Fact]
    public void RegisterCustomerCheckAggregateId()
    {
        RegisterCustomer e = DummyPartiesApplicationHelper
            .DummyRegisterCustomer();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void RegisterCustomerShouldBeDataContractSerializable()
        => DummyPartiesApplicationHelper
            .DummyRegisterCustomer()
            .Should()
            .BeDataContractSerializable();

    [Fact]
    public void SelectIntercompanyDropshipDeliveryForCustomerCheckAggregateId()
    {
        SelectIntercompanyDropshipDeliveryForCustomer e = DummyPartiesApplicationHelper
            .DummySelectIntercompanyDropshipDeliveryForCustomer();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    [Fact]
    public void SelectIntercompanyDropshipDeliveryForCustomerShouldBeDataContractSerializable()
        => DummyPartiesApplicationHelper
            .DummySelectIntercompanyDropshipDeliveryForCustomer()
            .Should()
            .BeDataContractSerializable();
}