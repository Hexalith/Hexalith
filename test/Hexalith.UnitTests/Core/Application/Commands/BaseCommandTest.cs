// <copyright file="BaseCommandTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Infrastructure.Serialization.Helpers;

public class BaseCommandTest
{
    [Fact]
    public void Polymorphic_serialize_and_deserialize_should_return_same_object()
    {
        JsonSerializerOptions options = new JsonSerializerOptions().AddPolymorphism();
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<BaseCommand>(original, options);
        BaseCommand result = JsonSerializer.Deserialize<BaseCommand>(json, options);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyCommand1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void Serialize_and_deserialize_should_return_same_object()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyCommand1 result = JsonSerializer.Deserialize<DummyCommand1>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void Data_contract_serialize_and_deserialize_should_return_same_object()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        _ = original.Should().BeDataContractSerializable();
    }

    [Fact]
    public void Binary_serialize_and_deserialize_should_return_same_object()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        _ = original.Should().BeBinarySerializable();
    }
}