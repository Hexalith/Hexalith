// <copyright file="IEnvelope.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Envelopes;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;

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
public interface IEnvelope<out TMessage, out TMetadata> : IEnvelope<TMessage>
    where TMessage : IMessage
    where TMetadata : IMetadata
{
    /// <summary>
    /// Gets the message metadata.
    /// </summary>
    new TMetadata Metadata { get; }
}

/// <summary>
/// The interface for all typed envelopes.
/// </summary>
/// <typeparam name="TMessage">The message.</typeparam>
public interface IEnvelope<out TMessage> : IEnvelope
    where TMessage : IMessage
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    new TMessage Message { get; }
}