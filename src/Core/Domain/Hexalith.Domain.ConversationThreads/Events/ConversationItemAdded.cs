// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationItemAdded.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ConversationThreads.Events;

using System;

/// <summary>
/// Class AddConversationItem.
/// Implements the <see cref="ConversationThreadEvent" />.
/// </summary>
/// <seealso cref="ConversationThreadEvent" />
public class ConversationItemAdded : ConversationThreadEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationItemAdded" /> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="startedDate">The started date.</param>
    /// <param name="itemDate">The date.</param>
    /// <param name="participant">The participant.</param>
    /// <param name="content">The content.</param>
    public ConversationItemAdded(string owner, DateTimeOffset startedDate, DateTimeOffset itemDate, string participant, string content)
        : base(owner, startedDate)
    {
        ItemDate = itemDate;
        Participant = participant;
        Content = content;
    }

    /// <summary>
    /// Gets the content.
    /// </summary>
    /// <value>The content.</value>
    public string Content { get; }

    /// <summary>
    /// Gets the date.
    /// </summary>
    /// <value>The date.</value>
    public DateTimeOffset ItemDate { get; }

    /// <summary>
    /// Gets the participant.
    /// </summary>
    /// <value>The participant.</value>
    public string Participant { get; }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultTypeName() => nameof(ConversationItemAdded);
}