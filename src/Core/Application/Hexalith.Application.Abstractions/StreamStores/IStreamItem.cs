// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persisted stream item iterface
/// </summary>
public interface IStreamItem
{
	/// <summary>
	/// Gets the message
	/// </summary>
	public IDataFragment Message { get; }

	/// <summary>
	/// Gets the stream sequence number
	/// </summary>
	public long Sequence { get; }
}