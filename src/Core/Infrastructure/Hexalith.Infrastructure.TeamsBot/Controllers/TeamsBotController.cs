// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.SemanticBot
// Author           : Jérôme Piquot
// Created          : 05-11-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-11-2023
// ***********************************************************************
// <copyright file="TeamsBotController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.TeamsBot.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.TeamsFx.Conversation;

/// <summary>
/// Class BotController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
/// <remarks>
/// Initializes a new instance of the <see cref="TeamsBotController" /> class.
/// </remarks>
/// <param name="conversation">The conversation.</param>
/// <param name="bot">The bot.</param>
[Route("api/messages")]
[ApiController]
public class TeamsBotController(ConversationBot conversation, IBot bot) : ControllerBase
{
    /// <summary>
    /// The bot.
    /// </summary>
    private readonly IBot _bot = bot;

    /// <summary>
    /// The conversation.
    /// </summary>
    private readonly ConversationBot _conversation = conversation;

    /// <summary>
    /// Post as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">The adapter is not a CloudAdapter.</exception>
    [HttpPost]
    public async Task PostAsync(CancellationToken cancellationToken = default)
    {
        CloudAdapter adapter = _conversation.Adapter as CloudAdapter
            ?? throw new InvalidOperationException("The adapter is not a CloudAdapter");

        await adapter
            .ProcessAsync(
                Request,
                Response,
                _bot,
                cancellationToken)
            .ConfigureAwait(false);
    }
}