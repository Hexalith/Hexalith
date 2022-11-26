// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Application.Abstractions.Envelopes;

using System;

/// <summary>
/// Interface for all message metadatas.
/// </summary>
public interface IMetadata
{
	/// <summary>
	/// Aggregate identifier.
	/// </summary>
	string AggregateId { get; }

	/// <summary>
	/// Aggregate name.
	/// </summary>
	string AggregateName { get; }

	/// <summary>
	/// Message correlation id. It's used to group messages that are related to the same business action.
	/// </summary>
	string CorrelationId { get; }

	/// <summary>
	/// The date the message was created
	/// </summary>
	DateTimeOffset Date { get; }

	/// <summary>
	/// The metadata major version
	/// </summary>
	int MajorVersion { get; }

	/// <summary>
	/// The message unique identifier
	/// </summary>
	string MessageId { get; }

	/// <summary>
	/// The message major version
	/// </summary>
	int MessageMajorVersion { get; }

	/// <summary>
	/// The message minor version
	/// </summary>
	int MessageMinorVersion { get; }

	/// <summary>
	/// The message name
	/// </summary>
	string MessageName { get; }

	/// <summary>
	/// The message type name
	/// </summary>
	string MessageTypeName { get; }

	/// <summary>
	/// The metadata minor version
	/// </summary>
	int MinorVersion { get; }

	/// <summary>
	/// The message scopes names
	/// </summary>
	IEnumerable<string>? Scopes { get; }

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