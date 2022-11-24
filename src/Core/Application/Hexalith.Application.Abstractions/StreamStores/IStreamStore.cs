// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Event store interface
/// </summary>
public interface IStreamStore
{
	/// <summary>
	/// Gets an event stream. If the stream does not exist, it is created.
	/// </summary>
	/// <param name="streamId">The stream identifier</param>
	/// <returns>The event stream</returns>
	IPersistedStream GetStream(string streamId);
}