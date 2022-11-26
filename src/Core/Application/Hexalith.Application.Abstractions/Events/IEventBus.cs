// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Events;

using Hexalith.Application.Abstractions.Envelopes;

/// <summary>
/// Interface for all event buses.
/// </summary>
public interface IEventBus : IMessageBus<IEventEnvelope>
{
}