// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftBot
// Author           : Jérôme Piquot
// Created          : 05-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-08-2023
// ***********************************************************************
// <copyright file="SemanticBotHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.TeamsBot.Helpers;

using Hexalith.Infrastructure.SemanticBot.Adapters;

using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.TeamsFx.Conversation;

/// <summary>
/// Class MicrosoftBotHelper.
/// </summary>
public static class SemanticBotHelper
{
    /// <summary>
    /// Adds the microsoft bot.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddSemanticBot(this IServiceCollection services, IConfiguration configuration)
    {
        return services

            // .AddSemanticKernel(configuration)
            .AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>()
            .AddSingleton<CloudAdapter, TeamsBotAdapter>()
            .AddSingleton<IBotFrameworkHttpAdapter>(sp => sp.GetRequiredService<CloudAdapter>())

            // .AddTransient<IBot, SemanticTeamsBot>()
            .AddSingleton(sp =>
            {
                ConversationOptions options = new()
                {
                    Adapter = sp.GetService<CloudAdapter>(),
                };

                return new ConversationBot(options);
            });
    }
}