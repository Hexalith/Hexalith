// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="ConversationThreadStarted.cs" company="Jérôme Piquot">
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
    /// Initializes a new instance of the <see cref="ConversationThreadStarted" /> class.
    /// </summary>
    [Obsolete("For serialization purpose only", true)]
    public ConversationThreadStarted()
    {
    }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultTypeName() => nameof(ConversationThreadStarted);
}