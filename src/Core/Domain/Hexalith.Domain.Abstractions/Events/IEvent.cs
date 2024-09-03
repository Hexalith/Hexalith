// <copyright file="IEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.Domain.Messages;

/// <summary>
/// Interface for all events.
/// </summary>
public interface IEvent : IMessage
{
}