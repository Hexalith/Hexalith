// <copyright file="ICommandHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface ICommandHandler.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="metadata">The command metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<(BaseEvent Event, Metadata Metadata)>> DoAsync(BaseCommand command, Metadata metadata, CancellationToken cancellationToken);
}
