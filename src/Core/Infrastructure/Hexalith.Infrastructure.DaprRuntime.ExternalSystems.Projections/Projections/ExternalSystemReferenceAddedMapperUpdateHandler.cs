// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceAddedMapperUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projection;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

/// <summary>
/// Class ExternalSystemReferenceAddedMapperUpdateHandler.
/// Implements the <see cref="Hexalith.Application.Projection.IProjectionUpdateHandler{Hexalith.Domain.Events.ExternalSystemReferenceAdded}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Projection.IProjectionUpdateHandler{Hexalith.Domain.Events.ExternalSystemReferenceAdded}" />
public class ExternalSystemReferenceAddedMapperUpdateHandler : IProjectionUpdateHandler<ExternalSystemReferenceAdded>
{
    private readonly string _applicationName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceAddedMapperUpdateHandler"/> class.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    public ExternalSystemReferenceAddedMapperUpdateHandler([NotNull] string applicationName)
    {
        ArgumentNullException.ThrowIfNull(applicationName);
        _applicationName = applicationName;
    }

    /// <summary>
    /// Apply as an asynchronous operation.
    /// </summary>
    /// <param name="baseEvent">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task ApplyAsync(ExternalSystemReferenceAdded baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ExternalReferenceMapperUpdateService mapper = new(_applicationName, baseEvent.ReferenceAggregateName);
        await mapper.SetExternalIdAsync(
                baseEvent.ReferenceAggregateId,
                baseEvent.SystemId,
                baseEvent.ExternalId)
            .ConfigureAwait(false);
        await mapper.SetAggregateIdAsync(
                baseEvent.PartitionId,
                baseEvent.CompanyId,
                baseEvent.SystemId,
                baseEvent.ExternalId,
                baseEvent.AggregateId)
            .ConfigureAwait(false);
    }
}