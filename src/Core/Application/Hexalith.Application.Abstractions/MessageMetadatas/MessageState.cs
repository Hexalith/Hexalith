// <copyright file="MessageState.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public record MessageState(
    [property: DataMember(Order = 1)]
    [property: JsonPropertyOrder(1)]
    object Message,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    Metadata Metadata)
{
}