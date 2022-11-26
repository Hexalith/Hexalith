// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Commands;

using Hexalith.Application.Abstractions.Envelopes;

/// <summary>
/// Interface for all command envelopes.
/// </summary>
public interface ICommandEnvelope : IEnvelope
{
	new ICommand Message { get; }
}

/// <summary>
/// Interface for all command envelopes.
/// </summary>
public interface ICommandEnvelope<TCommand, TMetadata> : IEnvelope<TCommand, TMetadata>, ICommandEnvelope
	where TCommand : ICommand
	where TMetadata : IMetadata
{
}