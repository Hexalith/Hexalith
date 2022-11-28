// <copyright file="BaseEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Abstractions.Events;

using Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;

/// <summary>
/// Base class for business events.
/// </summary>
[DataContract]
public abstract class BaseEvent : Message, IEvent
{
}