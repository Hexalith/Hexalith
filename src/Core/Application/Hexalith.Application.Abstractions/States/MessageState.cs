// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="MessageState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;

/// <summary>
/// Class MessageState.
/// Implements the <see cref="IIdempotent" />.
/// </summary>
/// <seealso cref="IIdempotent" />
[DataContract]
[Obsolete("This interface is obsolete. Use Hexalith.Application.MessageMetadatas.ApplicationMessageState instead.", false)]
public class MessageState : MessageState<BaseMessage, BaseMetadata>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState" /> class.
    /// </summary>
    public MessageState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public MessageState(
        DateTimeOffset? receivedDate,
        BaseMessage? message,
        BaseMetadata? metadata)
        : base(receivedDate, message, metadata)
    {
    }
}