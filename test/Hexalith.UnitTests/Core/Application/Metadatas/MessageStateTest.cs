// <copyright file="MessageStateTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.MessageMetadatas;
using Hexalith.PolymorphicSerialization;

public class MessageStateTest
{
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        PolymorphicSerializationMapper<DummyMessage, PolymorphicRecordBase> mapper = new("DummyMessageV1");
        DummyMessage message = new(101, "Hello");
        Metadata meta = new(
            new MessageMetadata(
                message,
                DateTimeOffset.UtcNow),
            new ContextMetadata(
                "325431",
                "user123",
                DateTimeOffset.UtcNow,
                null,
                null,
                []));
        MessageState messageState = new(
            message,
            meta);
        JsonSerializerOptions jsonOptions = new()
        {
            TypeInfoResolver = new PolymorphicSerializationResolver([mapper]),
        };
        string json = JsonSerializer.Serialize(messageState, jsonOptions);
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, jsonOptions);
        _ = result.Should().NotBeNull();
        _ = result.Message.Should().BeOfType<DummyMessage>();
        _ = result.Should().BeEquivalentTo(messageState);
    }
}