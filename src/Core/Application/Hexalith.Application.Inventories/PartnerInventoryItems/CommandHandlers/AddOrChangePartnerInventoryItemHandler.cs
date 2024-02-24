// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="AddOrChangePartnerInventoryItemHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.PartnerInventoryItems.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Inventories.PartnerInventoryItems.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;
using Hexalith.Domain.PartnerInventoryItems.Events;

/// <summary>
/// Class ChangeCustomerInformationHandler.
/// Implements the <see cref="CommandHandler{Parties.Commands.ChangeCustomerInformation}" />.
/// </summary>
/// <seealso cref="CommandHandler{Parties.Commands.ChangeCustomerInformation}" />
public class AddOrChangePartnerInventoryItemHandler : CommandHandler<AddOrChangePartnerInventoryItem>
{
    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync([NotNull] AddOrChangePartnerInventoryItem command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return aggregate == null || !aggregate.IsInitialized()
            ? await Task.FromResult<IEnumerable<BaseMessage>>([new PartnerInventoryItemAdded(
                        command.PartitionId,
                        command.CompanyId,
                        command.OriginId,
                        command.PartnerType,
                        command.PartnerId,
                        command.Id,
                        command.InventoryItemId,
                        command.UnitId,
                        command.Name,
                        command.Price,
                        command.CountryOfOriginId,
                        command.HarmonizedTariffScheduleCode,
                        command.ProductType)
                        ]).ConfigureAwait(false)
            : await Task.FromResult<IEnumerable<BaseMessage>>([new PartnerInventoryItemChanged(
                    command.PartitionId,
                    command.CompanyId,
                    command.OriginId,
                    command.PartnerType,
                    command.PartnerId,
                    command.Id,
                    command.InventoryItemId,
                    command.UnitId,
                    command.Name,
                    command.Price,
                    command.CountryOfOriginId,
                    command.HarmonizedTariffScheduleCode,
                    command.ProductType)
                    ]).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> UndoAsync(AddOrChangePartnerInventoryItem command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        throw new NotSupportedException();
    }
}