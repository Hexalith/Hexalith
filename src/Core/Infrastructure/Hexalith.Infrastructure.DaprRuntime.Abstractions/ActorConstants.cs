// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
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
    /// The continue callback method name.
    /// </summary>
    public const string ContinueCallbackMethodName = "ContinueAsync";

    /// <summary>
    /// The continue reminder name.
    /// </summary>
    public const string ContinueReminderName = "Continue";

    /// <summary>
    /// The continue timer name.
    /// </summary>
    public const string ContinueTimerName = "Continue";

    /// <summary>
    /// The event state name.
    /// </summary>
    public const string EventSourcingName = "EventSource";

    /// <summary>
    /// The message store name.
    /// </summary>
    public const string MessageStoreName = "Message";
}