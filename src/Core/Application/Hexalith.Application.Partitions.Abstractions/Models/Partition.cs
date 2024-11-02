// <copyright file="Partition.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Partitions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents a partition with its identifier, name, and status.
/// </summary>
/// <param name="Id">The unique identifier of the partition.</param>
/// <param name="Name">The name of the partition.</param>
/// <param name="Disabled">Indicates whether the partition is disabled.</param>
[DataContract]
public record Partition(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    string Name,
    [property: DataMember(Order = 3)]
    bool Disabled);