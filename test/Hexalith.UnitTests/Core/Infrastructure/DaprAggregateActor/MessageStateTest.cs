namespace Hexalith.UnitTests.Core.Infrastructure.DaprAggregateActor;

using FluentAssertions;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprAggregateActor;
using Hexalith.Infrastructure.Serialization.Helpers;
using Hexalith.UnitTests.Core.Domain;

using System;
using System.Text.Json;

public class MessageStateTest
{
    private static readonly string _json = $$"""
    {
        "IdempotencyId":"20230125085001962",
        "Message":{
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
    public void Deserialize_should_succeed()
    {
        var state = JsonSerializer.Deserialize<MessageState>(_json, new JsonSerializerOptions().AddPolymorphism());
        state.Should().NotBeNull();
        state!.Message.Should().BeOfType<DummyCommand1>();
        state.Message.As<DummyCommand1>().Value1.Should().Be(123456);
        state.Message.As<DummyCommand1>().BaseValue.Should().Be("Test");
        state.Metadata.Should().NotBeNull();
        state.Metadata!.Context.Should().NotBeNull();
        state.Metadata.Context!.CorrelationId.Should().Be("20230125085001962");
        state.Metadata.Context.UserId.Should().Be("TestUser");
        state.Metadata.Message.Should().NotBeNull();
        state.Metadata.Message.Id.Should().Be("20230125085001962");
        state.Metadata.Message.Name.Should().Be("DummyCommand1");
        state.Metadata.Message.Version.Major.Should().Be(0);
        state.Metadata.Message.Version.Minor.Should().Be(0);
    }

    [Fact]
    public void Serialize_should_succeed()
    {
        var messageId = UniqueIdHelper.GenerateDateTimeId();
        var command = new DummyCommand1("Test", 123456);
        var messageState = new MessageState(
            DateTimeOffset.UtcNow,
            messageId,
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
        string json = JsonSerializer.Serialize(messageState, new JsonSerializerOptions().AddPolymorphism());
        json.Should().NotBeEmpty();
        json.Should().Contain("\"$type\":\"DummyCommand1\"");
        json.Should().Contain($"\"CorrelationId\":\"{messageId}\"");
        json.Should().Contain("\"Value1\":123456");
        json.Should().Contain("\"BaseValue\":\"Test\"");
        json.Should().Contain($"\"Id\":\"{messageId}\"");
    }
}