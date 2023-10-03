// <copyright file="ProductBarcode.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.Inventory;

using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class ProductBarcode.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{ProductBarcode}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{ProductBarcode}" />
public record ProductBarcode(
        string Etag,
        string DataAreaId,
        string ItemNumber,
        string ProductConfigurationId,
        string ProductColorId,
        string ProductSizeId,
        string ProductStyleId,
        string ProductVersionId,
        string ProductQuantityUnitSymbol,
        string BarcodeSetupId,
        string Barcode,
        string ProductDescription,
        string IsDefaultScannedBarcode,
        string IsDefaultDisplayedBarcode,
        string IsDefaultPrintedBarcode,
        decimal ProductQuantity)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "ProductBarcodesV3";

    /// <summary>
    /// Converts to productidentifier.
    /// </summary>
    /// <param name="systemId">The system identifier.</param>
    /// <returns>ProductIdentifier.</returns>
    public ProductIdentifier ToProductIdentifier(string systemId)
    {
        ArgumentException.ThrowIfNullOrEmpty(systemId);
        return new ProductIdentifier
        {
            Barcode = Barcode,
            ColorId = ProductColorId,
            SizeId = ProductSizeId,
            StyleId = ProductStyleId,
            ConfigurationId = ProductConfigurationId,
            ItemId = ItemNumber,
            UnitId = ProductQuantityUnitSymbol,
            VersionId = ProductVersionId,
            Quantity = ProductQuantity,
            SystemId = systemId,
        };
    }
}