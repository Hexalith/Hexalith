// <copyright file="BaseCommandTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.PolymorphicSerializations;

using Shouldly;

public class BaseCommandTest
{
    public BaseCommandTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void DataContractSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        DataContractSerializer serializer = new(typeof(DummyCommand1));
        using MemoryStream stream = new();
        serializer.WriteObject(stream, original);
        stream.Position = 0;
        DummyCommand1 result = (DummyCommand1)serializer.ReadObject(stream);
        result.ShouldBeEquivalentTo(original);
    }

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<Polymorphic>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        object result = JsonSerializer.Deserialize<Polymorphic>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        result.ShouldNotBeNull();
        result.ShouldBeOfType<DummyCommand1>();
        result.ShouldBeEquivalentTo(original);
    }

    [Fact]
    public void PolymorphicSerializeFirstFieldShouldBeType()
    {
        DummyCommand1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<Polymorphic>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeNull();
        json.ShouldContain($"\"{PolymorphicHelper.Discriminator}\": \"{nameof(DummyCommand1)}\"");
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyCommand1 result = JsonSerializer.Deserialize<DummyCommand1>(json);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(original);
    }
}