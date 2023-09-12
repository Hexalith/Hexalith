// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="ChangeInventoryItemInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.Commands;

using System.Runtime.Serialization;

/// <summary>
/// Class InventoryItemInformationChanged.
/// Implements the <see cref="InventoryItemCommand" />.
/// </summary>
/// <seealso cref="InventoryItemCommand" />
[DataContract]
public class ChangeInventoryItemInformation : InventoryItemCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeInventoryItemInformation"/> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="date">The date.</param>
    public ChangeInventoryItemInformation(
        string companyId,
        string id,
        string name,
        DateTimeOffset date)
        : base(companyId, id)
    {
        Name = name;
        Date = date;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeInventoryItemInformation" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public ChangeInventoryItemInformation()
    {
        Name = string.Empty;
        Date = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    [DataMember(Order = 11)]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 10)]
    public string Name { get; set; }
}