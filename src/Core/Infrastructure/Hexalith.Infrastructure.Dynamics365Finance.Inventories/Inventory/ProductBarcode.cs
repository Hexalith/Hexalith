// <copyright file="ProductBarcode.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.Inventory;

using System.Runtime.Serialization;

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
[DataContract]
public record ProductBarcode(
        string Etag,
        string DataAreaId,
        [property: DataMember(Order = 3)] string ItemNumber,
        [property: DataMember(Order = 4)] string ProductConfigurationId,
        [property: DataMember(Order = 5)] string ProductColorId,
        [property: DataMember(Order = 6)] string ProductSizeId,
        [property: DataMember(Order = 7)] string ProductStyleId,
        [property: DataMember(Order = 8)] string ProductVersionId,
        [property: DataMember(Order = 9)] string ProductQuantityUnitSymbol,
        [property: DataMember(Order = 10)] string BarcodeSetupId,
        [property: DataMember(Order = 11)] string Barcode,
        [property: DataMember(Order = 12)] string ProductDescription,
        [property: DataMember(Order = 13)] string IsDefaultScannedBarcode,
        [property: DataMember(Order = 14)] string IsDefaultDisplayedBarcode,
        [property: DataMember(Order = 15)] string IsDefaultPrintedBarcode,
        [property: DataMember(Order = 16)] decimal ProductQuantity)
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
        ArgumentException.ThrowIfNullOrWhiteSpace(systemId);
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