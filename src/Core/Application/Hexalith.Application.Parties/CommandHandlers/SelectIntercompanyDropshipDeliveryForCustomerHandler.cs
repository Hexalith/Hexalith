// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="SelectIntercompanyDropshipDeliveryForCustomerHandler.cs" company="Fiveforty SAS Paris France">
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
public class SelectIntercompanyDropshipDeliveryForCustomerHandler : CommandHandler<SelectIntercompanyDropshipDeliveryForCustomer>
{
    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync([NotNull] SelectIntercompanyDropshipDeliveryForCustomer command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        IntercompanyDropshipDeliveryForCustomerSelected selected = new(
             command.PartitionId,
             command.CompanyId,
             command.OriginId,
             command.Id);
        if (aggregate is not null)
        {
            Customer customer = (Customer)aggregate;
            if (customer.IntercompanyDropship)
            {
                return await Task.FromResult<IEnumerable<BaseMessage>>([]).ConfigureAwait(false);
            }
        }

        return await Task.FromResult<IEnumerable<BaseMessage>>([selected]).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(SelectIntercompanyDropshipDeliveryForCustomer command, IAggregate? aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();
}