// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class WarehouseOnHandV2.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.WarehouseOnHandV2}" />
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IODataElement" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.WarehouseOnHandV2}" />
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