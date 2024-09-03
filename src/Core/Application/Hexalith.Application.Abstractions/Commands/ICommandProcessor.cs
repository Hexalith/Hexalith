// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="ICommandProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

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
    Task SubmitAsync(BaseCommand command, BaseMetadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Submits the asynchronous.
    /// </summary>
    /// <param name="commands">The command.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SubmitAsync(IEnumerable<BaseCommand> commands, BaseMetadata metadata, CancellationToken cancellationToken);
}