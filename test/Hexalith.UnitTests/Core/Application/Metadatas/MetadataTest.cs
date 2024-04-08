// <copyright file="MetadataTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;

public class MetadataTest
{
    private static readonly string[] _scopes = ["scope1", "scope9"];

    [Fact]
    public void MetadataShouldBeEqualAfterSerializeDeserialize()
    {
        Metadata meta = GetMetadata();
        string json = JsonSerializer.Serialize(meta);
        Metadata result = JsonSerializer.Deserialize<Metadata>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void MetadataShouldSerializeSuccessfuly()
    {
        Metadata meta = GetMetadata();
        string json = JsonSerializer.Serialize(meta);
        _ = json.Should().NotBeNull();
        _ = json.Should().Contain(meta.Message.Id);
    }

    private static Metadata GetMetadata()
    {
        return new(
            new MessageMetadata(
                "123-456-789",
                "TestMessage",
                DateTimeOffset.UtcNow.AddSeconds(-1),
                new MessageVersion(4, 6),
                new AggregateMetadata("123-AG", "TestAggregate")),
            new ContextMetadata("COR-6589", "TestUser", DateTimeOffset.UtcNow, 101, "session-6987"),
            _scopes);
    }
}