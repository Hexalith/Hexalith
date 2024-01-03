// <copyright file="SalesLineItemTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Sales.ValueObjects;

using FluentAssertions;

using Hexalith.Extensions.Helpers;
using Hexalith.TestMocks;

public class SalesLineItemTest : SerializationTestBase
{
    [Fact]
    public void SalesLineItemShouldBeEqualToSameSalesLineItem()
    {
        // Arrange
        Hexalith.Domain.ValueObjets.SalesLineItem salesLineItem1 = DummySalesDomainHelper.DummySalesLineItem();
        Hexalith.Domain.ValueObjets.SalesLineItem salesLineItem2 = DummySalesDomainHelper.DummySalesLineItem();

        // Act
        bool result = salesLineItem1.AreSame(salesLineItem2);

        // Assert
        _ = result.Should().BeTrue();
    }

    public override object ToSerializeObject() => DummySalesDomainHelper.DummySalesLineItem();
}