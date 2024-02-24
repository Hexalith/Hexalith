// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceRemovedMapperUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

/// <summary>
/// Class ExternalSystemReferenceRemovedMapperUpdateHandler.
/// Implements the <see cref="Application.Projections.IProjectionUpdateHandler{Hexalith.Domain.Events.ExternalSystemReferenceRemoved}" />.
/// </summary>
/// <seealso cref="Application.Projections.IProjectionUpdateHandler{Hexalith.Domain.Events.ExternalSystemReferenceRemoved}" />
public class ExternalSystemReferenceRemovedMapperUpdateHandler : IProjectionUpdateHandler<ExternalSystemReferenceRemoved>
{
    /// <summary>
    /// The aggregate names.
    /// </summary>
    private readonly IEnumerable<string> _aggregateNames;

    /// <summary>
    /// The application name.
    /// </summary>
    private readonly string _applicationName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceRemovedMapperUpdateHandler" /> class.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateNames">The aggregate names.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public ExternalSystemReferenceRemovedMapperUpdateHandler([NotNull] string applicationName, [NotNull] IEnumerable<string> aggregateNames)
    {
        ArgumentNullException.ThrowIfNull(applicationName);
        ArgumentNullException.ThrowIfNull(aggregateNames);
        _applicationName = applicationName;
        _aggregateNames = aggregateNames;
    }

    /// <inheritdoc/>
    public async Task ApplyAsync(ExternalSystemReferenceRemoved baseEvent, [NotNull] IMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        if (!_aggregateNames.Contains(baseEvent.ReferenceAggregateName, StringComparer.InvariantCultureIgnoreCase))
        {
            return;
        }

        ExternalReferenceMapperUpdateService mapper = new(_applicationName, baseEvent.ReferenceAggregateName);
        await mapper.SetExternalIdAsync(
                baseEvent.ReferenceAggregateId,
                baseEvent.SystemId,
                null)
            .ConfigureAwait(false);
        await mapper.SetAggregateIdAsync(
                baseEvent.PartitionId,
                baseEvent.CompanyId,
                baseEvent.SystemId,
                baseEvent.ExternalId,
                null)
            .ConfigureAwait(false);
    }
}