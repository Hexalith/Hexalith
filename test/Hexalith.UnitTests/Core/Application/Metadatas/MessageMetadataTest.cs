// <copyright file="MessageMetadataTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.UnitTests.Core.Domain.Messages;

public class MessageMetadataTest
{
    [Fact]
    public void MessageMetadataShouldBeEqualAfterSerializeDeserialize()
    {
        MessageMetadata meta = new("ID123", "Message 123", 123, new AggregateMetadata("101", "Dummy"), TimeProvider.System.GetLocalNow());
        string json = JsonSerializer.Serialize(meta);
        Metadata result = JsonSerializer.Deserialize<Metadata>(json);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(result);
    }

    [Fact]
    public void MetadataShouldContainMessageNameAndVersion1()
    {
        MessageMetadata meta = new(new MyDummyMessage("123", "Hello", 124), TimeProvider.System.GetLocalNow());
        _ = meta.Name.Should().Be(nameof(MyDummyMessage));
        _ = meta.Version.Should().Be(1);
    }

    [Fact]
    public void MetadataShouldContainMessageVersionInAttribute()
    {
        MessageMetadata meta = new(new MyDummyMessage2("123", "Hello", 124), TimeProvider.System.GetLocalNow());
        _ = meta.Name.Should().Be(nameof(MyDummyMessage2));
        _ = meta.Version.Should().Be(2);
    }

    [Fact]
    public void MetadataShouldContainMessageNameAndVersionInAttribute()
    {
        MessageMetadata meta = new(new MyDummyMessage3("123", "Hello", 124), TimeProvider.System.GetLocalNow());
        _ = meta.Name.Should().Be("MyMessage");
        _ = meta.Version.Should().Be(3);
    }
}