// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnitDetailsProjection.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryUnits.Projections;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class UnitDetailsProjection.
/// </summary>
[DataContract]
[Serializable]
public class InventoryUnitDetailsProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitDetailsProjection"/> class.
    /// </summary>
    public InventoryUnitDetailsProjection() => Id = Name = Description = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the round decimals.
    /// </summary>
    /// <value>The round decimals.</value>
    public int RoundDecimals { get; set; }
}