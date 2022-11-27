// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The message version
/// </summary>
public interface IMessageVersion
{
	/// <summary>
	/// The major version.
	/// </summary>
	int Major { get; }

	/// <summary>
	/// The minor version.
	/// </summary>
	int Minor { get; }
}