// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-15-2023
// ***********************************************************************
// <copyright file="BaseEnvelope.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Envelopes;

using System.Runtime.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;
using Hexalith.Extensions;

/// <summary>
/// Class BaseEnvelope.
/// Implements the <see cref="IEnvelope{TMessage, TMetadata}" />.
/// </summary>
/// <typeparam name="TMessage">The type of the t message.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
/// <seealso cref="IEnvelope{TMessage, TMetadata}" />
[DataContract]
[Serializable]
public class BaseEnvelope<TMessage, TMetadata> : IEnvelope<TMessage, TMetadata>
    where TMessage : IMessage
    where TMetadata : IMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEnvelope{TMessage, TMetadata}" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    public BaseEnvelope(TMessage message, TMetadata metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEnvelope{TMessage, TMetadata}" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public BaseEnvelope()
    {
        Message = default!;
        Metadata = default!;
    }

    /// <inheritdoc/>
    [DataMember(Order = 1)]
    public TMessage Message { get; set; }

    /// <inheritdoc/>
    [DataMember(Order = 2)]
    public TMetadata Metadata { get; set; }

    /// <inheritdoc/>
    IMessage IEnvelope.Message => Message;

    /// <inheritdoc/>
    IMetadata IEnvelope.Metadata => Metadata;
}