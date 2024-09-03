// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="IConversationThreadService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.ConversationThreads;

using System.Threading.Tasks;

using Hexalith.Domain.ConversationThreads.Entities;

/// <summary>
/// Interface IConversationThreadService.
/// </summary>
public interface IConversationThreadService
{
    /// <summary>
    /// Adds the message asynchronous.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="message">The message.</param>
    /// <param name="none">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddMessageAsync(string conversationId, string userId, string userName, string message, CancellationToken none);

    /// <summary>
    /// Gets the conversation messages asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;ConversationItem&gt;&gt;.</returns>
    Task<IEnumerable<ConversationItem>> GetConversationMessagesAsync(string id, CancellationToken cancellationToken);
}