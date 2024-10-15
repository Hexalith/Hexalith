﻿// <copyright file="BaseCommandTest.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;
using Hexalith.PolymorphicSerialization;

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
        string json = JsonSerializer.Serialize<object>(original);
        object result = JsonSerializer.Deserialize<object>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyCommand1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void PolymorphicSerializeFirstFieldShouldBeType()
    {
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<object>(original);
        _ = json.Should().NotBeNull();
        _ = json.Should().Contain($"\"{PolymorphicHelper.GetPolymorphicTypeDiscriminator(original)}\":\"{nameof(DummyCommand1)}\"");
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