// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-11-2023
// ***********************************************************************
// <copyright file="SemanticTeamsBot.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.SemanticBot.Handlers;

using Hexalith.Infrastructure.SemanticBot.Activities;
using Hexalith.Infrastructure.SemanticBot.Services;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;

/// <summary>
/// An empty bot handler.
/// You can add your customization code here to extend your bot logic if needed.
/// </summary>
public class SemanticTeamsBot : TeamsActivityHandler
{
    /// <summary>
    /// The artificial intelligence service.
    /// </summary>
    private readonly ArtificialIntelligenceService _artificialIntelligenceService;

    /// <summary>
    /// The semantic activities.
    /// </summary>
    private readonly IEnumerable<ISemanticActivity> _semanticActivities;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticTeamsBot" /> class.
    /// </summary>
    /// <param name="semanticActivities">The semantic activities.</param>
    /// <param name="artificialIntelligenceService">The artificial intelligence service.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public SemanticTeamsBot(IEnumerable<ISemanticActivity> semanticActivities, ArtificialIntelligenceService artificialIntelligenceService)
    {
        ArgumentNullException.ThrowIfNull(semanticActivities);
        ArgumentNullException.ThrowIfNull(artificialIntelligenceService);
        _semanticActivities = semanticActivities;
        _artificialIntelligenceService = artificialIntelligenceService;
    }

    /// <inheritdoc/>
    public override Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        => base.OnTurnAsync(turnContext, cancellationToken);

    /// <inheritdoc/>
    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(turnContext);
        await base.OnMessageActivityAsync(turnContext, cancellationToken).ConfigureAwait(false);
        if (turnContext.Responded)
        {
            return;
        }

        // Read adaptive card template
        IChatCompletion chat = _artificialIntelligenceService.Kernel.GetService<IChatCompletion>("GPT");
        OpenAIChatHistory conversation = (OpenAIChatHistory)chat.CreateNewChat("The following is a conversation with Fority, the Fiveforty company AI assistant. The assistant is helpful, creative, clever, very friendly and a Microsoft Dynamics 365 for finance and operations ERP expert. The assistant thinks step by step.");
        conversation.AddUserMessage(turnContext.Activity.Text);
        string reply = await chat.GenerateMessageAsync(conversation, new ChatRequestSettings(), cancellationToken).ConfigureAwait(false);

        // Sends an activity to the sender of the incoming activity.
        _ = await turnContext
            .SendActivityAsync(MessageFactory.Text(reply), cancellationToken)
            .ConfigureAwait(false);
    }
}