// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnitConversionDetailsProjection.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryUnitConversions.Projections;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class UnitConversionDetailsProjection.
/// </summary>
[DataContract]
[Serializable]
public class InventoryUnitConversionDetailsProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitConversionDetailsProjection" /> class.
    /// </summary>
    public InventoryUnitConversionDetailsProjection() => Id = ToUnitId = InventoryItemId = string.Empty;

    /// <summary>
    /// Gets or sets the factor.
    /// </summary>
    /// <value>The factor.</value>
    [DataMember(Order = 4)]
    public decimal Factor { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 3)]
    public string? InventoryItemId { get; set; }

    /// <summary>
    /// Gets or sets the round decimals.
    /// </summary>
    /// <value>The round decimals.</value>
    [DataMember(Order = 5)]
    public int RoundDecimals { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 2)]
    public string ToUnitId { get; set; }
}