// <copyright file="BaseCommandTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Text.Json;

using FluentAssertions;

using Hexalith.PolymorphicSerializations;

public class BaseCommandTest
{
    public BaseCommandTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void DataContractSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        _ = original.Should().BeDataContractSerializable();
    }

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<Polymorphic>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        object result = JsonSerializer.Deserialize<Polymorphic>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyCommand1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void PolymorphicSerializeFirstFieldShouldBeType()
    {
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<Polymorphic>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNull();
        _ = json.Should().Contain($"\"{PolymorphicHelper.Discriminator}\": \"{nameof(DummyCommand1)}\"");
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyCommand1 result = JsonSerializer.Deserialize<DummyCommand1>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }
}