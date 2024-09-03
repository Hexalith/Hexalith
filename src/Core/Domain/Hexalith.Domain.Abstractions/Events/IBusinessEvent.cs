// <copyright file="IBusinessEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.Domain.Messages;

/// <summary>
/// Domain Event represents something significant that has happened within a specific Bounded Context.
/// These events are named in the past tense to clearly indicate that they’ve already occurred.
/// </summary>
public interface IBusinessEvent : IBusinessMessage
{
    /// <summary>
    /// Gets a value indicating whether this message is a behavior message.
    /// Behavior events are used to trigger a behavior in the system and are stored in the event store.
    /// </summary>
    bool IsBehavior => true;
}