// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-15-2023
// ***********************************************************************
// <copyright file="IEventEnvelope.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;

/// <summary>
/// Interface for all event envelopes.
/// </summary>
public interface IEventEnvelope : IEnvelope
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    new IEvent Message { get; }
}

/// <summary>
/// Interface for all event envelopes.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
public interface IEventEnvelope<TEvent, TMetadata> : IEnvelope<TEvent, TMetadata>, IEventEnvelope
    where TEvent : IEvent
    where TMetadata : IMetadata
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    new TEvent Message { get; }
}