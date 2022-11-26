// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Commands;

using Hexalith.Application.Abstractions.Envelopes;

/// <summary>
/// Interface for all command buses.
/// </summary>
public interface ICommandBus : IMessageBus<ICommandEnvelope>
{
}