// <copyright file="MessageStateTest.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.UnitTests.Core.Domain.Messages;

using Xunit;

public class MessageStateTest
{
    [Fact]
    public void State_serialization_and_deserialization_should_return_same_object()
    {
        DummyMessage1 message = new();
        Hexalith.Application.Abstractions.Metadatas.Metadata meta = message.CreateMetadata();
        MessageState state = new(DateTimeOffset.UtcNow, message, meta);
        JsonSerializerOptions options = new JsonSerializerOptions().AddPolymorphism();
        string json = JsonSerializer.Serialize(state, options);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, options);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void State_serialization_should_succeed()
    {
        DummyMessage1 message = new();
        Hexalith.Application.Abstractions.Metadatas.Metadata meta = message.CreateMetadata();
        MessageState state = new(DateTimeOffset.UtcNow, message, meta);
        string json = JsonSerializer.Serialize(state, new JsonSerializerOptions().AddPolymorphism());
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"$type\":\"{nameof(DummyMessage1)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(message.Value1)}\":{message.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(message.BaseValue)}\":\"{message.BaseValue}\"");
    }
}