// <copyright file="DaprStateItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Queries;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public record DaprStateItem(
    [property: DataMember(Order = 1, Name ="id")]
    [property: JsonPropertyName("id")]
    string Id)
{
}