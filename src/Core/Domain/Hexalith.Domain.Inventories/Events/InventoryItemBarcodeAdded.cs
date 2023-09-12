// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="InventoryItemBarcodeAdded.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class InventoryItemAdded.
/// Implements the <see cref="Hexalith.Domain.Events.InventoryItemEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.InventoryItemEvent" />
[DataContract]
public class InventoryItemBarcodeAdded : InventoryItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeAdded"/> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    /// <param name="date">The date.</param>
    public InventoryItemBarcodeAdded(
        string companyId,
        string id,
        ItemBarcode barcode,
        DateTimeOffset date)
        : base(companyId, id)
    {
        Date = date;
        Barcode = barcode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeAdded"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public InventoryItemBarcodeAdded()
    {
        Barcode = new ItemBarcode();
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the barcode.
    /// </summary>
    /// <value>The barcode.</value>
    [DataMember(Order = 10)]
    public ItemBarcode Barcode { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 11)]
    public DateTimeOffset Date { get; set; }
}