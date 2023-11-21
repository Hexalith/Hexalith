// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="DeselectIntercompanyDropshipDeliveryForCustomerHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;

/// <summary>
/// Class SetCustomerIntercompanyDeliveryToIndirectHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.DeselectIntercompanyDropshipDeliveryForCustomer}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.DeselectIntercompanyDropshipDeliveryForCustomer}" />
public class DeselectIntercompanyDropshipDeliveryForCustomerHandler : CommandHandler<DeselectIntercompanyDropshipDeliveryForCustomer>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> DoAsync([NotNull] DeselectIntercompanyDropshipDeliveryForCustomer command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        IntercompanyDropshipDeliveryForCustomerDeselected deselected = new(
            command.PartitionId,
            command.CompanyId,
            command.OriginId,
            command.Id);
        return aggregate is not null and Customer customer
            ? customer.IntercompanyDropship
                ? Task.FromResult<IEnumerable<BaseMessage>>([deselected])
                : Task.FromResult<IEnumerable<BaseMessage>>([])
            : Task.FromException<IEnumerable<BaseMessage>>(new InvalidOperationException($"The event {command.TypeName} with id '{command.AggregateId}' can only be applied on an existing customer."));
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(DeselectIntercompanyDropshipDeliveryForCustomer command, IAggregate? aggregate, CancellationToken cancellationToken) => throw new NotSupportedException();
}