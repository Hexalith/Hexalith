// <copyright file="CommandStateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.States;

using Hexalith.UnitTests.Core.Application.Commands;

public class CommandStateTest
{
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        DummyCommand1 command = DummyCommand1.Create();
        CommandState original = new(
            DateTimeOffset.Now,
            command,
            command.CreateMetadata());
        string json = JsonSerializer.Serialize(original);
        CommandState result = JsonSerializer.Deserialize<CommandState>(json);
        _ = result.Should().NotBeNull();
        _ = result.Message.Should().BeOfType<DummyCommand1>();
        _ = result.Should().BeEquivalentTo(original);
    }
}