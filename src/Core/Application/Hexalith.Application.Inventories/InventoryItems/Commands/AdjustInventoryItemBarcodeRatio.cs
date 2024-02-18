// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="AdjustInventoryItemBarcodeRatio.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryItems.Commands;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemConversionInformationChanged.
/// Implements the <see cref="InventoryItemBarcodeCommand" />.
/// </summary>
/// <seealso cref="InventoryItemBarcodeCommand" />
[DataContract]
[Serializable]
public class AdjustInventoryItemBarcodeRatio : InventoryItemBarcodeCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdjustInventoryItemBarcodeRatio" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    /// <param name="unitId">The unit identifier.</param>
    /// <param name="quantity">The quantity.</param>
    public AdjustInventoryItemBarcodeRatio(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string barcode,
        string unitId,
        decimal quantity)
        : base(partitionId, companyId, originId, id, barcode)
    {
        UnitId = unitId;
        Quantity = quantity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdjustInventoryItemBarcodeRatio" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AdjustInventoryItemBarcodeRatio() => UnitId = string.Empty;

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