// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persited stream interface
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
	/// Gets the stream items
	/// </summary>
	/// <param name="first">first item to retreive</param>
	/// <param name="last">last item to retreive</param>
	/// <returns>The stream data slice</returns>
	IEnumerable<IStreamItem> GetItems(long first = 0, long last = -1);
}