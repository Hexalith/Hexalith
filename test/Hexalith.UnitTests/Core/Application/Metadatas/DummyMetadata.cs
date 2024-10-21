// <copyright file="DummyMetadata.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System.Runtime.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;

[DataContract]
public record DummyMetadata(MessageMetadata Message, ContextMetadata Context)
    : Metadata(Message, Context)
{
    public DummyMetadata(object message)
        : this(
            new MessageMetadata(message, DateTimeOffset.UtcNow),
            new ContextMetadata(
                UniqueIdHelper.GenerateUniqueStringId(),
                "Test User",
                "PART1",
                DateTimeOffset.UtcNow,
                102,
                "GFD4565",
                []))
    {
    }
}