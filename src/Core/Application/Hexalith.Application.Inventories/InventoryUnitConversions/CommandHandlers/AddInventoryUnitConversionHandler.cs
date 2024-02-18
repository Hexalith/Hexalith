// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="AddInventoryUnitConversionHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryUnitConversions.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Inventories.InventoryUnitConversions.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.InventoryUnitConversions.Events;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ChangeCustomerInformationHandler.
/// Implements the <see cref="CommandHandler{Parties.Commands.ChangeCustomerInformation}" />.
/// </summary>
/// <seealso cref="CommandHandler{Parties.Commands.ChangeCustomerInformation}" />
public class AddInventoryUnitConversionHandler : CommandHandler<AddInventoryUnitConversion>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> DoAsync([NotNull] AddInventoryUnitConversion command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return Task.FromResult<IEnumerable<BaseMessage>>(new InventoryUnitConversionAdded(
                    command.PartitionId,
                    command.CompanyId,
                    command.OriginId,
                    command.Id,
                    command.ToUnitId,
                    command.InventoryItemId,
                    command.Factor,
                    command.RoundDecimals)
                    .IntoArray<BaseMessage>());
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(AddInventoryUnitConversion command, IAggregate? aggregate, CancellationToken cancellationToken)
        => throw new NotSupportedException();
}