// <copyright file="EventEnvelope.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Envelopes;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;
using Hexalith.Extensions;

/// <summary>
/// Class EventEnvelope.
/// Implements the <see cref="BaseEnvelope{BaseEvent, Metadata}" />
/// Implements the <see cref="IEventEnvelope" />.
/// </summary>
/// <seealso cref="BaseEnvelope{BaseEvent, Metadata}" />
/// <seealso cref="IEventEnvelope" />
[DataContract]
[Serializable]
public class EventEnvelope : BaseEnvelope<BaseEvent, BaseMetadata>, IEventEnvelope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventEnvelope" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public EventEnvelope(BaseEvent message, BaseMetadata metadata)
        : base(message, metadata)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventEnvelope"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public EventEnvelope()
    {
    }

    /// <inheritdoc/>
    IEvent IEventEnvelope.Message => Message;
}