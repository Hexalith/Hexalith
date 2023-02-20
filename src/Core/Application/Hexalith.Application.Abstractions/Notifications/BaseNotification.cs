// <copyright file="BaseNotification.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Notifications;

using System.Runtime.Serialization;

using Hexalith.Domain.Abstractions.Messages;

/// <summary>
/// Class BaseNotification.
/// Implements the <see cref="BaseMessage" />
/// Implements the <see cref="Application.Notifications.INotification" />.
/// </summary>
/// <seealso cref="BaseMessage" />
/// <seealso cref="Application.Notifications.INotification" />
[DataContract]
[Serializable]
public class BaseNotification : BaseMessage, INotification
{
}