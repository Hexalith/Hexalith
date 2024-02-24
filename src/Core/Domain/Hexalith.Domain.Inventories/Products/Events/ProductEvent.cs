// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="ProductEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Products.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.Products.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class ProductConversionEvent.
/// Implements the <see cref="Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Domain.Events.CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class ProductEvent : CommonEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected ProductEvent(
        string partitionId,
        string originId,
        string id)
        : base(partitionId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected ProductEvent()
    {
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => Product.GetAggregateId(PartitionId, OriginId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => Product.GetAggregateName();
}