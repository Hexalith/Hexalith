// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="PartnerInventoryItemProjection.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.PartnerInventoryItems.Projections;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class PartnerInventoryItemProjection.
/// </summary>
[DataContract]
[Serializable]
public class PartnerInventoryItemProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemProjection" /> class.
    /// </summary>
    public PartnerInventoryItemProjection()
    {
        InventoryItemId = string.Empty;
        UnitId = string.Empty;
    }

    /// <summary>
    /// Gets or sets the inventory item identifier.
    /// </summary>
    /// <value>The inventory item identifier.</value>
    [DataMember(Order = 1)]
    public string InventoryItemId { get; set; }

    /// <summary>
    /// Gets or sets the unit identifier.
    /// </summary>
    /// <value>The unit identifier.</value>
    [DataMember(Order = 2)]
    public string UnitId { get; set; }
}