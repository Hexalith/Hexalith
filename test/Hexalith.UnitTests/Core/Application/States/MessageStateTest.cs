// <copyright file="MessageStateTest.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.UnitTests.Core.Domain.Messages;

using Xunit;

public class MessageStateTest
{
    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        DummyMessage1 message = new();
        Hexalith.Application.Metadatas.Metadata meta = message.CreateMetadata();
        MessageState state = new(DateTimeOffset.UtcNow, message, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationShouldSucceed()
    {
        DummyMessage1 message = new();
        Hexalith.Application.Metadatas.Metadata meta = message.CreateMetadata();
        MessageState state = new(DateTimeOffset.UtcNow, message, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyMessage1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(message.Value1)}\":{message.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(message.BaseValue)}\":\"{message.BaseValue}\"");
    }
}