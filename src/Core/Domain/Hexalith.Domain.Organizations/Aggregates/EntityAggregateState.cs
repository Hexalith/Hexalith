// ***********************************************************************
// Assembly         : Hexalith.Domain.Organizations
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesInvoiceState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Organizations.Aggregates;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class EntityAggregateState.
/// </summary>
[DataContract]
[Serializable]
public abstract class EntityAggregateState
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
}