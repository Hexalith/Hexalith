// <copyright file="CommandStateTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System.Text.Json;

using Hexalith.Application.States;
using Hexalith.Applications.States;
using Hexalith.UnitTests.Core.Application.Commands;

using Shouldly;

public class CommandStateTest
{
    public CommandStateTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 command = DummyCommand1.Create();
        MessageState original = new(
            command,
            command.CreateMetadata());
        string json = JsonSerializer.Serialize(original);
        MessageState result = JsonSerializer.Deserialize<MessageState>(json);
        result.ShouldNotBeNull();
        result.ShouldBeOfType<MessageState>();
        result.MessageObject.ShouldBeOfType<DummyCommand1>();

        // Compare by re-serializing (avoids array/list type mismatch from polymorphic serialization)
        string resultJson = JsonSerializer.Serialize(result);
        resultJson.ShouldBe(json);
    }
}