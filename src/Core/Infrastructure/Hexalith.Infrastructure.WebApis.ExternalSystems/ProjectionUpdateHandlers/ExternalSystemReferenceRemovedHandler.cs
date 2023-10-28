// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="ExternalSystemReferenceRemovedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.ExternalSystems.IntegrationEvents;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;

/// <summary>
/// Class ExternalSystemReferenceRemovedHandler.
/// Implements the <see cref="IntegrationEventHandler`1" />.
/// </summary>
/// <seealso cref="IntegrationEventHandler`1" />
public class ExternalSystemReferenceRemovedHandler : IntegrationEventHandler<ExternalSystemReferenceRemoved>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseCommand>> ApplyAsync(ExternalSystemReferenceRemoved @event, CancellationToken cancellationToken)
        => Task.FromResult<IEnumerable<BaseCommand>>([]);
}