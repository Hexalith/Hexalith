// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="ActorConstants.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
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
    /// The aggregate state store name.
    /// </summary>
    public const string AggregateStateStoreName = "State";

    /// <summary>
    /// The command store name.
    /// </summary>
    public const string CommandStoreName = "Command";

    /// <summary>
    /// The event state name.
    /// </summary>
    public const string EventSourcingName = "EventSource";

    /// <summary>
    /// The message store name.
    /// </summary>
    public const string MessageStoreName = "Message";

    /// <summary>
    /// The process reminder name.
    /// </summary>
    public const string ProcessReminderName = "ContinueProcess";

    /// <summary>
    /// The process timer name.
    /// </summary>
    public const string ProcessTimerName = "ContinueProcess";

    /// <summary>
    /// The publish reminder name.
    /// </summary>
    public const string PublishReminderName = "ContinuePublish";

    /// <summary>
    /// The publish timer name.
    /// </summary>
    public const string PublishTimerName = "ContinuePublish";
}