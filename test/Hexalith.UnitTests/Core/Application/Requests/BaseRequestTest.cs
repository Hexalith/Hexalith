// <copyright file="BaseRequestTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using System.Text.Json;

using FluentAssertions;

using Hexalith.PolymorphicSerialization;

public class BaseRequestTest
{
    public BaseRequestTest() => Extensions.HexalithUnitTests.RegisterPolymorphicMappers();

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyRequest1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<PolymorphicRecordBase>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        object result = JsonSerializer.Deserialize<PolymorphicRecordBase>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyRequest1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyRequest1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        DummyRequest1 result = JsonSerializer.Deserialize<DummyRequest1>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }
}