// ***********************************************************************
// Assembly         : Hexalith.Application.Sales
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-21-2023
// ***********************************************************************
// <copyright file="RegisterSalesInvoiceHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Sales.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Sales.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class RegisterSalesInvoiceHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Sales.Commands.IssueSalesInvoice}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Sales.Commands.IssueSalesInvoice}" />
public partial class IssueSalesInvoiceHandler : CommandHandler<IssueSalesInvoice>
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<IssueSalesInvoiceHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IssueSalesInvoiceHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public IssueSalesInvoiceHandler(ILogger<IssueSalesInvoiceHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync([NotNull] IssueSalesInvoice command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        SalesInvoiceRegistered registered =
            new(
                command.PartitionId,
                command.CompanyId,
                command.OriginId,
                command.Id,
                command.Name,
                command.PartyType,
                new Contact(command.Contact),
                command.WarehouseId,
                command.CommissionSalesGroupId,
                command.GroupId,
                command.SalesCurrencyId,
                command.Date);
        return await (aggregate is null
            ? Task.FromResult<IEnumerable<BaseMessage>>((IEnumerable<BaseMessage>)([registered]))
            : Task.FromException<IEnumerable<BaseMessage>>(
                new InvalidOperationException(
                    $"The event {command.TypeName} with id '{command.AggregateId}' cannot be applied on an existing customer."))).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(IssueSalesInvoice command, IAggregate? aggregate, CancellationToken cancellationToken) => throw new NotSupportedException();
}