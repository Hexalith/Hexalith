// <copyright file="CommandStateTest.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.UnitTests.Core.Application.Commands;

using Xunit;

public class CommandStateTest
{
    [Fact]
    public void StateSerializationAndDeserializationShouldReturnSameObject()
    {
        DummyCommand1 command = new();
        Hexalith.Application.Metadatas.Metadata meta = command.CreateMetadata();
        CommandState state = new(DateTimeOffset.UtcNow, command, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        CommandState result = JsonSerializer.Deserialize<CommandState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public void StateSerializationShouldSucceed()
    {
        DummyCommand1 command = new();
        Hexalith.Application.Metadatas.Metadata meta = command.CreateMetadata();
        CommandState state = new(DateTimeOffset.UtcNow, command, meta);
        string json = JsonSerializer.Serialize(state);
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(DummyCommand1)}\"");
        _ = json.Should().Contain($"\"{IPolymorphicSerializable.TypeNamePropertyName}\":\"{nameof(Metadata)}\"");
        _ = json.Should().Contain($"\"{nameof(meta.Message.Id)}\":\"{meta.Message.Id}\"");
        _ = json.Should().Contain($"\"{nameof(command.Value1)}\":{command.Value1.ToInvariantString()}");
        _ = json.Should().Contain($"\"{nameof(command.BaseValue)}\":\"{command.BaseValue}\"");
    }
}