// <copyright file="Metadata.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the metadata of a message.
/// </summary>
/// <param name="Message">The message metadata.</param>
/// <param name="Context">The context metadata.</param>
[DataContract]
public record Metadata(
    [property: DataMember(Order = 1)]
    [property: JsonPropertyOrder(1)]
    MessageMetadata Message,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    ContextMetadata Context)
{
}