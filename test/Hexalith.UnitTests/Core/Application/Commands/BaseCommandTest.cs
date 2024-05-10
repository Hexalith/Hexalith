// <copyright file="BaseCommandTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;

public class BaseCommandTest
{
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
        string json = JsonSerializer.Serialize<BaseCommand>(original);
        BaseCommand result = JsonSerializer.Deserialize<BaseCommand>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyCommand1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void PolymorphicSerializeFirstFieldShouldBeType()
    {
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<BaseCommand>(original);
        _ = json.Should().NotBeNull();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyCommand1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.MajorVersionPropertyName}\":{original.MajorVersion.ToInvariantString()}");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.MinorVersionPropertyName}\":{original.MinorVersion.ToInvariantString()}");
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