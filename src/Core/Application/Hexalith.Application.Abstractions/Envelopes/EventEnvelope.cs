// <copyright file="EventEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Envelopes;

using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class EventEnvelope.
/// Implements the <see cref="Hexalith.Application.Abstractions.Envelopes.BaseEnvelope{Hexalith.Domain.Abstractions.Events.BaseEvent, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// Implements the <see cref="IEventEnvelope" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Abstractions.Envelopes.BaseEnvelope{Hexalith.Domain.Abstractions.Events.BaseEvent, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// <seealso cref="IEventEnvelope" />
[DataContract]
public class EventEnvelope : BaseEnvelope<BaseEvent, Metadata>, IEventEnvelope
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventEnvelope" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public EventEnvelope(BaseEvent message, Metadata metadata)
        : base(message, metadata)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventEnvelope"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public EventEnvelope()
    {
    }

    /// <inheritdoc/>
    IEvent IEventEnvelope.Message => Message;
}
