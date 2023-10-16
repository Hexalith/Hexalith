// <copyright file="DummyMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Helpers;

[DataContract]
public class DummyMetadata : Metadata
{
    [JsonConstructor]
    public DummyMetadata(
        MessageMetadata message,
        ContextMetadata context,
        IEnumerable<string> scopes)
        : base(message, context, scopes)
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
        null)
    {
    }

    [Obsolete("Only for serialization")]
    public DummyMetadata()
    {
    }
}