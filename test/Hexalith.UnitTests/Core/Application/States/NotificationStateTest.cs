// <copyright file="NotificationStateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;
using Hexalith.UnitTests.Core.Application.Notifications;

using Xunit;

public class NotificationStateTest
{
    [Fact]
    public void State_serialization_and_deserialization_should_return_same_object()
    {
        DummyNotification1 notification = new();
        Hexalith.Application.Abstractions.Metadatas.Metadata meta = notification.CreateMetadata();
        NotificationState state = new(DateTimeOffset.UtcNow, notification, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        NotificationState result = JsonSerializer.Deserialize<NotificationState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void State_serialization_should_succeed()
    {
        DummyNotification1 notification = new();
        Hexalith.Application.Abstractions.Metadatas.Metadata meta = notification.CreateMetadata();
        NotificationState state = new(DateTimeOffset.UtcNow, notification, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"{PolymorphicJsonConverter<DummyNotification1>.TypeNamePropertyName}\":\"{nameof(DummyNotification1)}\"");
        _ = json.Should().Contain($"\"{PolymorphicJsonConverter<DummyNotification1>.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(notification.Value1)}\":{notification.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(notification.BaseValue)}\":\"{notification.BaseValue}\"");
    }
}