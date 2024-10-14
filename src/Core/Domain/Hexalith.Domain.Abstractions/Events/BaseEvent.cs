// <copyright file="BaseEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Base class for business events.
/// </summary>
[DataContract]
[Serializable]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseEvent>))]
[Obsolete]
public class BaseEvent : BaseMessage, IEvent
{
}