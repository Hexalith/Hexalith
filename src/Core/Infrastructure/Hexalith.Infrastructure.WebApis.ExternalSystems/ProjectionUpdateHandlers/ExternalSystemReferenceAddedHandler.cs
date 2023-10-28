// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceAddedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.ExternalSystems.ProjectionUpdateHandlers;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projection;
using Hexalith.Application.States;
using Hexalith.Domain.Events;

/// <summary>
/// Class ExternalSystemReferenceAddedHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <seealso cref="IntegrationEventHandler`1" />
public class ExternalSystemReferenceAddedHandler : ProjectionUpdateHandler<ExternalSystemReferenceAdded>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceAddedHandler"/> class.
    /// </summary>
    /// <param name="stateStore"></param>
    public ExternalSystemReferenceAddedHandler(IStateStoreProvider stateStore)
        : base(stateStore)
    {
    }

    /// <inheritdoc/>
    public override async Task ApplyAsync(ExternalSystemReferenceAdded baseEvent, IMetadata metadata, CancellationToken cancellationToken)
    {
        var projection = await StateStore.TryGetStateAsync(baseEvent.AggregateId, cancellationToken).ConfigureAwait(false);
    }
}