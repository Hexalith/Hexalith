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
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> DoAsync(ChangeCustomerInformation command, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<BaseMessage>>(new CustomerInformationChanged(
                    command.CompanyId,
                    command.Id,
                    command.Name,
                    command.Contact,
                    command.WarehouseId,
                    command.CommissionSalesGroupId,
                    command.Date)
                    .IntoArray<BaseMessage>());
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(ChangeCustomerInformation command, CancellationToken cancellationToken) => throw new NotSupportedException();
}