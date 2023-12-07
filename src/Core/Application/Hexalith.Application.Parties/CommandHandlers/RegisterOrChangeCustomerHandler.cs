// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="RegisterOrChangeCustomerHandler.cs" company="Fiveforty SAS Paris France">
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
/// Class RegisterOrChangeCustomerHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.RegisterOrChangeCustomer}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.RegisterOrChangeCustomer}" />
public class RegisterOrChangeCustomerHandler : CommandHandler<RegisterOrChangeCustomer>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> DoAsync([NotNull] RegisterOrChangeCustomer command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (aggregate is not null and Customer customer)
        {
            CustomerInformationChanged changed = new(
                    command.PartitionId,
                    command.CompanyId,
                    command.OriginId,
                    command.Id,
                    command.Name,
                    command.PartyType,
                    command.Contact,
                    command.WarehouseId,
                    command.CommissionSalesGroupId,
                    command.GroupId,
                    command.SalesCurrencyId,
                    command.Date);
            return customer.HasChanges(changed)
                ? Task.FromResult<IEnumerable<BaseMessage>>([changed])
                : Task.FromResult<IEnumerable<BaseMessage>>([]);
        }

        CustomerRegistered registered = new(
            command.PartitionId,
            command.CompanyId,
            command.OriginId,
            command.Id,
            command.Name,
            command.PartyType,
            command.Contact,
            command.WarehouseId,
            command.CommissionSalesGroupId,
            command.GroupId,
            command.SalesCurrencyId,
            command.Date);
        return aggregate is null
            ? Task.FromResult<IEnumerable<BaseMessage>>([registered])
            : Task.FromException<IEnumerable<BaseMessage>>(new InvalidOperationException($"The event {command.TypeName} with id '{command.AggregateId}' can only be applied. Invalid aggregate type '{aggregate.AggregateName}'."));
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(RegisterOrChangeCustomer command, IAggregate? aggregate, CancellationToken cancellationToken) => throw new NotSupportedException();
}