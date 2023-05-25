// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationThreadEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ConversationThreads.Events;

using Hexalith.Domain.ConversationThreads.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Class ConversationThreadEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
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
    /// Gets the owner.
    /// </summary>
    /// <value>The owner.</value>
    public string Owner { get; }

    /// <summary>
    /// Gets the started date.
    /// </summary>
    /// <value>The started date.</value>
    public DateTimeOffset StartedDate { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => ConversationThread.GetAggregateId(Owner, StartedDate);

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultAggregateName() => nameof(ConversationThread);
}