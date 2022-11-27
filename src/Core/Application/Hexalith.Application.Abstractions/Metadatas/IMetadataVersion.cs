// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The metadata version
/// </summary>
public interface IMetadataVersion
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