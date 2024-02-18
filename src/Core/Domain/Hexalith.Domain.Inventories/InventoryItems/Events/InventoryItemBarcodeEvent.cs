// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemBarcodeEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryItems.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.InventoryItem.Events;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryUnitConversionEvent.
/// Implements the <see cref="Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Domain.Events.CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class InventoryItemBarcodeEvent : InventoryItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    [JsonConstructor]
    protected InventoryItemBarcodeEvent(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string barcode)
        : base(partitionId, companyId, originId, id) => Barcode = barcode;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryItemBarcodeEvent() => Barcode = string.Empty;

    /// <summary>
    /// Gets the barcode.
    /// </summary>
    /// <value>The barcode.</value>
    [DataMember(Order = 20)]
    public string Barcode { get; }
}