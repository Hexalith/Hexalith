// <copyright file="MetadataTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using FluentAssertions;

using Hexalith.Application.Abstractions.Metadatas;

using System.Text.Json;

public class MetadataTest
{
    [Fact]
    public void Metadata_should_be_equal_after_serialize_deserialize()
    {
        Metadata meta = GetMetadata();
        string json = JsonSerializer.Serialize(meta);
        Metadata result = JsonSerializer.Deserialize<Metadata>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void Metadata_should_serialize_successfuly()
    {
        Metadata meta = GetMetadata();
        string json = JsonSerializer.Serialize(meta);
        _ = json.Should().NotBeNull();
        _ = json.Should().Contain(meta.Message.Id);
    }

    private static Metadata GetMetadata()
    {
        return new(
            new MetadataVersion(2, 4),
            new MessageMetadata(
                "123-456-789",
                "TestMessage",
                DateTimeOffset.UtcNow.AddSeconds(-1),
                new MessageVersion(4, 6),
                new AggregateMetadata("123-AG", "TestAggregate")),
            new ContextMetadata("COR-6589", "TestUser", DateTimeOffset.UtcNow, 101, "session-6987"),
            new[] { "scope1", "scope9" });
    }
}