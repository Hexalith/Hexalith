// <copyright file="MessageStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application;
using Hexalith.Application.MessageMetadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;
using Hexalith.UnitTests.Core.Domain.Messages;
using Hexalith.UnitTests.Extensions;

using Xunit;

public class MessageStateTest
{
    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        if (PolymorphicSerializationResolver.DefaultMappers.All(p => p.JsonDerivedType.DerivedType != typeof(MyDummyMessage)))
        {
            PolymorphicSerializationResolver.DefaultMappers = PolymorphicSerializationResolver.DefaultMappers.AddHexalithUnitTestsMappers();
        }

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
        MessageState state = MessageState.Create(message, meta);
        string json = JsonSerializer.Serialize(state, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateWithVersionSerializationAndDeserializationShouldReturnSameObject()
    {
        if (!PolymorphicSerializationResolver.DefaultMappers.Any(p => p.JsonDerivedType.DerivedType == typeof(MyDummyMessage2)))
        {
            PolymorphicSerializationResolver.DefaultMappers = PolymorphicSerializationResolver.DefaultMappers.AddHexalithUnitTestsMappers();
        }

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
        MessageState state = MessageState.Create(message, meta);
        string json = JsonSerializer.Serialize(state, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateWithVersionAndNameSerializationAndDeserializationShouldReturnSameObject()
    {
        if (!PolymorphicSerializationResolver.DefaultMappers.Any(p => p.JsonDerivedType.DerivedType == typeof(MyDummyMessage3)))
        {
            PolymorphicSerializationResolver.DefaultMappers = PolymorphicSerializationResolver.DefaultMappers.AddHexalithUnitTestsMappers();
        }

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
        MessageState state = MessageState.Create(message, meta);
        string json = JsonSerializer.Serialize(state, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationWithVersionAttributeShouldSucceed()
    {
        if (!PolymorphicSerializationResolver.DefaultMappers.Any(p => p.JsonDerivedType.DerivedType == typeof(MyDummyMessage2)))
        {
            PolymorphicSerializationResolver.DefaultMappers = PolymorphicSerializationResolver.DefaultMappers.AddHexalithUnitTestsMappers();
        }

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
        MessageState state = MessageState.Create(message, meta);
        string json = JsonSerializer.Serialize(state, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"$type\": \"MyDummyMessage2V2\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\": \"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(message.Name)}\": \"{message.Name}");
        _ = json.Should().Contain($"\"{nameof(message.Value)}\": {message.Value.ToInvariantString()}");
    }

    [Fact]
    public void StateSerializationWithNameAndVersionAttributeShouldSucceed()
    {
        if (!PolymorphicSerializationResolver.DefaultMappers.Any(p => p.JsonDerivedType.DerivedType == typeof(MyDummyMessage3)))
        {
            PolymorphicSerializationResolver.DefaultMappers = PolymorphicSerializationResolver.DefaultMappers.AddHexalithUnitTestsMappers();
        }

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
        MessageState state = MessageState.Create(message, meta);
        string json = JsonSerializer.Serialize(state, ApplicationConstants.DefaultJsonSerializerOptions);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"$type\": \"MyMessageV3\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\": \"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(message.Name)}\": \"{message.Name}");
        _ = json.Should().Contain($"\"{nameof(message.Value)}\": {message.Value.ToInvariantString()}");
    }
}