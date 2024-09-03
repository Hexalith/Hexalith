// <copyright file="AggregateMetadata.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The aggregate metadata.
/// </summary>
/// <param name="Id">The aggregate identifier.</param>
/// <param name="Name">The aggregate name.</param>
[DataContract]
public record AggregateMetadata(
    [property:DataMember(Order = 1)]
    [property:JsonPropertyOrder(1)]
    string Id,
    [property:DataMember(Order = 2)]
    [property:JsonPropertyOrder(2)]
    string Name)
{
}