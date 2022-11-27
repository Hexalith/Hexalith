// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The message metadata
/// </summary>
public interface IMessageMetadata
{
	/// <summary>
	/// Aggregate metadata.
	/// </summary>
	IAggregateMetaData Aggregate { get; }

	/// <summary>
	/// The aggregate identifier.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// The aggregate name.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The message version
	/// </summary>
	IMessageVersion Version { get; }
}