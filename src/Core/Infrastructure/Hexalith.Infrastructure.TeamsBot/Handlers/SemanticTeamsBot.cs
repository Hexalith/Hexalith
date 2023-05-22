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
namespace Hexalith.Infrastructure.TeamsBot.Handlers;

using System.Threading.Tasks;

using Hexalith.Application.Conversations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Services;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;

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
    private readonly IEnumerable<IConversationActivity> _conversationActivities;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticTeamsBot" /> class.
    /// </summary>
    /// <param name="conversationActivities">The semantic activities.</param>
    /// <param name="artificialIntelligenceService">The artificial intelligence service.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public SemanticTeamsBot(IEnumerable<IConversationActivity> conversationActivities, ArtificialIntelligenceService artificialIntelligenceService)
    {
        ArgumentNullException.ThrowIfNull(conversationActivities);
        ArgumentNullException.ThrowIfNull(artificialIntelligenceService);
        _conversationActivities = conversationActivities;
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
        _ = _artificialIntelligenceService.Kernel.GetService<IChatCompletion>("GPT");
        SKContext context = _artificialIntelligenceService.Kernel.CreateNewContext();
        _ = _artificialIntelligenceService.Kernel.ImportApplicationCommandsAsSkills();
        _ = _artificialIntelligenceService.Kernel.AddSkills();
        context["AssistantEmail"] = "hexai@hexalith.com";
        context["AssistantName"] = "Hexai";
        context["UserEmail"] = "jdoe@hexalith.com";
        context["UserName"] = "John Doe";
        ActionPlanner planner = new(_artificialIntelligenceService.Kernel);
        Plan plan = await planner.CreatePlanAsync(turnContext.Activity.Text);
        string jsonPlan = plan.ToJson();
        plan = Plan.FromJson(jsonPlan, context);
        int iterations = 0;

        while (plan.HasNextStep &&
               iterations < 100)
        {
            plan = await _artificialIntelligenceService.Kernel.StepAsync(context.Variables, plan);
            iterations++;
        }

        // Sends an activity to the sender of the incoming activity.
        _ = await turnContext
            .SendActivityAsync(MessageFactory.Text(plan.State.Input), cancellationToken)
            .ConfigureAwait(false);
    }
}