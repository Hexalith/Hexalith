﻿// <copyright file="BaseRequestTest.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Requests;

public class BaseRequestTest
{
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyRequest1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<BaseRequest>(original);
        BaseRequest result = JsonSerializer.Deserialize<BaseRequest>(json);
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