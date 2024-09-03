// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-15-2023
// ***********************************************************************
// <copyright file="ByCompanyAggregate.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using System.Runtime.Serialization;

/// <summary>
/// Class Aggregate.
/// Implements the <see cref="IAggregate" />.
/// </summary>
/// <seealso cref="IAggregate" />
[DataContract]
public abstract record ByCompanyAggregate(string PartitionId, [property: DataMember(Order = 2)] string CompanyId)
    : PartitionedAggregate(PartitionId)
{
}