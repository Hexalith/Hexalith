// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Domain.Abstractions.Messages;

/// <summary>
/// Interface for all envelopes.
/// </summary>
public interface IEnvelope
{
	/// <summary>
	/// The envelope message
	/// </summary>
	IMessage Message { get; }

	/// <summary>
	/// The envelope meta data
	/// </summary>
	IMetadata Metadata { get; }
}

/// <summary>
/// The interface for all typed envelopes
/// </summary>
/// <typeparam name="TMessage">The message</typeparam>
/// <typeparam name="TMetadata">The message metadata</typeparam>
public interface IEnvelope<TMessage, TMetadata> : IEnvelope
	where TMessage : IMessage
	where TMetadata : IMetadata
{
	/// <summary>
	/// The message
	/// </summary>
	new TMessage Message { get; }

	/// <summary>
	/// The message metadata
	/// </summary>
	new TMetadata Metadata { get; }
}