// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions;

/// <summary>
/// Persisted stream item iterface
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
/// Persisted stream item iterface
/// </summary>
public interface IDataFragment<TData, TMeta> : IDataFragment
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