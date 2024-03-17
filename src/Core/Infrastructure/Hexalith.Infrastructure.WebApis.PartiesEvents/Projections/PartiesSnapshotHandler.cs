// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="PartiesSnapshotHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class IntercompanyDropshipDeliveryForCustomerDeselectedHandler.
/// Implements the <see cref="Application.Events.IntegrationEventProjectionUpdateHandler{CustomerRegistered}" />.
/// </summary>
/// <seealso cref="Application.Events.IntegrationEventProjectionUpdateHandler{CustomerRegistered}" />
/// <remarks>
/// Initializes a new instance of the <see cref="PartiesSnapshotHandler"/> class.
/// </remarks>
/// <param name="stateStoreProvider">The state store provider.</param>
public partial class PartiesSnapshotHandler(
    IActorProjectionFactory<Customer> customerFactory,
    ILogger<PartiesSnapshotHandler> logger) : IProjectionUpdateHandler<SnapshotEvent>
{
    /// <inheritdoc/>
    public async Task ApplyAsync(SnapshotEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        if (baseEvent is null || baseEvent.AggregateName != Customer.GetAggregateName() || string.IsNullOrWhiteSpace(baseEvent.SourceAggregateId))
        {
            LogProjectionEventIgnoredWarning(logger, baseEvent?.TypeName, baseEvent?.AggregateName, baseEvent?.AggregateId);
            return;
        }

        Customer customer = baseEvent.GetAggregate<Customer>();
        await customerFactory.SetStateAsync(baseEvent.AggregateId, customer, cancellationToken).ConfigureAwait(false);
        LogProjectionInitializedWithSnapshotInformation(logger, baseEvent.AggregateName, baseEvent.AggregateId);
    }

    [LoggerMessage(
        EventId = 2,
    Level = LogLevel.Warning,
    Message = "Parties snapshot event ignored. EventType='{TypeName}'; AggregateName='{AggregateName}'; AggregateId='{AggregateId}'.")]
    private static partial void LogProjectionEventIgnoredWarning(ILogger logger, string? typeName, string? aggregateName, string? aggregateId);

    [LoggerMessage(
        EventId = 1,
    Level = LogLevel.Information,
    Message = "{AggregateName} with id '{AggregateId}' initialized with a snapshot.")]
    private static partial void LogProjectionInitializedWithSnapshotInformation(ILogger logger, string aggregateName, string aggregateId);
}