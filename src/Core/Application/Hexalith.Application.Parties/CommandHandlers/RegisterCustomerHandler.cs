// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="RegisterCustomerHandler.cs" company="Fiveforty SAS Paris France">
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
/// Class RegisterCustomerHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.RegisterCustomer}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.RegisterCustomer}" />
public class RegisterCustomerHandler : CommandHandler<RegisterCustomer>
{
    /// <summary>
    /// The customer service.
    /// </summary>
    private readonly ICustomerQueryService _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCustomerHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    public RegisterCustomerHandler(ICustomerQueryService customerService)
    {
        ArgumentNullException.ThrowIfNull(customerService);
        _customerService = customerService;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync(RegisterCustomer command, CancellationToken cancellationToken)
    {
        return await _customerService.ExistAsync(command.AggregateId, cancellationToken)
            ? new CustomerInformationChanged(
                command.Id,
                command.Name,
                command.CompanyId,
                command.Contact,
                command.InvoiceAddress,
                command.DeliveryAddress,
                command.WarehouseId,
                command.CommissionSalesGroupId,
                command.Date)
                .IntoArray()
            : new CustomerRegistered(
                command.Id,
                command.Name,
                command.CompanyId,
                command.Contact,
                command.InvoiceAddress,
                command.DeliveryAddress,
                command.WarehouseId,
                command.CommissionSalesGroupId,
                command.Date)
                .IntoArray();
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(RegisterCustomer command, CancellationToken cancellationToken) => throw new NotSupportedException();
}