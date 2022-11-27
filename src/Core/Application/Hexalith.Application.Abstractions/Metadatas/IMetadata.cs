// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Metadatas;

using System;

/// <summary>
/// Interface for all message metadatas.
/// </summary>
public interface IMetadata
{
	/// <summary>
	/// The message context metadata
	/// </summary>
	IContextMetadata Context { get; }

	/// <summary>
	/// The date the message was created
	/// </summary>
	DateTimeOffset Date { get; }

	/// <summary>
	/// The message metadata
	/// </summary>
	IMessageMetadata Message { get; }

	/// <summary>
	/// The message scopes names
	/// </summary>
	IEnumerable<string>? Scopes { get; }

	/// <summary>
	/// The metadata version
	/// </summary>
	IMetadataVersion Version { get; }
}