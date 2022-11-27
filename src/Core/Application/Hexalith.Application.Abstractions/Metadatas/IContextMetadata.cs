// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The message context metadata
/// </summary>
public interface IContextMetadata
{
	/// <summary>
	/// Message correlation id. It's used to group messages that are related to the same business action.
	/// </summary>
	string CorrelationId { get; }

	/// <summary>
	/// The message sequence number
	/// </summary>
	public long SequenceNumber { get; }

	/// <summary>
	/// Session identifier.
	/// </summary>
	string? SessionId { get; }

	/// <summary>
	/// The user identifier
	/// </summary>
	string UserId { get; }
}