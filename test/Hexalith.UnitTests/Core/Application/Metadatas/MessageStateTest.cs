// <copyright file="MessageStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System;
using System.Text.Json;

using Hexalith.Applications.States;
using Hexalith.Commons.Metadatas;
using Hexalith.Commons.Strings;
using Hexalith.PolymorphicSerializations;
using Hexalith.UnitTests.Core.Domain.Messages;

using Shouldly;

public class MessageStateTest
{
    public MessageStateTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        MyDummyMessage message = new("ID21", "My 21 dummies", 21);
        Metadata meta = new(
            message.CreateMessageMetadata(DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                TimeSpan.FromMinutes(30),
                25686L,
                "Eth-4566",
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        result.ShouldNotBeNull();

        // Compare by re-serializing (avoids array/list type mismatch from polymorphic serialization)
        string resultJson = JsonSerializer.Serialize(result, PolymorphicHelper.DefaultJsonSerializerOptions);
        resultJson.ShouldBe(json);
    }

    [Fact]
    public void StateSerializationWithNameAndVersionAttributeShouldSucceed()
    {
        MyDummyMessage3 message = new("ID22", "My 22 dummies", 22);
        Metadata meta = new(
            message.CreateMessageMetadata(DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                TimeSpan.FromMinutes(30),
                25686L,
                "Eth-4566",
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeNullOrEmpty();
        json.ShouldContain($"MyMessageV3");
        json.ShouldContain($"\"{nameof(meta.Message.Name)}\": \"MyMessage\"");
        json.ShouldContain($"\"{nameof(meta.Message.Version)}\": 3");
        json.ShouldContain($"\"{nameof(meta.Message.Id)}\": \"{meta.Message.Id}\"");
        json.ShouldContain(message.Value.ToInvariantString());
    }

    [Fact]
    public void StateSerializationWithVersionAttributeShouldSucceed()
    {
        MyDummyMessage2 message = new("ID22", "My 22 dummies", 22);
        Metadata meta = new(
            message.CreateMessageMetadata(DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                TimeSpan.FromMinutes(30),
                25686L,
                "Eth-4566",
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        state.Message.ShouldContain($"MyDummyMessage2V2");
        json.ShouldNotBeNullOrEmpty();
        json.ShouldContain($"MyDummyMessage2V2");
        json.ShouldContain($"\"{nameof(meta.Message.Version)}\": 2");
        json.ShouldContain($"\"{nameof(meta.Message.Id)}\": \"{meta.Message.Id}\"");
        json.ShouldContain(message.Name);
        json.ShouldContain(message.Value.ToInvariantString());
    }

    [Fact]
    public void StateWithVersionAndNameSerializationAndDeserializationShouldReturnSameObject()
    {
        MyDummyMessage3 message = new("ID21", "My 21 dummies", 21);
        Metadata meta = new(
            message.CreateMessageMetadata(DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                TimeSpan.FromMinutes(30),
                25686L,
                "Eth-4566",
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        result.ShouldNotBeNull();

        // Compare by re-serializing (avoids array/list type mismatch from polymorphic serialization)
        string resultJson = JsonSerializer.Serialize(result, PolymorphicHelper.DefaultJsonSerializerOptions);
        resultJson.ShouldBe(json);
    }

    [Fact]
    public void StateWithVersionSerializationAndDeserializationShouldReturnSameObject()
    {
        MyDummyMessage2 message = new("ID21", "My 21 dummies", 21);
        Metadata meta = new(
            message.CreateMessageMetadata(DateTimeOffset.UtcNow),
            new ContextMetadata(
                "COR-144662",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                TimeSpan.FromMinutes(30),
                25686L,
                "Eth-4566",
                "SESS-4566",
                ["scope1", "scope9"]));
        MessageState state = new(message, meta);
        string json = JsonSerializer.Serialize(state, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        result.ShouldNotBeNull();

        // Compare by re-serializing (avoids array/list type mismatch from polymorphic serialization)
        string resultJson = JsonSerializer.Serialize(result, PolymorphicHelper.DefaultJsonSerializerOptions);
        resultJson.ShouldBe(json);
    }
}