// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application.Abstractions.Envelopes;

/// <summary>
/// Interface for all message buses.
/// </summary>
public interface IMessageBus<in TEnvelope> where TEnvelope : IEnvelope
{
	/// <summary>
	/// Publish a message
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="envelope">The envelope to send</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task PublishAsync<T>(T envelope, CancellationToken cancellationToken) where T : TEnvelope;
}