// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-29-2023
// ***********************************************************************
// <copyright file="TeamsBotAdapter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.SemanticBot.Adapters;

using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class AdapterWithErrorHandler.
/// Implements the <see cref="CloudAdapter" />.
/// </summary>
/// <seealso cref="CloudAdapter" />
public partial class TeamsBotAdapter : CloudAdapter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamsBotAdapter"/> class.
    /// </summary>
    /// <param name="auth">The authentication.</param>
    /// <param name="logger">The logger.</param>
    public TeamsBotAdapter(BotFrameworkAuthentication auth, ILogger<TeamsBotAdapter> logger)
        : base(auth, logger)
    {
        OnTurnError = async (turnContext, exception) =>
        {
            // Log any leaked exception from the application.
            BotUnhandledError(logger, exception, turnContext.Activity.Type);
            logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

            // Send a message to the user
            _ = await turnContext.SendActivityAsync($"The bot encountered an unhandled error: {exception.Message}").ConfigureAwait(false);
            _ = await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.").ConfigureAwait(false);

            // Send a trace activity
            _ = await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError").ConfigureAwait(false);
        };
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Bot unhandled error for activity : {activityType}")]
    public static partial void BotUnhandledError(ILogger logger, Exception ex, string? activityType);
}