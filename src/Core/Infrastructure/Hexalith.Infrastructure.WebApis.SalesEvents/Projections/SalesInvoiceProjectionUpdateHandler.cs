// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.SalesEvents
// Author           : Jérôme Piquot
// Created          : 11-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-12-2023
// ***********************************************************************
// <copyright file="SalesInvoiceProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.SalesEvents.Projections;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Projections;

/// <summary>
/// Class SalesInvoiceProjectionUpdateHandler.
/// Implements the <see cref="ProjectionUpdateHandler{TSalesInvoiceEvent}" />.
/// </summary>
/// <typeparam name="TSalesInvoiceEvent">The type of the t customer event.</typeparam>
/// <seealso cref="ProjectionUpdateHandler{TSalesInvoiceEvent}" />
/// <remarks>
/// Initializes a new instance of the <see cref="SalesInvoiceProjectionUpdateHandler{TSalesInvoiceEvent}"/> class.
/// </remarks>
/// <param name="factory">The factory.</param>
/// <param name="logger">The logger.</param>
public abstract partial class SalesInvoiceProjectionUpdateHandler<TSalesInvoiceEvent>(IActorProjectionFactory<SalesInvoiceState> factory)
    : KeyValueActorProjectionUpdateEventHandlerBase<TSalesInvoiceEvent, SalesInvoiceState>(factory)
    where TSalesInvoiceEvent : SalesInvoiceEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TSalesInvoiceEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (baseEvent is SalesInvoiceIssued issued)
        {
            await SaveProjectionAsync(baseEvent.AggregateId, new SalesInvoiceState(issued), cancellationToken).ConfigureAwait(false);
        }
    }
}