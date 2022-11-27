// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persisted stream interface
/// </summary>
public interface IPersistedStream
{
	/// <summary>
	/// Add new items to the stream
	/// </summary>
	/// <param name="items">The items to add</param>
	/// <returns>The new stream version</returns>
	public long AddItems(IEnumerable<IDataFragment> items);

	/// <summary>
	/// Gets a stream items slice
	/// </summary>
	/// <param name="first">first item to retrieve</param>
	/// <param name="last">last item to retrieve</param>
	/// <returns>The stream data slice</returns>
	IEnumerable<IStreamItem> GetItems(long first, long last);

	/// <summary>
	/// Gets all stream items
	/// </summary>
	/// <returns></returns>
	IEnumerable<IStreamItem> GetItems();
}