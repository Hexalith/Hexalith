// <copyright file="EntityAggregateState.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Organizations.Aggregates;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Class EntityAggregateState.
/// </summary>
[DataContract]
[Serializable]
public abstract class EntityAggregateState : IEquatableObject
{
    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    public string CompanyId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 4)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the origin identifier.
    /// </summary>
    /// <value>The origin identifier.</value>
    [DataMember(Order = 3)]
    public string OriginId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    public string PartitionId { get; set; } = string.Empty;

    /// <inheritdoc/>
    public virtual IEnumerable<object?> GetEqualityComponents()
        => [PartitionId, CompanyId, OriginId, Id];
}