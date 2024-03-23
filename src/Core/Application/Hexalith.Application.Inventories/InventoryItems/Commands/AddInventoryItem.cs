// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="AddInventoryItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryItems.Commands;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjects;
using Hexalith.Extensions;

/// <summary>
/// Represents a command to add an inventory item.
/// </summary>
[DataContract]
[Serializable]
public class AddInventoryItem : InventoryItemCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddInventoryItem" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="dimensions">The dimensions.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    public AddInventoryItem(
        string partitionId,
        string companyId,
        string originId,
        string id,
        IEnumerable<DimensionValue>? dimensions,
        string name,
        string? description)
        : base(partitionId, companyId, originId, id)
    {
        Dimensions = dimensions;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddInventoryItem" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AddInventoryItem() => Name = Description = string.Empty;

    /// <summary>
    /// Gets or sets the description of the inventory item.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 22)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the dimensions of the inventory item.
    /// </summary>
    /// <value>The dimensions.</value>
    [DataMember(Order = 20)]
    public IEnumerable<DimensionValue>? Dimensions { get; set; }

    /// <summary>
    /// Gets or sets the name of the inventory item.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 21)]
    public string Name { get; set; }
}