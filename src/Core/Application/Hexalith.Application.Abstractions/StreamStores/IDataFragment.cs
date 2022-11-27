// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persisted stream item interface
/// </summary>
public interface IDataFragment
{
	/// <summary>
	/// Gets the data object
	/// </summary>
	public object Data { get; }

	/// <summary>
	/// Gets the meta data object
	/// </summary>
	public object Metadata { get; }
}

/// <summary>
/// Persisted stream item interface
/// </summary>
public interface IDataFragment<out TData, out TMeta> : IDataFragment
{
	/// <summary>
	/// Gets the data object
	/// </summary>
	public new TData Data { get; }

	/// <summary>
	/// Gets the meta data object
	/// </summary>
	public new TMeta Metadata { get; }
}