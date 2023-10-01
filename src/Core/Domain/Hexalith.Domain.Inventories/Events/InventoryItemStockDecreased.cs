// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="InventoryItemStockDecreased.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

/// <summary>
/// Class InventoryItemAdded.
/// Implements the <see cref="Hexalith.Domain.Events.InventoryItemEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.InventoryItemEvent" />
[DataContract]
public class InventoryItemStockDecreased : InventoryItemStockEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockDecreased" /> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="locationId">The location identifier.</param>
    /// <param name="inventoryItemId">The inventory item identifier.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="date">The date.</param>
    public InventoryItemStockDecreased(
        string companyId,
        string locationId,
        string inventoryItemId,
        decimal quantity,
        DateTimeOffset date)
        : base(companyId, locationId, inventoryItemId)
    {
        Quantity = quantity;
        Date = date;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockDecreased" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public InventoryItemStockDecreased()
    {
        Quantity = 0;
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 11)]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [DataMember(Order = 10)]
    public decimal Quantity { get; set; }
}