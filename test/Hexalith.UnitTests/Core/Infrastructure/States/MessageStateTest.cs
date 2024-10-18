// <copyright file="MessageStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.MessageMetadatas;
using Hexalith.PolymorphicSerialization;
using Hexalith.UnitTests.Core.Application.Commands;

public class MessageStateTest
{
    private const string _json = $$"""
    {
        "IdempotencyId":"20230125085001962",
        "RecordObject":{
            "$type":"DummyCommand1",
            "Value1":123456,
            "BaseValue":"Test"
        },
        "Metadata":{
            "Context":{
                "CorrelationId":"20230125085001962",
                "UserId":"TestUser"
            },
            "Message":{
                "Id":"20230125085001962",
                "Name":"DummyCommand1",
                "Version": 2,
                "Aggregate":{"Id":"Test-123456","Name":"Test"},
                "Date":"2023-01-25T08:50:01.9630826+00:00"
            }
        },
        "ReceivedDate":"2023-01-25T08:50:01.9630825+00:00"
    }
    """;

    public MessageStateTest() => Extensions.HexalithUnitTests.RegisterPolymorphicMappers();

    [Fact]
    public void DeserializeShouldSucceed()
    {
        MessageState state = JsonSerializer.Deserialize<MessageState>(_json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = state.Should().NotBeNull();
        _ = state.Message.Should().BeOfType<DummyCommand1>();
        _ = state.Message.As<DummyCommand1>().Value1.Should().Be(123456);
        _ = state.Message.As<DummyCommand1>().BaseValue.Should().Be("Test");
        _ = state.Metadata.Should().NotBeNull();
        _ = state.Metadata!.Context.Should().NotBeNull();
        _ = state.Metadata.Context!.CorrelationId.Should().Be("20230125085001962");
        _ = state.Metadata.Context.UserId.Should().Be("TestUser");
        _ = state.Metadata.Message.Should().NotBeNull();
        _ = state.Metadata.Message.Id.Should().Be("20230125085001962");
        _ = state.Metadata.Message.Name.Should().Be("DummyCommand1");
        _ = state.Metadata.Message.Version.Should().Be(2);
    }

    [Fact]
    public void SerializeShouldSucceed()
    {
        DummyCommand1 command = new("Test", 123456);
        MessageState messageState = MessageState.Create(
            command,
            new Metadata(
                new MessageMetadata(
                    command,
                    DateTimeOffset.UtcNow),
                new ContextMetadata(
                        "COR1234566",
                        "TestUser",
                        "Part1",
                        DateTimeOffset.UtcNow,
                        100,
                        "session-56",
                        [])));
        string json = JsonSerializer.Serialize(messageState, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeEmpty();
        _ = json.Should().Contain($"\"{PolymorphicHelper.Discriminator}\": \"{nameof(DummyCommand1)}\"");
        _ = json.Should().Contain($"\"CorrelationId\": \"{messageState.Metadata.Context.CorrelationId}\"");
        _ = json.Should().Contain("\"Value1\": 123456");
        _ = json.Should().Contain("\"BaseValue\": \"Test\"");
        _ = json.Should().Contain($"\"Id\": \"{messageState.Metadata.Message.Id}\"");
    }
}