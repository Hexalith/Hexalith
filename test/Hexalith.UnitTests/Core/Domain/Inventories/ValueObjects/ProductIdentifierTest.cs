// <copyright file="ProductIdentifierTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Inventories.ValueObjects;

using FluentAssertions;

using Hexalith.TestMocks;
using Hexalith.UnitTests.Core.Domain.Inventories;

public class ProductIdentifierTest
{
    [Fact]
    public void ContactShouldBeDataContractSerializable()
        => DummyInventoriesDomainHelper.DummyProductIdentifier().Should().BeDataContractSerializable();

    [Fact]
    public void ContactShouldBeJsonSerializable()
        => DummyInventoriesDomainHelper.DummyProductIdentifier().Should().BeJsonSerializable();
}