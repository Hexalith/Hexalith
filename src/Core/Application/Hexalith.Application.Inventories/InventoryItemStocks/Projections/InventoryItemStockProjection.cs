// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemStockProjection.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryItemStocks.Projections;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class InventoryItemDetailsProjection.
/// </summary>
[DataContract]
[Serializable]
public class InventoryItemStockProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockProjection"/> class.
    /// </summary>
    public InventoryItemStockProjection()
    {
    }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 1)]
    public decimal Quantity { get; set; }
}