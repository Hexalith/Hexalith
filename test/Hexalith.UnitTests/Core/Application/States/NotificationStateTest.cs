// <copyright file="NotificationStateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;
using Hexalith.UnitTests.Core.Application.Notifications;

using Xunit;

public class NotificationStateTest
{
    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        DummyNotification1 notification = new();
        Hexalith.Application.Metadatas.Metadata meta = notification.CreateMetadata();
        NotificationState state = new(DateTimeOffset.UtcNow, notification, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        NotificationState result = JsonSerializer.Deserialize<NotificationState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationShouldSucceed()
    {
        DummyNotification1 notification = new();
        Hexalith.Application.Metadatas.Metadata meta = notification.CreateMetadata();
        NotificationState state = new(DateTimeOffset.UtcNow, notification, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyNotification1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(notification.Value1)}\":{notification.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(notification.BaseValue)}\":\"{notification.BaseValue}\"");
    }
}