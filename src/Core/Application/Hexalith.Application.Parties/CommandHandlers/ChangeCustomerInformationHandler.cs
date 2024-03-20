// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-21-2023
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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Parties.Commands;
using Hexalith.Application.Parties.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;
using Hexalith.Domain.ValueObjets;

using KellermanSoftware.CompareNetObjects;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class ChangeCustomerInformationHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.ChangeCustomerInformation}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.Parties.Commands.ChangeCustomerInformation}" />
public partial class ChangeCustomerInformationHandler : CommandHandler<ChangeCustomerInformation>
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<ChangeCustomerInformationHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeCustomerInformationHandler" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">logger.</exception>
    public ChangeCustomerInformationHandler(ILogger<ChangeCustomerInformationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync([NotNull] ChangeCustomerInformation command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        CustomerInformationChanged changed = new(
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

        if (aggregate is null || aggregate is not Customer customer || !customer.IsInitialized())
        {
            LogFailedToUpdateCustomerError(command.AggregateId);

            // The customer update failed. The customer is not registered. We need to register it.
            return await Task.FromResult<IEnumerable<BaseMessage>>([command.ToCustomer(false).ToCustomerRegistered()]).ConfigureAwait(false);
        }

        return HasChanges(customer.ToCustomerInformationChanged(), changed)
            ? await Task.FromResult<IEnumerable<BaseMessage>>([changed]).ConfigureAwait(false)
            : await Task.FromResult<IEnumerable<BaseMessage>>([]).ConfigureAwait(false);
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Failed to update customer {AggregateId} : Customer not found. Registering as a new customer.")]
    public partial void LogFailedToUpdateCustomerError(string aggregateId);

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> UndoAsync(ChangeCustomerInformation command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        throw new NotSupportedException();
    }

    /// <summary>
    /// Determines whether the specified current has changes.
    /// </summary>
    /// <param name="current">The current.</param>
    /// <param name="changed">The changed.</param>
    /// <returns><c>true</c> if the specified current has changes; otherwise, <c>false</c>.</returns>
    private bool HasChanges(CustomerInformationChanged current, CustomerInformationChanged changed)
    {
        CompareLogic compareLogic = new();

        compareLogic.Config.IgnoreProperty<CustomerInformationChanged>(p => p.Date);

        ComparisonResult result = compareLogic.Compare(current, changed);

        if (result.AreEqual)
        {
            LogNoChangeToApplyInformation(current.AggregateId);
            return false;
        }

        LogChangesToApplyFoundInformation(current.AggregateId, result.DifferencesString);
        return true;
    }

    /// <summary>
    /// Logs the changes to apply found information.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="changes">The changes.</param>
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Changes to apply found for customer '{AggregateId}' :\n{Changes}")]
    private partial void LogChangesToApplyFoundInformation(string aggregateId, string changes);

    /// <summary>
    /// Logs the no change to apply information.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "No change to apply found for customer '{AggregateId}'")]
    private partial void LogNoChangeToApplyInformation(string aggregateId);
}