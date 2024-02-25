// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-15-2023
// ***********************************************************************
// <copyright file="CommonEntityAggregate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class Aggregate.
/// Implements the <see cref="IAggregate" />.
/// </summary>
/// <seealso cref="IAggregate" />
[DataContract]
[Serializable]
public abstract record CommonEntityAggregate(string PartitionId, string OriginId, string Id)
    : PartitionedAggregate(PartitionId)
{
    /// <summary>
    /// Gets the origin identifier.
    /// </summary>
    /// <value>The origin identifier.</value>
    [DataMember(Order = 2)]
    public string OriginId { get; init; } = OriginId;

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    public string Id { get; init; } = Id;
}