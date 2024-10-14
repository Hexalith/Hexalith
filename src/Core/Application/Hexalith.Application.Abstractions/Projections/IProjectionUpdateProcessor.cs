// <copyright file="IProjectionUpdateProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Interface IProjectionUpdateProcessor.
/// </summary>
public interface IProjectionUpdateProcessor
{
    /// <summary>
    /// Applies the event asynchronously.
    /// </summary>
    /// <param name="ev">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ApplyAsync(object ev, Metadata metadata, CancellationToken cancellationToken);
}