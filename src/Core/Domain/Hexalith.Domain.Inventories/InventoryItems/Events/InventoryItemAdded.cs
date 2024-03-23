// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="InventoryItemAdded.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryItems.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjects;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemAdded.
/// Implements the <see cref="InventoryItemEvent" />.
/// </summary>
/// <seealso cref="InventoryItemEvent" />
[DataContract]
[Serializable]
public class InventoryItemAdded : InventoryItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemAdded"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="dimensions">The dimensions.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    public InventoryItemAdded(
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
    /// Initializes a new instance of the <see cref="InventoryItemAdded" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public InventoryItemAdded() => Name = Description = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 22)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the dimensions.
    /// </summary>
    /// <value>The dimensions.</value>
    [DataMember(Order = 21)]
    public IEnumerable<DimensionValue>? Dimensions { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 20)]
    public string Name { get; set; }
}