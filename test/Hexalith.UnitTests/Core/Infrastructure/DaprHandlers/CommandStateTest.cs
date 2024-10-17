// <copyright file="CommandStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.MessageMetadatas;
using Hexalith.PolymorphicSerialization;
using Hexalith.UnitTests.Core.Application.Commands;

public class CommandStateTest
{
    public CommandStateTest() => Extensions.HexalithUnitTestsMapperExtension.Initialize();

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 command = DummyCommand1.Create();
        MessageState original = MessageState.Create(
            command,
            command.CreateMetadata());
        string json = JsonSerializer.Serialize(original, PolymorphicHelper.DefaultJsonSerializerOptions);
        MessageState result = JsonSerializer.Deserialize<MessageState>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeOfType<MessageState>();
        _ = result.Message.Should().BeOfType<DummyCommand1>();
        _ = result.Should().BeEquivalentTo(original);
    }
}