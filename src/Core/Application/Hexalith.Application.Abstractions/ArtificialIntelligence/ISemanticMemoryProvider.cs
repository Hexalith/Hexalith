// <copyright file="ISemanticMemoryProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.ArtificialIntelligence;

/// <summary>
/// Interface ISemanticMemoryProvider.
/// </summary>
public interface ISemanticMemoryProvider
{
    /// <summary>
    /// Adds the asynchronous.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddAsync(string content, CancellationToken cancellationToken);

    /// <summary>
    /// Searches the asynchronous.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;System.String&gt;&gt;.</returns>
    Task<IEnumerable<string>> SearchAsync(string search, CancellationToken cancellationToken);
}