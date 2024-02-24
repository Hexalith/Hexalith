// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;
using Hexalith.Extensions.Common;

/// <summary>
/// Class SalesLineItem.
/// </summary>
[DataContract]
[Serializable]
public class SalesLineItem : IEquatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineItem" /> class.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="unitId">The unit identifier.</param>
    /// <param name="price">The price.</param>
    [JsonConstructor]
    public SalesLineItem(string itemId, decimal quantity, string unitId, decimal price)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        ItemId = itemId;
        Quantity = quantity;
        UnitId = unitId;
        Price = price;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineItem"/> class.
    /// </summary>
    /// <param name="lineItem">The line item.</param>
    public SalesLineItem(SalesLineItem lineItem)
        : this(
              (lineItem ?? throw new ArgumentNullException(nameof(lineItem))).ItemId,
              lineItem.Quantity,
              lineItem.UnitId,
              lineItem.Price)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineItem" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    [ExcludeFromCodeCoverage]
    public SalesLineItem() => ItemId = UnitId = string.Empty;

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    /// <value>The item identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string ItemId
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public decimal Price
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public decimal Quantity
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the unit identifier.
    /// </summary>
    /// <value>The unit identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string UnitId
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <inheritdoc/>
    public IEnumerable<object?> GetEqualityComponents()
        => [ItemId, Quantity, UnitId, Price];
}