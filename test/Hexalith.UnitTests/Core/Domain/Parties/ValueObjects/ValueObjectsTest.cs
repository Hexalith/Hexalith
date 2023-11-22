// <copyright file="ValueObjectsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Parties.ValueObjects;

using FluentAssertions;

public class ValueObjectsTest
{
    [Fact]
    public void ContactShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper.DummyContact().Should().BeDataContractSerializable();

    [Fact]
    public void PersonShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper.DummyPerson().Should().BeDataContractSerializable();

    [Fact]
    public void PostalAddressShouldBeDataContractSerializable()
        => DummyPartiesDomainHelper.DummyPostalAddress().Should().BeDataContractSerializable();
}