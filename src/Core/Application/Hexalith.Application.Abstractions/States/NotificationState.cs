// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="NotificationState.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Application.Abstractions.Notifications;

/// <summary>
/// Class NotificationState.
/// </summary>
[DataContract]
public class NotificationState : MessageState<BaseNotification, Metadata>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationState" /> class.
    /// </summary>
    public NotificationState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="message">The notification.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public NotificationState(
        DateTimeOffset? receivedDate,
        BaseNotification? message,
        Metadata? metadata)
        : base(receivedDate, message, metadata)
    {
    }
}