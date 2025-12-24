// <copyright file="BaseMessageTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using System.Text.Json;

using Hexalith.PolymorphicSerializations;
using Hexalith.UnitTests.Extensions;

using Shouldly;

public class BaseMessageTest
{
    public BaseMessageTest() => HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyMessage1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<Polymorphic>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        object result = JsonSerializer.Deserialize<Polymorphic>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        result.ShouldNotBeNull();
        result.ShouldBeOfType<DummyMessage1>();
        result.ShouldBeEquivalentTo(original);
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyMessage1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        DummyMessage1 result = JsonSerializer.Deserialize<DummyMessage1>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(original);
    }
}