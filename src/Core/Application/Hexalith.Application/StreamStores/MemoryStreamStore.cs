// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.StreamStores;

using Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Stream store
/// </summary>
public abstract class MemoryStreamStore : IStreamStore
{
	private readonly Dictionary<string, MemoryPersistedStream> _streams = new();

	/// <summary>
	/// Gets an event stream. If the stream does not exist, it is created.
	/// </summary>
	/// <param name="streamId">The stream identifier</param>
	/// <returns>The event stream</returns>
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