// <copyright file="MessageStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;
using Hexalith.UnitTests.Core.Domain.Messages;

using Xunit;

public class MessageStateTest
{
    public MessageStateTest() => Extensions.HexalithUnitTests.RegisterPolymorphicMappers();

    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        MyDummyMessage message = new("ID21", "My 21 dummies", 21);
        Metadata meta = new(
            new MessageMetadata(message, DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                25686L,
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationWithNameAndVersionAttributeShouldSucceed()
    {
        MyDummyMessage3 message = new("ID22", "My 22 dummies", 22);
        Metadata meta = new(
            new MessageMetadata(message, DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                25686L,
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"MyMessageV3");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Name)}\": \"MyMessage\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Version)}\": 3");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\": \"{meta.Message.Id}\"");
        _ = json.Should().Contain(message.Value.ToInvariantString());
    }

    [Fact]
    public void StateSerializationWithVersionAttributeShouldSucceed()
    {
        MyDummyMessage2 message = new("ID22", "My 22 dummies", 22);
        Metadata meta = new(
            new MessageMetadata(message, DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                25686L,
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = state.Message.Should().Contain($"MyDummyMessage2V2");
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"MyDummyMessage2V2");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Version)}\": 2");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\": \"{meta.Message.Id}\"");
        _ = json.Should().Contain(message.Name);
        _ = json.Should().Contain(message.Value.ToInvariantString());
    }

    [Fact]
    public void StateWithVersionAndNameSerializationAndDeserializationShouldReturnSameObject()
    {
        MyDummyMessage3 message = new("ID21", "My 21 dummies", 21);
        Metadata meta = new(
            new MessageMetadata(message, DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                25686L,
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateWithVersionSerializationAndDeserializationShouldReturnSameObject()
    {
        MyDummyMessage2 message = new("ID21", "My 21 dummies", 21);
        Metadata meta = new(
            new MessageMetadata(message, DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                25686L,
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }
}