// <copyright file="BaseNotificationTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Notifications;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Notifications;

public class BaseNotificationTest
{
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyNotification1 original = new("IB2343213FR", 655463);
        string json = JsonSerializer.Serialize<BaseNotification>(original);
        BaseNotification result = JsonSerializer.Deserialize<BaseNotification>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<DummyNotification1>();
        _ = result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void SerializeAndDeserializeShouldReturnSameObject()
    {
        DummyNotification1 original = new("IB2343213FR", 1256);
        string json = JsonSerializer.Serialize(original);
        DummyNotification1 result = JsonSerializer.Deserialize<DummyNotification1>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(original);
    }
}