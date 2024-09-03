// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 01-04-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="AggregateActorState.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

/// <summary>
/// The aggregate state class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
[Serializable]
[DataContract]
public class AggregateActorState
{
    /// <summary>
    /// Gets or sets the command count.
    /// </summary>
    /// <value>The command count.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public long CommandCount { get; set; }

    /// <summary>
    /// Gets or sets the event count.
    /// </summary>
    /// <value>The event count.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public long EventSourceCount { get; set; }

    /// <summary>
    /// Gets or sets the last command processed.
    /// </summary>
    /// <value>The last command processed.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public long LastCommandProcessed { get; set; }

    /// <summary>
    /// Gets or sets the last event processed.
    /// </summary>
    /// <value>The last event processed.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public long LastMessagePublished { get; set; }

    /// <summary>
    /// Gets or sets the message count.
    /// </summary>
    /// <value>The message count.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public long MessageCount { get; set; }

    /// <summary>
    /// Gets or sets the process reminder due time.
    /// </summary>
    /// <value>The process reminder due time.</value>
    [DataMember(Order = 7)]
    [JsonPropertyOrder(7)]
    public TimeSpan? ProcessReminderDueTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [publish failed].
    /// </summary>
    /// <value><c>true</c> if [publish failed]; otherwise, <c>false</c>.</value>
    [DataMember(Order = 8)]
    [JsonPropertyOrder(8)]
    public bool PublishFailed { get; set; }

    /// <summary>
    /// Gets or sets the reminder.
    /// </summary>
    /// <value>The reminder.</value>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    public TimeSpan? PublishReminderDueTime { get; set; }

    /// <summary>
    /// Gets or sets the retry on failure time.
    /// </summary>
    /// <value>The retry on failure time.</value>
    [DataMember(Order = 9)]
    [JsonPropertyOrder(9)]
    public DateTimeOffset? RetryOnFailureDateTime { get; set; }

    /// <summary>
    /// Gets or sets the retry on failure period.
    /// </summary>
    /// <value>The retry on failure period.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public TimeSpan? RetryOnFailurePeriod { get; set; }
}