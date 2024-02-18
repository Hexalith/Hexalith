// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnitCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryUnits.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Organizations.Commands;
using Hexalith.Domain.InventoryUnits.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemConversionCommand.
/// Implements the <see cref="Domain.Commands.CompanyEntityCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.CompanyEntityCommand" />
[DataContract]
[Serializable]
public abstract class InventoryUnitCommand : CompanyEntityCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected InventoryUnitCommand(
        string partitionId,
        string companyId,
        string originId,
        string id)
        : base(partitionId, companyId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryUnitCommand()
    {
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryUnit.GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryUnit.GetAggregateName();
}