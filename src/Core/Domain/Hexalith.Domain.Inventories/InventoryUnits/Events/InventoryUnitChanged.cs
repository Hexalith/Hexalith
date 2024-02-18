// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnitChanged.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryUnits.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class InventoryUnitConversionInformationChanged.
/// Implements the <see cref="InventoryUnitEvent" />.
/// </summary>
/// <seealso cref="InventoryUnitEvent" />
[DataContract]
[Serializable]
public class InventoryUnitChanged : InventoryUnitEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitChanged" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="roundDecimals">The round decimals.</param>
    public InventoryUnitChanged(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string name,
        string? description,
        int roundDecimals)
        : base(partitionId, companyId, originId, id)
    {
        RoundDecimals = roundDecimals;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitChanged" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public InventoryUnitChanged() => Name = string.Empty;

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember(Order = 21)]
    public string? Description { get; }

    /// <summary>
    /// Gets the factor.
    /// </summary>
    /// <value>The factor.</value>
    [DataMember(Order = 20)]
    public string Name { get; }

    /// <summary>
    /// Gets the round decimals.
    /// </summary>
    /// <value>The round decimals.</value>
    [DataMember(Order = 22)]
    public int RoundDecimals { get; }
}