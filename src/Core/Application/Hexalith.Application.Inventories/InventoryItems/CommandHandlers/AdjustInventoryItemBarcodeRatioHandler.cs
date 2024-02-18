// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="AssignInventoryItemDefaultBarcodeHandler - Copy.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryItems.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Inventories.InventoryItems.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ChangeCustomerInformationHandler.
/// Implements the <see cref="CommandHandler{Parties.Commands.ChangeCustomerInformation}" />.
/// </summary>
/// <seealso cref="CommandHandler{Parties.Commands.ChangeCustomerInformation}" />
public class AdjustInventoryItemBarcodeRatioHandler : CommandHandler<AdjustInventoryItemBarcodeRatio>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> DoAsync([NotNull] AdjustInventoryItemBarcodeRatio command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return Task.FromResult<IEnumerable<BaseMessage>>(new InventoryItemBarcodeRatioAdjusted(
                    command.PartitionId,
                    command.CompanyId,
                    command.OriginId,
                    command.Id,
                    command.Barcode,
                    command.UnitId,
                    command.Quantity)
                    .IntoArray<BaseMessage>());
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(AdjustInventoryItemBarcodeRatio command, IAggregate? aggregate, CancellationToken cancellationToken)
        => throw new NotSupportedException();
}