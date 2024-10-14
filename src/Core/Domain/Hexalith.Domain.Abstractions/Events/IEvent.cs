// <copyright file="IEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.Domain.Messages;

/// <summary>
/// Interface for all events.
/// </summary>
[Obsolete]
public interface IEvent : IMessage
{
}