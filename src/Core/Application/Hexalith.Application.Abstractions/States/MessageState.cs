// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="MessageState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Abstractions.States;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Common;

/// <summary>
/// Class MessageState.
/// Implements the <see cref="IIdempotent" />.
/// </summary>
/// <seealso cref="IIdempotent" />
[Serializable]
[DataContract]
public class MessageState : MessageState<BaseMessage, Metadata>
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
        Metadata? metadata)
        : base(receivedDate, message, metadata)
    {
    }
}