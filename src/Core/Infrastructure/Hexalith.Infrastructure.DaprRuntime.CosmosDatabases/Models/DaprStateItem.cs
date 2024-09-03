// <copyright file="DaprStateItem.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public record DaprStateItem(
    [property: DataMember(Order = 1, Name ="id")]
    [property: JsonPropertyName("id")]
    string Id)
{
}