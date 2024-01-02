// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesLineItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.ValueObjets;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class SalesLineItem.
/// </summary>
[DataContract]
[Serializable]
public class SalesLineItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineItem"/> class.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="unitId">The unit identifier.</param>
    /// <param name="price">The price.</param>
    /// <param name="currencyId">The currency identifier.</param>
    [JsonConstructor]
    public SalesLineItem(string itemId, decimal quantity, string unitId, decimal price)
    {
        ItemId = itemId;
        Quantity = quantity;
        UnitId = unitId;
        Price = price;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineItem"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SalesLineItem() => ItemId = UnitId = string.Empty;

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    /// <value>The item identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string ItemId { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit identifier.
    /// </summary>
    /// <value>The unit identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string UnitId { get; set; }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(SalesLineItem? a, SalesLineItem? b)
    {
        return a is null
            ? b is null
            : a == b ||
                (a.ItemId == b?.ItemId &&
                a.Price == b?.Price &&
                a.Quantity == b?.Quantity &&
                a.UnitId == b?.UnitId);
    }
}