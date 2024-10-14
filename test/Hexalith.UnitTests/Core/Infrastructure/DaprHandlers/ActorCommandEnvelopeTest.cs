// <copyright file="ActorCommandEnvelopeTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.UnitTests.Core.Application.Commands;

/// <summary>
/// Class ActorCommandEnvelopeTest.
/// </summary>
public class ActorMessageEnvelopeTest
{
    /// <summary>
    /// Defines the test method Envelope_serialization_deserialization_should_return_same.
    /// </summary>
    [Fact]
    [Obsolete]
    public void EnvelopeSerializationDeserializationShouldReturnSame()
    {
        DummyCommand1 c1 = DummyCommand1.Create();
        DummyCommand2 c2 = DummyCommand2.Create();
        ActorCommandEnvelope envelope = new(
            new BaseCommand[] { c1, c2 },
            new Metadata[] { c1.CreateMetadata(), c2.CreateMetadata() });
        string json = JsonSerializer.Serialize(envelope);
        ActorCommandEnvelope result = JsonSerializer.Deserialize<ActorCommandEnvelope>(json);
        _ = result.Should().BeEquivalentTo(envelope);
    }

    /// <summary>
    /// Defines the test method Envelope_should_be_data_contract_serializable.
    /// </summary>
    [Fact]
    [Obsolete]
    public void EnvelopeShouldBeDataContractSerializable()
    {
        DummyCommand1 c1 = DummyCommand1.Create();
        DummyCommand2 c2 = DummyCommand2.Create();
        ActorCommandEnvelope envelope = new(
            new BaseCommand[] { c1, c2 },
            new Metadata[] { c1.CreateMetadata(), c2.CreateMetadata() });
        _ = envelope.Should().BeDataContractSerializable();
    }

    /// <summary>
    /// Defines the test method Serialized_envelope_should_contain_commands.
    /// </summary>
    [Fact]
    [Obsolete]
    public void SerializedEnvelopeShouldContainCommands()
    {
        DummyCommand1 c1 = DummyCommand1.Create();
        DummyCommand2 c2 = DummyCommand2.Create();
        ActorCommandEnvelope envelope = new(
            new BaseCommand[] { c1, c2 },
            new Metadata[] { c1.CreateMetadata(), c2.CreateMetadata() });
        string json = JsonSerializer.Serialize(envelope);
        _ = json.Should().NotBeNullOrWhiteSpace();
        _ = json.Should().Contain(c1.BaseValue);
        _ = json.Should().Contain(c1.Value1.ToInvariantString());
        _ = json.Should().Contain(c2.BaseValue);
        _ = json.Should().Contain(c2.Value2.ToInvariantString());
        _ = json.Should().Contain(nameof(DummyCommand1));
        _ = json.Should().Contain(nameof(DummyCommand2));
        _ = json.Should().Contain("$type_name");
    }
}