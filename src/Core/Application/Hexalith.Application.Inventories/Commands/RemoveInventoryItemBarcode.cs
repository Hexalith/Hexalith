// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="RemoveInventoryItemBarcode.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.Commands;

using System.Runtime.Serialization;

/// <summary>
/// Class InventoryItemAdded.
/// Implements the <see cref="InventoryItemCommand" />.
/// </summary>
/// <seealso cref="InventoryItemCommand" />
[DataContract]
public class RemoveInventoryItemBarcode : InventoryItemCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveInventoryItemBarcode"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="barcode">The barcode.</param>
    /// <param name="date">The date.</param>
    public RemoveInventoryItemBarcode(
        string partitionId,
        string companyId,
        string id,
        string barcode,
        DateTimeOffset date)
        : base(partitionId, companyId, id)
    {
        Date = date;
        Barcode = barcode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveInventoryItemBarcode"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public RemoveInventoryItemBarcode()
    {
        Barcode = string.Empty;
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the contact.
    /// </summary>
    /// <value>The contact.</value>
    [DataMember(Order = 10)]
    public string Barcode { get; set; }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 11)]
    public DateTimeOffset Date { get; set; }
}