// <copyright file="IDomainCommandProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Command processor interface.
/// </summary>
public interface IDomainCommandProcessor
{
    /// <summary>
    /// Submits commands.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="metadata">The command metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SubmitAsync(object command, Metadata metadata, CancellationToken cancellationToken);
}