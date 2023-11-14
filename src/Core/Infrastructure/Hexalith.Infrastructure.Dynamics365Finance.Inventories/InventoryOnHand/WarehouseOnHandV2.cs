// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Inventories
// Author           : Jérôme Piquot
// Created          : 10-24-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-24-2023
// ***********************************************************************
// <copyright file="WarehouseOnHandV2.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.InventoryOnHand;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class WarehouseOnHandV2.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{WarehouseOnHandV2}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{WarehouseOnHandV2}" />
public record WarehouseOnHandV2
(
    string DataAreaId,
    string InventorySiteId,
    string InventoryWarehouseId,
    string ItemNumber,
    string? Etag = null,
    string? ProductColorId = null,
    string? ProductConfigurationId = null,
    string? ProductSizeId = null,
    string? ProductStyleId = null,
    string? ProductVersionId = null,
    decimal? AvailableOnHandQuantity = null,
    decimal? ReservedOnHandQuantity = null,
    decimal? AvailableOrderedQuantity = null,
    string? ProductName = null,
    decimal? ReservedOrderedQuantity = null,
    decimal? OnHandQuantity = null,
    string? AreWarehouseManagementProcessesUsed = null,
    decimal? OrderedQuantity = null,
    decimal? OnOrderQuantity = null,
    decimal? TotalAvailableQuantity = null)

: ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "WarehousesOnHandV2";
}