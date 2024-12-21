// <copyright file="IChunkableRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Interface for chunkable requests.
/// </summary>
public interface IChunkableRequest : ICollectionRequest
{
    /// <summary>
    /// Gets a value indicating whether this instance has results.
    /// </summary>
    bool HasNextChunk => Take > 0 && HasResults;

    /// <summary>
    /// Gets a value indicating whether this instance has results.
    /// </summary>
    bool HasResults => Results is not null && Results.Any();

    /// <summary>
    /// Gets the number of items to skip.
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// Gets the number of items to take.
    /// </summary>
    int Take { get; }

    /// <summary>
    /// Creates the next chunk request. Skip is incremented by Take.
    /// </summary>
    /// <returns>The next chunk request.</returns>
    IChunkableRequest CreateNextChunkRequest();
}