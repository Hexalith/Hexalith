// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="InventoryItemCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Organizations.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemCommand.
/// Implements the <see cref="Domain.Commands.BaseCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.BaseCommand" />
[DataContract]
public abstract class InventoryItemCommand : CompanyEntityCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemCommand"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected InventoryItemCommand(string partitionId, string companyId, string originId, string id)
        : base(partitionId, companyId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryItemCommand()
    {
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryItem.GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryItem.GetAggregateName();
}