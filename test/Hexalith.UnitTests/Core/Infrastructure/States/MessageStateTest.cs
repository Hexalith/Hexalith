// <copyright file="MessageStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;
using Hexalith.UnitTests.Core.Application.Commands;

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

    public MessageStateTest() => Extensions.HexalithUnitTests.RegisterPolymorphicMappers();

    [Fact]
    public void DeserializeShouldSucceed()
    {
        MessageState state = JsonSerializer.Deserialize<MessageState>(_json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = state.Should().NotBeNull();
        _ = state.Message.Should().NotBeNull();
        _ = state.MessageObject.Should().NotBeNull();
        _ = state.MessageObject.Should().BeOfType<DummyCommand1>();
        _ = state.MessageObject.As<DummyCommand1>().Value1.Should().Be(123456);
        _ = state.MessageObject.As<DummyCommand1>().BaseValue.Should().Be("Test");
        _ = state.Metadata.Should().NotBeNull();
        _ = state.Metadata!.Context.Should().NotBeNull();
        _ = state.Metadata.Context!.CorrelationId.Should().Be("COR1234566");
        _ = state.Metadata.Context.UserId.Should().Be("TestUser");
        _ = state.Metadata.Message.Should().NotBeNull();
        _ = state.Metadata.Message.Id.Should().Be("23rZSlC-ukyRtjuSQfI6BQ");
        _ = state.Metadata.Message.Name.Should().Be("DummyCommand1");
        _ = state.Metadata.Message.Version.Should().Be(1);
    }

    [Fact]
    public void SerializeShouldSucceed()
    {
        DummyCommand1 command = new("Test", 123456);
        MessageState messageState = new(
            command,
            new Metadata(
                MessageMetadata.Create(
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
        _ = json.Should().Contain(PolymorphicHelper.Discriminator);
        _ = json.Should().Contain(nameof(DummyCommand1));
        _ = json.Should().Contain(messageState.Metadata.Context.CorrelationId);
        _ = json.Should().Contain(nameof(DummyCommand1.Value1));
        _ = json.Should().Contain(command.Value1.ToInvariantString());
        _ = json.Should().Contain(nameof(DummyCommand1.BaseValue));
        _ = json.Should().Contain(command.BaseValue);
        _ = json.Should().Contain(messageState.Metadata.Message.Id);
    }
}