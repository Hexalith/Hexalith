// <copyright file="IPersistedStream.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persisted stream interface.
/// </summary>
public interface IPersistedStream
{
    /// <summary>
    /// Add new items to the stream.
    /// </summary>
    /// <param name="items">The items to add.</param>
    /// <returns>The new stream version.</returns>
    public long AddItems(IEnumerable<IDataFragment> items);

    /// <summary>
    /// Add new items to the stream and verify the version.
    /// </summary>
    /// <param name="items">The items to add.</param>
    /// <param name="expectedVersion">The expected stream version.</param>
    /// <returns>The new stream version.</returns>
    public long AddItems(IEnumerable<IDataFragment> items, long expectedVersion);

    /// <summary>
    /// Gets a stream items slice.
    /// </summary>
    /// <param name="first">first item to retrieve.</param>
    /// <param name="last">last item to retrieve.</param>
    /// <returns>The stream data slice.</returns>
    IEnumerable<IStreamItem> GetItems(long first, long last);

    /// <summary>
    /// Gets all stream items.
    /// </summary>
    /// <returns>The list of all items.</returns>
    IEnumerable<IStreamItem> GetItems();
}