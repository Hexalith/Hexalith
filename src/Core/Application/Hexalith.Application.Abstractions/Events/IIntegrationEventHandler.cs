// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-27-2023
// ***********************************************************************
// <copyright file="IIntegrationEventHandler.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Domain.Events;

/// <summary>
/// Event handler interface.
/// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

public interface IIntegrationEventHandler
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="baseEvent">The event to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseCommand>> ApplyAsync(IEvent baseEvent, CancellationToken cancellationToken);
}