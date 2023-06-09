// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="AuthorRoles.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

/// <summary>
/// Role of the author of a chat message.
/// </summary>
public enum AuthorRole
{
    /// <summary>
    /// The current user of the chat.
    /// </summary>
    User,

    /// <summary>
    /// The bot.
    /// </summary>
    Bot,

    /// <summary>
    /// The participant who is not the current user nor the bot of the chat.
    /// </summary>
    Participant,
}