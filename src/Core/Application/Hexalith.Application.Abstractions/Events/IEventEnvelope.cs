// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Events;

using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Interface for all event envelopes.
/// </summary>
public interface IEventEnvelope : IEnvelope
{
	new IEvent Message { get; }
}

/// <summary>
/// Interface for all event envelopes.
/// </summary>
public interface IEventEnvelope<TEvent, TMetadata> : IEnvelope<TEvent, TMetadata>, IEventEnvelope
	where TEvent : IEvent
	where TMetadata : IMetadata
{
}