// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemDefaultBarcodeCleared.cs" company="Fiveforty SAS Paris France">
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
public class InventoryItemDefaultBarcodeCleared : InventoryItemBarcodeEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemDefaultBarcodeCleared" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    public InventoryItemDefaultBarcodeCleared(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string barcode)
        : base(partitionId, companyId, originId, id, barcode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemDefaultBarcodeCleared" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public InventoryItemDefaultBarcodeCleared()
    {
    }
}