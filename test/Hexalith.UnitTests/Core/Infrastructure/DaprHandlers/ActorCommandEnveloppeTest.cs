namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprHandlers;
using Hexalith.UnitTests.Core.Application.Commands;

public class ActorCommandEnvelopeTest
{
    [Fact]
    public void Serialized_envelope_should_contain_commands ()
    {
        var c1 = DummyCommand1.Create();
        var c2 = DummyCommand2.Create();
        var envelope = new ActorCommandEnvelope(
            new BaseCommand[] { c1, c2 },
            new Metadata[] { c1.CreateMetadata(), c2.CreateMetadata() });
        var json = JsonSerializer.Serialize(envelope);
        json.Should().NotBeNullOrWhiteSpace();
        json.Should().Contain(c1.BaseValue);
        json.Should().Contain(c1.Value1.ToInvariantString());
        json.Should().Contain(c2.BaseValue);
        json.Should().Contain(c2.Value2.ToInvariantString());
        json.Should().Contain(nameof(DummyCommand1));
        json.Should().Contain(nameof(DummyCommand2));
        json.Should().Contain("$type");
    }

    [Fact]
    public void Envelope_serialization_deserialization_should_return_same()
    {
        var c1 = DummyCommand1.Create();
        var c2 = DummyCommand2.Create();
        var envelope = new ActorCommandEnvelope(
            new BaseCommand[] { c1, c2 },
            new Metadata[] { c1.CreateMetadata(), c2.CreateMetadata() });
        var json = JsonSerializer.Serialize(envelope);
        var result = JsonSerializer.Deserialize<ActorCommandEnvelope>(json);
        result.Should().BeEquivalentTo(envelope);
    }

    [Fact]
    public void Envelope_should_be_data_contract_serializable()
    {
        var c1 = DummyCommand1.Create();
        var c2 = DummyCommand2.Create();
        var envelope = new ActorCommandEnvelope(
            new BaseCommand[] { c1, c2 },
            new Metadata[] { c1.CreateMetadata(), c2.CreateMetadata() });
        envelope.Should().BeDataContractSerializable();
    }
}
