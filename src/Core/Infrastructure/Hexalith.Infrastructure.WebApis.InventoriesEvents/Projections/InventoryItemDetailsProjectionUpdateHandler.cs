// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.PartiesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="InventoryDetailsItemProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Projections;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Inventories.InventoryItems.Projections;
using Hexalith.Application.Metadatas;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class InventoryItemProjectionUpdateHandler.
/// Implements the <see cref="ProjectionUpdateHandler{TInventoryItemEvent}" />.
/// </summary>
/// <typeparam name="TInventoryItemEvent">The type of the t customer event.</typeparam>
/// <seealso cref="ProjectionUpdateHandler{TInventoryItemEvent}" />
public abstract partial class InventoryItemDetailsProjectionUpdateHandler<TInventoryItemEvent> : KeyValueActorProjectionUpdateEventHandlerBase<TInventoryItemEvent, InventoryItemDetailsProjection>
    where TInventoryItemEvent : InventoryItemEvent
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemDetailsProjectionUpdateHandler{TInventoryItemEvent}"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="logger">The logger.</param>
    protected InventoryItemDetailsProjectionUpdateHandler(IActorProjectionFactory<InventoryItemDetailsProjection> factory, ILogger logger)
        : base(factory) => _logger = logger;

    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TInventoryItemEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (baseEvent is InventoryItemAdded registered)
        {
            await SaveProjectionAsync(baseEvent.AggregateId, new InventoryItemDetailsProjection(registered), cancellationToken).ConfigureAwait(false);
            return;
        }

        InventoryItemDetailsProjection? existingInventoryItem = await GetProjectionAsync(baseEvent.AggregateId, cancellationToken).ConfigureAwait(false);
        if (existingInventoryItem == null)
        {
            if (baseEvent is InventoryItemDescriptionChanged changed)
            {
                await SaveProjectionAsync(baseEvent.AggregateId, new InventoryItemDetailsProjection(changed), cancellationToken).ConfigureAwait(false);
            }

            InventoryItemProjectionNotInitializedWarning(baseEvent.AggregateId, baseEvent.TypeName);
            return;
        }

        await SaveProjectionAsync(baseEvent.AggregateId, existingInventoryItem.Apply(baseEvent), cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "InventoryItem projection not initialized for aggregate {AggregateId} and event {TypeName}.")]
    private partial void InventoryItemProjectionNotInitializedWarning(string aggregateId, string typeName);
}