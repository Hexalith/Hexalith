// <copyright file="MemoryStreamStore.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

/// <summary>
/// Stream store.
/// </summary>
public abstract class MemoryStreamStore : IStreamStore
{
    private readonly Dictionary<string, MemoryPersistedStream> _streams = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets an event stream. If the stream does not exist, it is created.
    /// </summary>
    /// <param name="streamId">The stream identifier.</param>
    /// <returns>The event stream.</returns>
    public IPersistedStream GetStream(string streamId)
    {
        if (!_streams.TryGetValue(streamId, out MemoryPersistedStream? stream))
        {
            stream = new MemoryPersistedStream();
            _streams.Add(streamId, stream);
        }

        return stream;
    }
}