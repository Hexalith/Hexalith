// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;

public class Envelope<TMessage, TMetadata> : IEnvelope<TMessage, TMetadata>
	where TMessage : IMessage
	where TMetadata : IMetadata
{
	public Envelope(TMessage message, TMetadata metadata)
	{
		Message = message;
		Metadata = metadata;
	}

	public TMessage Message { get; }
	public TMetadata Metadata { get; }
	IMessage IEnvelope.Message => Message;
	IMetadata IEnvelope.Metadata => Metadata;
}