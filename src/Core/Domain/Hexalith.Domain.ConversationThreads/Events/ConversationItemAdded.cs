// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="ConversationItemAdded.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ConversationThreads.Events;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Class AddConversationItem.
/// Implements the <see cref="ConversationThreadEvent" />.
/// </summary>
/// <seealso cref="ConversationThreadEvent" />
[DataContract]
[Serializable]
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
    /// Initializes a new instance of the <see cref="ConversationItemAdded" /> class.
    /// </summary>
    [Obsolete("For serialization purpose only", true)]
    public ConversationItemAdded()
    {
        Participant = Content = string.Empty;
        ItemDate = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>The content.</value>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    public DateTimeOffset ItemDate { get; set; }

    /// <summary>
    /// Gets or sets the participant.
    /// </summary>
    /// <value>The participant.</value>
    public string Participant { get; set; }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultTypeName() => nameof(ConversationItemAdded);
}