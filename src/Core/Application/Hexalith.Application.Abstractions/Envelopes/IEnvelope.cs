// <copyright file="IEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;

/// <summary>
/// Interface for all envelopes.
/// </summary>
public interface IEnvelope
{
	/// <summary>
	/// Gets the envelope message.
	/// </summary>
	IMessage Message { get; }

	/// <summary>
	/// Gets the envelope meta data.
	/// </summary>
	IMetadata Metadata { get; }
}

/// <summary>
/// The interface for all typed envelopes.
/// </summary>
/// <typeparam name="TMessage">The message.</typeparam>
/// <typeparam name="TMetadata">The message metadata.</typeparam>
public interface IEnvelope<out TMessage, out TMetadata> : IEnvelope
	where TMessage : IMessage
	where TMetadata : IMetadata
{
	/// <summary>
	/// Gets the message.
	/// </summary>
	new TMessage Message { get; }

	/// <summary>
	/// Gets the message metadata.
	/// </summary>
	new TMetadata Metadata { get; }
}