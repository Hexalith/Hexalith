// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="ChangeCustomerInformationHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Services;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ChangeCustomerInformationHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.ChangeCustomerInformation}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.ChangeCustomerInformation}" />
public class ChangeCustomerInformationHandler : CommandHandler<ChangeCustomerInformation>
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly ICustomerQueryService _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeCustomerInformationHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    public ChangeCustomerInformationHandler(ICustomerQueryService customerService)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        _customerService = customerService;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync(ChangeCustomerInformation command, CancellationToken cancellationToken)
    {
        return await _customerService.ExistAsync(command.AggregateId, cancellationToken)
            ? await _customerService.HasChangesAsync(command, cancellationToken)
                ? new CustomerInformationChanged(
                    command.Id,
                    command.Name,
                    command.CompanyId,
                    command.Contact,
                    command.InvoiceAddress,
                    command.DeliveryAddress,
                    command.WarehouseId,
                    command.Date)
                    .IntoArray()
                : Array.Empty<BaseMessage>()
            : new CustomerRegistered(
                command.Id,
                command.Name,
                command.CompanyId,
                command.Contact,
                command.InvoiceAddress,
                command.DeliveryAddress,
                command.WarehouseId,
                command.Date)
                .IntoArray();
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(ChangeCustomerInformation command, CancellationToken cancellationToken) => throw new NotSupportedException();
}