// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationThreadEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ConversationThreads.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.ConversationThreads.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Class ConversationThreadEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
[DataContract]
[Serializable]
public class ConversationThreadEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationThreadEvent" /> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="startedDate">The started date.</param>
    public ConversationThreadEvent(string owner, DateTimeOffset startedDate)
    {
        Owner = owner;
        StartedDate = startedDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationThreadEvent"/> class.
    /// </summary>
    [Obsolete("For serialization purpose only", true)]
    public ConversationThreadEvent()
    {
        Owner = string.Empty;
        StartedDate = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the owner.
    /// </summary>
    /// <value>The owner.</value>
    public string Owner { get; set; }

    /// <summary>
    /// Gets or sets the started date.
    /// </summary>
    /// <value>The started date.</value>
    public DateTimeOffset StartedDate { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => ConversationThread.GetAggregateId(Owner, StartedDate);

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultAggregateName() => nameof(ConversationThread);
}