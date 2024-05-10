// <copyright file="EventStateTest.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.UnitTests.Core.Domain.Events;

using Xunit;

public class EventStateTest
{
    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        DummyEvent1 @event = new();
        Hexalith.Application.Metadatas.Metadata meta = @event.CreateMetadata();
        EventState state = new(DateTimeOffset.UtcNow, @event, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        EventState result = JsonSerializer.Deserialize<EventState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationShouldSucceed()
    {
        DummyEvent1 @event = new();
        Hexalith.Application.Metadatas.Metadata meta = @event.CreateMetadata();
        EventState state = new(DateTimeOffset.UtcNow, @event, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyEvent1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(@event.Value1)}\":{@event.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(@event.BaseValue)}\":\"{@event.BaseValue}\"");
    }
}