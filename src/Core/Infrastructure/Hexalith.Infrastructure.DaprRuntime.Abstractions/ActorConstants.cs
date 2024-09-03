// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="ActorConstants.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Abstractions;

/// <summary>
/// Class ActorConstants.
/// </summary>
public static class ActorConstants
{
    /// <summary>
    /// Gets the aggregate state store name.
    /// </summary>
    public static string AggregateStateStoreName => "State";

    /// <summary>
    /// Gets the command store name.
    /// </summary>
    public static string CommandStoreName => "Command";

    /// <summary>
    /// Gets the event state name.
    /// </summary>
    public static string EventSourcingName => "EventSource";

    /// <summary>
    /// Gets the message store name.
    /// </summary>
    public static string MessageStoreName => "Message";

    /// <summary>
    /// Gets the process reminder name.
    /// </summary>
    public static string ProcessReminderName => "ContinueProcess";

    /// <summary>
    /// Gets the process timer name.
    /// </summary>
    public static string ProcessTimerName => "ContinueProcess";

    /// <summary>
    /// Gets the publish reminder name.
    /// </summary>
    public static string PublishReminderName => "ContinuePublish";

    /// <summary>
    /// Gets the publish timer name.
    /// </summary>
    public static string PublishTimerName => "ContinuePublish";
}