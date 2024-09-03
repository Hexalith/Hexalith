// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="MessageState{TMessage,TMetadata}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;

/// <summary>
/// Class MessageState.
/// </summary>
/// <typeparam name="TMessage">The type of the t message.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
[DataContract]
[Serializable]
public class MessageState<TMessage, TMetadata> : IMessageState
    where TMessage : BaseMessage
    where TMetadata : BaseMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState{TMessage, TMetadata}" /> class.
    /// </summary>
    public MessageState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState{TMessage, TMetadata}" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public MessageState(DateTimeOffset? receivedDate, TMessage? message, TMetadata? metadata)
    {
        ReceivedDate = receivedDate;
        Message = message;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the idempotency identifier.
    /// </summary>
    /// <value>The idempotency identifier.</value>
    /// <exception cref="InvalidOperationException">The Idempotency identifier is not defined.</exception>
    [JsonIgnore]
    [IgnoreDataMember]
    public string IdempotencyId => string.IsNullOrWhiteSpace(Metadata?.Message.Id)
        ? throw new InvalidOperationException("The Idempotency identifier is not defined.")
        : Metadata.Message.Id;

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public TMessage? Message { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public TMetadata? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the received date.
    /// </summary>
    /// <value>The received date.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public DateTimeOffset? ReceivedDate { get; set; }

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    BaseMessage? IMessageState.Message => Message;

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    BaseMetadata? IMessageState.Metadata => Metadata;
}