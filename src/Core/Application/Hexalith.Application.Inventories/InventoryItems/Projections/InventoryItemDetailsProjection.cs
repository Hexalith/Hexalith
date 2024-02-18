// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemDetailsProjection.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryItems.Projections;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class InventoryItemDetailsProjection.
/// </summary>
[DataContract]
[Serializable]
public class InventoryItemDetailsProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemDetailsProjection"/> class.
    /// </summary>
    public InventoryItemDetailsProjection() => Id = Name = Description = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 3)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 2)]
    public string Name { get; set; }
}