// <copyright file="ICommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Command processor interface.
/// </summary>
public interface ICommandProcessor
{
    /// <summary>
    /// Submit to the command processor.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="metadata">The command metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SubmitAsync(BaseCommand command, Metadata metadata, CancellationToken cancellationToken);
}