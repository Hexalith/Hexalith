﻿// <copyright file="BaseEventTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Events;

using System.Text.Json;

using FluentAssertions;

using Hexalith.PolymorphicSerializations;

public class BaseEventTest
{
    public BaseEventTest() => Hexalith.UnitTests.Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyEvent1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<Polymorphic>(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        Polymorphic result = JsonSerializer.Deserialize<Polymorphic>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyEvent1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyEvent1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyEvent1 result = JsonSerializer.Deserialize<DummyEvent1>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }
}