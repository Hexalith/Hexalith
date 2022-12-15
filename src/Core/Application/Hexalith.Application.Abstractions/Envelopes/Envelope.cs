// <copyright file="Envelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;

public class Envelope<TMessage, TMetadata> : IEnvelope<TMessage, TMetadata>
    where TMessage : IMessage
    where TMetadata : IMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Envelope{TMessage, TMetadata}"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="metadata"></param>
    public Envelope(TMessage message, TMetadata metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <inheritdoc/>
    public TMessage Message { get; }

    /// <inheritdoc/>
    public TMetadata Metadata { get; }

    /// <inheritdoc/>
    IMessage IEnvelope.Message => Message;

    /// <inheritdoc/>
    IMetadata IEnvelope.Metadata => Metadata;
}