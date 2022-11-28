// <copyright file="IMessageBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

/// <summary>
/// Interface for all message buses.
/// </summary>
public interface IMessageBus<in TEnvelope>
	where TEnvelope : IEnvelope
{
	/// <summary>
	/// Publish a message.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="envelope">The envelope to send.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task PublishAsync<T>(T envelope, CancellationToken cancellationToken)
		where T : TEnvelope;
}