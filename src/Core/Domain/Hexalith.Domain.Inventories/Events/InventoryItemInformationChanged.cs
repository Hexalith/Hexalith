// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="InventoryItemInformationChanged.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemInformationChanged.
/// Implements the <see cref="Hexalith.Domain.Events.InventoryItemEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.InventoryItemEvent" />
[DataContract]
[Serializable]
public class InventoryItemInformationChanged : InventoryItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemInformationChanged"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="date">The date.</param>
    public InventoryItemInformationChanged(string partitionId, string companyId, string originId, string id, string name, DateTimeOffset date)
        : base(partitionId, companyId, originId, id)
    {
        Name = name;
        Date = date;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemInformationChanged" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public InventoryItemInformationChanged()
    {
        Name = string.Empty;
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the external ids.
    /// </summary>
    /// <value>The external ids.</value>
    [DataMember(Order = 11)]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 10)]
    public string Name { get; set; }
}