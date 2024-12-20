﻿// <copyright file="MetadataTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
    public void MetadataShouldSerializeSuccessfully()
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
                10,
                new AggregateMetadata("123356", "TestAggregate"),
                DateTimeOffset.UtcNow.AddSeconds(-1)),
            new ContextMetadata(
                "COR-123-456-789",
                "USER123",
                "PART1",
                DateTimeOffset.UtcNow,
                25686L,
                "SESS-4566",
                _scopes));
    }
}