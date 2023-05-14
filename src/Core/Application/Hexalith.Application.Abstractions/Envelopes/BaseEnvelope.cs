// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-15-2023
// ***********************************************************************
// <copyright file="BaseEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Envelopes;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;

/// <summary>
/// Class BaseEnvelope.
/// Implements the <see cref="IEnvelope{TMessage, TMetadata}" />.
/// </summary>
/// <typeparam name="TMessage">The type of the t message.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
/// <seealso cref="IEnvelope{TMessage, TMetadata}" />
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
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public BaseEnvelope()
    {
        Message = default!;
        Metadata = default!;
    }

    /// <inheritdoc/>
    public TMessage Message { get; }

    /// <inheritdoc/>
    IMessage IEnvelope.Message => Message;

    /// <inheritdoc/>
    public TMetadata Metadata { get; }

    /// <inheritdoc/>
    IMetadata IEnvelope.Metadata => Metadata;
}