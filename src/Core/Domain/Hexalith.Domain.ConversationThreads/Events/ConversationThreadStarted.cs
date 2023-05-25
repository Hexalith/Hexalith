// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationThreadStarted.cs" company="Fiveforty SAS Paris France">
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
public class ConversationThreadStarted : ConversationThreadEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationThreadStarted" /> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="startedDate">The started date.</param>
    public ConversationThreadStarted(string owner, DateTimeOffset startedDate)
        : base(owner, startedDate)
    {
    }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultTypeName() => nameof(ConversationThreadStarted);
}