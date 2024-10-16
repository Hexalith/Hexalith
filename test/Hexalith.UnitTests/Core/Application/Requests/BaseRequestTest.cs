// <copyright file="BaseRequestTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using System.Text.Json;

using FluentAssertions;

public class BaseRequestTest
{
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyRequest1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<object>(original);
        object result = JsonSerializer.Deserialize<object>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyRequest1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyRequest1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyRequest1 result = JsonSerializer.Deserialize<DummyRequest1>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }
}