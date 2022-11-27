// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Aggregate metadata.
/// </summary>
public interface IAggregateMetaData
{
	/// <summary>
	/// The aggregate identifier.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// The aggregate name.
	/// </summary>
	string Name { get; }
}