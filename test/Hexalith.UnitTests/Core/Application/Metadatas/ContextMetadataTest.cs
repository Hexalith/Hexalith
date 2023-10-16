// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : JérômePiquot
// Created          : 01-22-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 01-22-2023
// ***********************************************************************
// <copyright file="ContextMetadataTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;

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
            DateTimeOffset.UtcNow,
            101,
            "session-6987");

        string json = JsonSerializer.Serialize(meta);
        ContextMetadata result = JsonSerializer.Deserialize<ContextMetadata>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(meta);
    }
}