// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 12-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-07-2023
// ***********************************************************************
// <copyright file="RegisterCustomerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Application.Parties.Commands;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.Commands;
using Hexalith.TestMocks;
using Hexalith.UnitTests.Core.Application.Parties;

/// <summary>
/// Class RegisterCustomerTest.
/// Implements the <see cref="Hexalith.TestMocks.PolymorphicSerializationTestBase{Hexalith.Application.Parties.Commands.RegisterCustomer, Hexalith.Application.Commands.BaseCommand}" />.
/// </summary>
/// <seealso cref="Hexalith.TestMocks.PolymorphicSerializationTestBase{Hexalith.Application.Parties.Commands.RegisterCustomer, Hexalith.Application.Commands.BaseCommand}" />
public class RegisterCustomerTest : PolymorphicSerializationTestBase<RegisterCustomer, BaseCommand>
{
    /// <summary>
    /// Defines the test method RegisterCustomerCheckAggregateId.
    /// </summary>
    [Fact]
    public void RegisterCustomerCheckAggregateId()
    {
        RegisterCustomer e = DummyPartiesApplicationHelper
            .DummyRegisterCustomer();
        _ = e.AggregateId.Should().Be("Customer-PART1-Company1-ORIG1-Cust123456");
    }

    /// <summary>
    /// Converts to serialize object.
    /// </summary>
    /// <returns>System.Object.</returns>
    public override object ToSerializeObject() => DummyPartiesApplicationHelper.DummyRegisterCustomer();
}