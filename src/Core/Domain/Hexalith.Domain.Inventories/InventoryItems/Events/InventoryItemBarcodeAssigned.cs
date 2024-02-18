// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemBarcodeAssigned.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryItems.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class InventoryUnitConversionInformationChanged.
/// Implements the <see cref="InventoryItemBarcodeEvent" />.
/// </summary>
/// <seealso cref="InventoryItemBarcodeEvent" />
[DataContract]
[Serializable]
public class InventoryItemBarcodeAssigned : InventoryItemBarcodeEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeAssigned" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    /// <param name="unitId">The unit identifier.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="isDefaultBarcode">if set to <c>true</c> [default barcode].</param>
    public InventoryItemBarcodeAssigned(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string barcode,
        string unitId,
        decimal quantity,
        bool isDefaultBarcode)
        : base(partitionId, companyId, originId, id, barcode)
    {
        UnitId = unitId;
        Quantity = quantity;
        IsDefaultBarcode = isDefaultBarcode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeAssigned" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public InventoryItemBarcodeAssigned() => UnitId = string.Empty;

    /// <summary>
    /// Gets a value indicating whether [default barcode].
    /// </summary>
    /// <value><c>true</c> if [default barcode]; otherwise, <c>false</c>.</value>
    [DataMember(Order = 32)]
    public bool IsDefaultBarcode { get; }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [DataMember(Order = 31)]
    public decimal Quantity { get; }

    /// <summary>
    /// Gets the unit identifier.
    /// </summary>
    /// <value>The unit identifier.</value>
    [DataMember(Order = 30)]
    public string UnitId { get; }
}