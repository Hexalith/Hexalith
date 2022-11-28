// <copyright file="MemoryPersistedStream.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// In memory persisted stream.
/// </summary>
public class MemoryPersistedStream : IPersistedStream
{
	private readonly Dictionary<long, IStreamItem> _items = new();

	/// <summary>
	/// Add items to the stream.
	/// </summary>
	/// <param name="items">List of items to add.</param>
	/// <param name="expectedVersion">The stream expected version (Etag) for concurrency checks.</param>
	/// <returns>The stream version.</returns>
	public long AddItems(IEnumerable<IDataFragment> items, long expectedVersion)
	{
		long sequence = _items.Max(p => p.Key);
		if (expectedVersion != sequence)
		{
			throw new UnexpectedStreamVersionException(expectedVersion: expectedVersion, actualVersion: sequence);
		}

		foreach (IDataFragment item in items)
		{
			sequence++;
			_items.Add(sequence, new StreamItem(sequence, item));
		}

		return sequence;
	}

	/// <inheritdoc/>
	public long AddItems(IEnumerable<IDataFragment> items)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Get item slice from stream.
	/// </summary>
	/// <param name="first">Sequence of the top element.</param>
	/// <param name="last">Sequence of the last element.</param>
	/// <returns>The list of items in the slice.</returns>
	public IEnumerable<IStreamItem> GetItems(long first, long last)
	{
		return _items
			.Where(p => p.Key >= first && (last == -1 || p.Key <= last))
			.OrderBy(p => p.Key)
			.Select(p => p.Value);
	}

	/// <summary>
	/// Get all items from stream.
	/// </summary>
	/// <returns>All the items in the stream.</returns>
	/// <exception cref="NotImplementedException"></exception>
	public IEnumerable<IStreamItem> GetItems()
	{
		return GetItems(0, -1);
	}
}