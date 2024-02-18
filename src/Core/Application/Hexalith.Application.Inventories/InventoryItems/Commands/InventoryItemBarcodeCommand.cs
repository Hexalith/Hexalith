// <copyright file="InventoryItemBarcodeCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Inventories.InventoryItems.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemConversionCommand.
/// Implements the <see cref="Domain.Commands.CompanyEntityCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.CompanyEntityCommand" />
[DataContract]
[Serializable]
public abstract class InventoryItemBarcodeCommand : InventoryItemCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    [JsonConstructor]
    protected InventoryItemBarcodeCommand(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string barcode)
        : base(partitionId, companyId, originId, id) => Barcode = barcode;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcodeCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryItemBarcodeCommand() => Barcode = string.Empty;

    /// <summary>
    /// Gets the barcode.
    /// </summary>
    /// <value>The barcode.</value>
    [DataMember(Order = 20)]
    public string Barcode { get; }
}