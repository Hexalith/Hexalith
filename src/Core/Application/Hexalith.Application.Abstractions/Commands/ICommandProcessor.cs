// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="ICommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
    [Obsolete("Use SubmitAsync with object type commands instead")]
    Task SubmitAsync(BaseCommand command, BaseMetadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Submits the asynchronous.
    /// </summary>
    /// <param name="commands">The command.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    [Obsolete("Use SubmitAsync with object type commands instead")]
    Task SubmitAsync(IEnumerable<BaseCommand> commands, BaseMetadata metadata, CancellationToken cancellationToken);
}