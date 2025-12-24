// <copyright file="MessageStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.States;

using System;
using System.Text.Json;

using Hexalith.Applications.States;
using Hexalith.Commons.Metadatas;
using Hexalith.Commons.Strings;
using Hexalith.PolymorphicSerializations;
using Hexalith.UnitTests.Core.Application.Commands;

using Shouldly;

public class MessageStateTest
{
    private const string _json = $$"""
    {
      "Message": "{\r\n  \u0022$type\u0022: \u0022DummyCommand1\u0022,\r\n  \u0022Value1\u0022: 123456,\r\n  \u0022AggregateId\u0022: \u0022Test-123456\u0022,\r\n  \u0022BaseValue\u0022: \u0022Test\u0022,\r\n  \u0022AggregateName\u0022: \u0022Test\u0022\r\n}",
      "Metadata": {
        "AggregateGlobalId": "Part1-Test-Test-123456",
        "Message": {
          "Id": "23rZSlC-ukyRtjuSQfI6BQ",
          "Name": "DummyCommand1",
          "Version": 1,
          "Aggregate": {
            "Id": "Test-123456",
            "Name": "Test"
          },
          "CreatedDate": "2024-12-06T07:28:45.4546543+00:00"
        },
        "Context": {
          "CorrelationId": "COR1234566",
          "UserId": "TestUser",
          "PartitionId": "Part1",
          "ReceivedDate": "2024-12-06T07:28:45.4572418+00:00",
          "SequenceNumber": 100,
          "SessionId": "session-56",
          "Scopes": []
        }
      }
    }
    """;

    public MessageStateTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void DeserializeShouldSucceed()
    {
        MessageState state = JsonSerializer.Deserialize<MessageState>(_json, PolymorphicHelper.DefaultJsonSerializerOptions);
        state.ShouldNotBeNull();
        state.Message.ShouldNotBeNull();
        state.MessageObject.ShouldNotBeNull();
        state.MessageObject.ShouldBeOfType<DummyCommand1>();
        DummyCommand1 cmd = (DummyCommand1)state.MessageObject;
        cmd.Value1.ShouldBe(123456);
        cmd.BaseValue.ShouldBe("Test");
        state.Metadata.ShouldNotBeNull();
        state.Metadata!.Context.ShouldNotBeNull();
        state.Metadata.Context!.CorrelationId.ShouldBe("COR1234566");
        state.Metadata.Context.UserId.ShouldBe("TestUser");
        state.Metadata.Message.ShouldNotBeNull();
        state.Metadata.Message.Id.ShouldBe("23rZSlC-ukyRtjuSQfI6BQ");
        state.Metadata.Message.Name.ShouldBe("DummyCommand1");
        state.Metadata.Message.Version.ShouldBe(1);
    }

    [Fact]
    public void SerializeShouldSucceed()
    {
        DummyCommand1 command = new("Test", 123456);
        MessageState messageState = new(
            command,
            new Metadata(
                    command.CreateMessageMetadata(DateTimeOffset.UtcNow),
                new ContextMetadata(
                        "COR1234566",
                        "TestUser",
                        "Part1",
                        DateTimeOffset.UtcNow,
                        TimeSpan.FromMinutes(30),
                        100,
                        "ETA-56",
                        "session-56",
                        [])));
        string json = JsonSerializer.Serialize(messageState, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeEmpty();
        json.ShouldContain(PolymorphicHelper.Discriminator);
        json.ShouldContain(nameof(DummyCommand1));
        json.ShouldContain(messageState.Metadata.Context.CorrelationId);
        json.ShouldContain(nameof(DummyCommand1.Value1));
        json.ShouldContain(command.Value1.ToInvariantString());
        json.ShouldContain(nameof(DummyCommand1.BaseValue));
        json.ShouldContain(command.BaseValue);
        json.ShouldContain(messageState.Metadata.Message.Id);
    }
}