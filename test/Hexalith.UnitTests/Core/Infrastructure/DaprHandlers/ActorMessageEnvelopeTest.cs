// <copyright file="ActorMessageEnvelopeTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.PolymorphicSerializations;
using Hexalith.UnitTests.Core.Application.Commands;

using Shouldly;

/// <summary>
/// Class ActorMessageEnvelopeTest.
/// </summary>
public class ActorMessageEnvelopeTest
{
    public ActorMessageEnvelopeTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    /// <summary>
    /// Defines the test method Envelope_serialization_deserialization_should_return_same.
    /// </summary>
    [Fact]
    public void EnvelopeSerializationDeserializationShouldReturnSame()
    {
        DummyCommand1 c1 = DummyCommand1.Create();
        ActorMessageEnvelope envelope = ActorMessageEnvelope.Create(c1, c1.CreateMetadata());
        string json = JsonSerializer.Serialize(envelope);
        ActorMessageEnvelope result = JsonSerializer.Deserialize<ActorMessageEnvelope>(json);
        result.ShouldBeEquivalentTo(envelope);
    }

    /// <summary>
    /// Defines the test method Envelope_should_be_data_contract_serializable.
    /// </summary>
    [Fact]
    public void EnvelopeShouldBeDataContractSerializable()
    {
        DummyCommand1 c1 = DummyCommand1.Create();
        ActorMessageEnvelope envelope = ActorMessageEnvelope.Create(c1, c1.CreateMetadata());
        DataContractSerializer serializer = new(typeof(ActorMessageEnvelope));
        using MemoryStream stream = new();
        serializer.WriteObject(stream, envelope);
        stream.Position = 0;
        ActorMessageEnvelope result = (ActorMessageEnvelope)serializer.ReadObject(stream);
        result.ShouldBeEquivalentTo(envelope);
    }

    /// <summary>
    /// Defines the test method Serialized_envelope_should_contain_commands.
    /// </summary>
    [Fact]
    public void SerializedEnvelopeShouldContainCommands()
    {
        DummyCommand1 c1 = DummyCommand1.Create();
        ActorMessageEnvelope envelope = ActorMessageEnvelope.Create(c1, c1.CreateMetadata());
        string json = JsonSerializer.Serialize(envelope, PolymorphicHelper.DefaultJsonSerializerOptions);
        json.ShouldNotBeNullOrWhiteSpace();
        json.ShouldContain(nameof(envelope.Message));
        json.ShouldContain(nameof(envelope.Metadata));
    }
}