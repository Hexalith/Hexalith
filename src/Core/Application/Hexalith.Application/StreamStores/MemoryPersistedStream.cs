// <copyright file="MemoryPersistedStream.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// In memory persisted stream.
/// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

public class MemoryPersistedStream : IPersistedStream
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    private readonly Dictionary<long, IStreamItem> _items = [];

    /// <inheritdoc/>
    public long AddItems(IEnumerable<IDataFragment> items, long expectedVersion)
    {
        long sequence = _items.Max(p => p.Key);
        return expectedVersion != sequence
            ? throw new UnexpectedStreamVersionException(expectedVersion: expectedVersion, actualVersion: sequence)
            : AddItems(items);
    }

    /// <inheritdoc/>
    public long AddItems([NotNull] IEnumerable<IDataFragment> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        long sequence = _items.Max(p => p.Key);
        foreach (IDataFragment item in items)
        {
            sequence++;
            _items.Add(sequence, new StreamItem(sequence, item));
        }

        return sequence;
    }

    /// <inheritdoc/>
    public IEnumerable<IStreamItem> GetItems(long first, long last)
    {
        return _items
            .Where(p => p.Key >= first && (last == -1 || p.Key <= last))
            .OrderBy(p => p.Key)
            .Select(p => p.Value);
    }

    /// <inheritdoc/>
    public IEnumerable<IStreamItem> GetItems() => GetItems(0, -1);
}