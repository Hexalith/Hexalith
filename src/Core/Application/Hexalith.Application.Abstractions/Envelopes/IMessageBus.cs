// <copyright file="IMessageBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

/// <summary>
/// A message bus is a component that allows to send messages.
/// </summary>
/// <typeparam name="TEnvelope">The message type.</typeparam>
public interface IMessageBus<in TEnvelope>
	where TEnvelope : IEnvelope
{
	/// <summary>
	/// Publish a message.
	/// </summary>
	/// <param name="envelope">The envelope to send.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task PublishAsync(TEnvelope envelope, CancellationToken cancellationToken);
}