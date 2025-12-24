// <copyright file="ContextMetadataTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System;
using System.Text.Json;

using Hexalith.Commons.Metadatas;

using Shouldly;

/// <summary>
/// Class ContextMetadataTest.
/// </summary>
public class ContextMetadataTest
{
    [Fact]
    public void ContextMetadataShouldBeEqualAfterSerializeDeserialize()
    {
        ContextMetadata meta = new(
            "COR-6589",
            "TestUser",
            "PART-123",
            DateTimeOffset.UtcNow,
            TimeSpan.FromSeconds(150),
            101,
            "etag-4587",
            "session-6987",
            []);

        string json = JsonSerializer.Serialize(meta);
        ContextMetadata result = JsonSerializer.Deserialize<ContextMetadata>(json);
        result.ShouldNotBeNull();
        result.CorrelationId.ShouldBe(meta.CorrelationId);
        result.UserId.ShouldBe(meta.UserId);
        result.PartitionId.ShouldBe(meta.PartitionId);
        result.ReceivedDate.ShouldBe(meta.ReceivedDate);
        result.SequenceNumber.ShouldBe(meta.SequenceNumber);
        result.SessionId.ShouldBe(meta.SessionId);
        result.Scopes.ShouldBe(meta.Scopes);
    }
}