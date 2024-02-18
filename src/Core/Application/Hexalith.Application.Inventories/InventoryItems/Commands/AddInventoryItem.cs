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

using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemAdded.
/// Implements the <see cref="InventoryItemCommand" />.
/// </summary>
/// <seealso cref="InventoryItemCommand" />
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
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="date">The date.</param>
    public AddInventoryItem(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string name,
        string? description)
        : base(partitionId, companyId, originId, id)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddInventoryItem" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AddInventoryItem() => Name = Description = string.Empty;

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 21)]
    public string? Description { get; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 20)]
    public string Name { get; set; }
}