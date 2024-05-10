// <copyright file="MessageStateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;
using Hexalith.UnitTests.Core.Application.Commands;

public class MessageStateTest
{
    private const string _json = $$"""
    {
        "IdempotencyId":"20230125085001962",
        "Message":{
            "$type_name":"DummyCommand1",
            "$version_major":4,
            "$version_minor":6,
            "Value1":123456,
            "BaseValue":"Test"
        },
        "Metadata":{
            "$type_name":"Metadata",
            "Context":{
                "CorrelationId":"20230125085001962",
                "UserId":"TestUser"
            },
            "Message":{
                "Id":"20230125085001962",
                "Name":"DummyCommand1",
                "Version":{"Major":0,"Minor":0},
                "Aggregate":{"Id":"Test-123456","Name":"Test"},
                "Date":"2023-01-25T08:50:01.9630826+00:00"
            },
            "Version":{"Major":0,"Minor":0}
        },
        "ReceivedDate":"2023-01-25T08:50:01.9630825+00:00"
    }
    """;

    [Fact]
    public void DeserializeShouldSucceed()
    {
        MessageState state = JsonSerializer.Deserialize<MessageState>(_json);
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
        _ = state.Metadata.Message.Version.Major.Should().Be(0);
        _ = state.Metadata.Message.Version.Minor.Should().Be(0);
    }

    [Fact]
    public void SerializeShouldSucceed()
    {
        string messageId = UniqueIdHelper.GenerateDateTimeId();
        DummyCommand1 command = new("Test", 123456);
        MessageState messageState = new(
            DateTimeOffset.UtcNow,
            command,
            new Metadata(
                messageId,
                command,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                        messageId,
                        "TestUser",
                        null,
                        null,
                        null),
                null));
        string json = JsonSerializer.Serialize(messageState);
        _ = json.Should().NotBeEmpty();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyCommand1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"CorrelationId\":\"{messageId}\"");
        _ = json.Should().Contain("\"Value1\":123456");
        _ = json.Should().Contain("\"BaseValue\":\"Test\"");
        _ = json.Should().Contain($"\"Id\":\"{messageId}\"");
    }
}