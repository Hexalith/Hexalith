// <copyright file="DummyMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Application.Envelopes;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Helpers;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class DummyMetadata : Metadata
{
    [JsonConstructor]
    public DummyMetadata(
        MetadataVersion version,
        MessageMetadata message,
        ContextMetadata context,
        IEnumerable<string>? scopes)
        : base(version, message, context, scopes)
    {
    }

    public DummyMetadata(IMessage message)
        : base(
        UniqueIdHelper.GenerateUniqueStringId(),
        message,
        DateTimeOffset.UtcNow,
        new ContextMetadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            "Test User",
            DateTimeOffset.UtcNow,
            102,
            "GFD4565"),
        new[] { "test123456" })
    {
    }
}
