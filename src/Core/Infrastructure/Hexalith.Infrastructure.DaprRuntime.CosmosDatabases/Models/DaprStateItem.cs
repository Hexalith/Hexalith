// <copyright file="DaprStateItem.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a state item in Dapr.
/// </summary>
/// <param name="Id">The state item identifier.</param>
[DataContract]
public record DaprStateItem(
    [property: DataMember(Order = 1, Name ="id")]
    [property: JsonPropertyName("id")]
    string Id);