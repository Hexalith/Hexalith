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

using System.Runtime.Serialization;

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
    [property: DataMember(Order = 3)] string InventorySiteId,
    [property: DataMember(Order = 4)] string InventoryWarehouseId,
    [property: DataMember(Order = 5)] string ItemNumber,
    string? Etag = null,
    [property: DataMember(Order = 6)] string? ProductColorId = null,
    [property: DataMember(Order = 7)] string? ProductConfigurationId = null,
    [property: DataMember(Order = 8)] string? ProductSizeId = null,
    [property: DataMember(Order = 9)] string? ProductStyleId = null,
    [property: DataMember(Order = 10)] string? ProductVersionId = null,
    [property: DataMember(Order = 11)] decimal? AvailableOnHandQuantity = null,
    [property: DataMember(Order = 12)] decimal? ReservedOnHandQuantity = null,
    [property: DataMember(Order = 13)] decimal? AvailableOrderedQuantity = null,
    [property: DataMember(Order = 14)] string? ProductName = null,
    [property: DataMember(Order = 15)] decimal? ReservedOrderedQuantity = null,
    [property: DataMember(Order = 16)] decimal? OnHandQuantity = null,
    [property: DataMember(Order = 17)] string? AreWarehouseManagementProcessesUsed = null,
    [property: DataMember(Order = 18)] decimal? OrderedQuantity = null,
    [property: DataMember(Order = 19)] decimal? OnOrderQuantity = null,
    [property: DataMember(Order = 20)] decimal? TotalAvailableQuantity = null)

: ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "WarehousesOnHandV2";
}