// <copyright file="NotificationStateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Abstractions.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Serialization.Helpers;
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
        JsonSerializerOptions options = new JsonSerializerOptions().AddPolymorphism();
        string json = JsonSerializer.Serialize(state, options);
        _ = json.Should().NotBeNullOrEmpty();
        NotificationState result = JsonSerializer.Deserialize<NotificationState>(json, options);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void State_serialization_should_succeed()
    {
        DummyNotification1 notification = new();
        Hexalith.Application.Abstractions.Metadatas.Metadata meta = notification.CreateMetadata();
        NotificationState state = new(DateTimeOffset.UtcNow, notification, meta);
        string json = JsonSerializer.Serialize(state, new JsonSerializerOptions().AddPolymorphism());
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"$type\":\"{nameof(DummyNotification1)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(notification.Value1)}\":{notification.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(notification.BaseValue)}\":\"{notification.BaseValue}\"");
    }
}