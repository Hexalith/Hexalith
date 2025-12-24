// <copyright file="MessageMetadataTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System.Text.Json;

using Hexalith.Commons.Metadatas;
using Hexalith.UnitTests.Core.Domain.Messages;

using Shouldly;

public class MessageMetadataTest
{
    [Fact]
    public void MessageMetadataShouldBeEqualAfterSerializeDeserialize()
    {
        MessageMetadata meta = new("ID123", "Message 123", 123, new DomainMetadata("101", "Dummy"), TimeProvider.System.GetLocalNow());
        string json = JsonSerializer.Serialize(meta);
        Metadata result = JsonSerializer.Deserialize<Metadata>(json);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(result);
    }

    [Fact]
    public void MetadataShouldContainMessageNameAndVersion1()
    {
        MessageMetadata meta = new MyDummyMessage("123", "Hello", 124).CreateMessageMetadata(TimeProvider.System.GetLocalNow());
        meta.Name.ShouldBe(nameof(MyDummyMessage));
        meta.Version.ShouldBe(1);
    }

    [Fact]
    public void MetadataShouldContainMessageNameAndVersionInAttribute()
    {
        MessageMetadata meta = new MyDummyMessage3("123", "Hello", 124).CreateMessageMetadata(TimeProvider.System.GetLocalNow());
        meta.Name.ShouldBe("MyMessage");
        meta.Version.ShouldBe(3);
    }

    [Fact]
    public void MetadataShouldContainMessageVersionInAttribute()
    {
        MessageMetadata meta = new MyDummyMessage2("123", "Hello", 124).CreateMessageMetadata(TimeProvider.System.GetLocalNow());
        meta.Name.ShouldBe(nameof(MyDummyMessage2));
        meta.Version.ShouldBe(2);
    }
}