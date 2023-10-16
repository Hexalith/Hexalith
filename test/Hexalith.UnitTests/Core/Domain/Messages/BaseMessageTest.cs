// <copyright file="BaseMessageTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Messages;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Domain.Messages;

public class BaseMessageTest
{
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyMessage1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<BaseMessage>(original);
        BaseMessage result = JsonSerializer.Deserialize<BaseMessage>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyMessage1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyMessage1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyMessage1 result = JsonSerializer.Deserialize<DummyMessage1>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }
}