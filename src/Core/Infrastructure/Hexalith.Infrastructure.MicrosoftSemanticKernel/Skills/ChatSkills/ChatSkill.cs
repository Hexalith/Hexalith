// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="ChatSkill.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Hexalith.Application.ConversationThreads;
using Hexalith.Domain.ConversationThreads.Entities;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Model;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

/// <summary>
/// ChatSkill offers a more coherent chat experience by using memories
/// to extract conversation history and user intentions.
/// </summary>
public class ChatSkill
{
    /// <summary>
    /// The artificial intelligence settings.
    /// </summary>
    private readonly ArtificialIntelligenceServiceSettings _artificialIntelligenceSettings;

    /// <summary>
    /// A repository to save and retrieve chat messages.
    /// </summary>
    private readonly IConversationThreadService _conversationThreadService;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// A document memory skill instance to query document memories.
    /// </summary>
    private readonly DocumentMemorySkill _documentMemorySkill;

    /// <summary>
    /// A skill instance to acquire external information.
    /// </summary>
    private readonly ExternalInformationSkill _externalInformationSkill;

    /// <summary>
    /// A kernel instance to create a completion function since each invocation
    /// of the <see cref="ChatAsync" /> function will generate a new prompt dynamically.
    /// </summary>
    private readonly IKernel _kernel;

    /// <summary>
    /// Settings containing prompt texts.
    /// </summary>
    private readonly PromptsSettings _promptOptions;

    /// <summary>
    /// A semantic chat memory skill instance to query semantic memories.
    /// </summary>
    private readonly SemanticChatMemorySkill _semanticChatMemorySkill;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatSkill" /> class.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="conversationThreadService">The conversation thread service.</param>
    /// <param name="artificialIntelligenceSettings">The artificial intelligence settings.</param>
    /// <param name="planner">The planner.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="logger">The logger.</param>
    public ChatSkill(
        IKernel kernel,
        IConversationThreadService conversationThreadService,
        IOptions<ArtificialIntelligenceServiceSettings> artificialIntelligenceSettings,
        CopilotChatPlanner planner,
        IDateTimeService dateTimeService,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(kernel);
        ArgumentNullException.ThrowIfNull(conversationThreadService);
        ArgumentNullException.ThrowIfNull(artificialIntelligenceSettings);
        ArgumentNullException.ThrowIfNull(artificialIntelligenceSettings.Value.Prompts);
        ArgumentNullException.ThrowIfNull(artificialIntelligenceSettings.Value.DocumentMemory);
        ArgumentNullException.ThrowIfNull(planner);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        ArgumentNullException.ThrowIfNull(logger);
        _kernel = kernel;
        _conversationThreadService = conversationThreadService;
        _artificialIntelligenceSettings = artificialIntelligenceSettings.Value;
        _dateTimeService = dateTimeService;
        _promptOptions = artificialIntelligenceSettings.Value.Prompts;

        _semanticChatMemorySkill = new SemanticChatMemorySkill(_promptOptions);
        _documentMemorySkill = new DocumentMemorySkill(_promptOptions, _artificialIntelligenceSettings.DocumentMemory);
        _externalInformationSkill = new ExternalInformationSkill(_promptOptions, planner);
    }

    /// <summary>
    /// This is the entry point for getting a chat response. It manages the token limit, saves
    /// messages to memory, and fill in the necessary context variables for completing the
    /// prompt that will be rendered by the template engine.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="context">Contains the 'tokenLimit' and the 'contextTokenLimit' controlling the length of the prompt.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [SKFunction("Get chat response")]
    [SKFunctionName("Chat")]
    [SKFunctionInput(Description = "The new message")]
    [SKFunctionContextParameter(Name = "userId", Description = "Unique and persistent identifier for the user")]
    [SKFunctionContextParameter(Name = "userName", Description = "Name of the user")]
    [SKFunctionContextParameter(Name = "chatId", Description = "Unique and persistent identifier for the chat")]
    [SKFunctionContextParameter(Name = "proposedPlan", Description = "Previously proposed plan that is approved")]
    public async Task<SKContext> ChatAsync(string message, SKContext context)
    {
        // TODO: check if user has access to the chat
        string userId = context["userId"];
        string userName = context["userName"];
        string chatId = context["chatId"];

        // Save this new message to memory such that subsequent chat responses can use it
        try
        {
            await SaveNewMessageAsync(message, userId, userName, chatId);
        }
        catch (Exception ex) when (!ex.IsCriticalException())
        {
            context.Log.LogError("Unable to save new message: {0}", ex.Message);
            _ = context.Fail($"Unable to save new message: {ex.Message}", ex);
            return context;
        }

        // Clone the context to avoid modifying the original context variables.
        SKContext chatContext = Utilities.CopyContextWithVariablesClone(context);
        chatContext.Variables.Set("knowledgeCutoff", _promptOptions.KnowledgeCutoffDate);
        chatContext.Variables.Set("audience", chatContext["userName"]);

        string response = chatContext.Variables.ContainsKey("userCancelledPlan")
            ? "I am sorry the plan did not meet your goals."
            : await GetChatResponseAsync(chatContext);

        if (chatContext.ErrorOccurred)
        {
            _ = context.Fail(chatContext.LastErrorDescription);
            return context;
        }

        // Save this response to memory such that subsequent chat responses can use it
        try
        {
            await SaveNewResponseAsync(response, chatId);
        }
        catch (Exception ex) when (!ex.IsCriticalException())
        {
            context.Log.LogError("Unable to save new response: {0}", ex.Message);
            _ = context.Fail($"Unable to save new response: {ex.Message}");
            return context;
        }

        // Extract semantic chat memory
        await SemanticChatMemoryExtractor.ExtractSemanticChatMemoryAsync(
            chatId,
            _kernel,
            chatContext,
            _promptOptions);

        _ = context.Variables.Update(response);
        context.Variables.Set("userId", "Bot");
        return context;
    }

    /// <summary>
    /// Extract chat history.
    /// </summary>
    /// <param name="context">Contains the 'tokenLimit' controlling the length of the prompt.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [SKFunction("Extract chat history")]
    [SKFunctionName("ExtractChatHistory")]
    [SKFunctionContextParameter(Name = "chatId", Description = "Chat ID to extract history from")]
    [SKFunctionContextParameter(Name = "tokenLimit", Description = "Maximum number of tokens")]
    public async Task<string> ExtractChatHistoryAsync(SKContext context)
    {
        string chatId = context["chatId"];
        int tokenLimit = int.Parse(context["tokenLimit"], new NumberFormatInfo());

        IEnumerable<ConversationItem> messages = await _conversationThreadService.GetConversationMessagesAsync(chatId, CancellationToken.None);
        IOrderedEnumerable<ConversationItem> sortedMessages = messages.OrderByDescending(m => m.Date);

        int remainingToken = tokenLimit;
        string historyText = string.Empty;
        foreach (ConversationItem? chatMessage in sortedMessages)
        {
            string formattedMessage = ToFormattedString(chatMessage);
            int tokenCount = Utilities.TokenCount(formattedMessage);

            // Plan object is not meaningful content in generating chat response, exclude it
            if (remainingToken - tokenCount > 0 && !formattedMessage.Contains("proposedPlan\\\":", StringComparison.InvariantCultureIgnoreCase))
            {
                historyText = $"{formattedMessage}\n{historyText}";
                remainingToken -= tokenCount;
            }
            else
            {
                break;
            }
        }

        return $"Chat history:\n{historyText.Trim()}";
    }

    /// <summary>
    /// Extract user intent from the conversation history.
    /// </summary>
    /// <param name="context">The SKContext.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [SKFunction("Extract user intent")]
    [SKFunctionName("ExtractUserIntent")]
    [SKFunctionContextParameter(Name = "chatId", Description = "Chat ID to extract history from")]
    [SKFunctionContextParameter(Name = "audience", Description = "The audience the chat bot is interacting with.")]
    public async Task<string> ExtractUserIntentAsync(SKContext context)
    {
        int tokenLimit = _promptOptions.CompletionTokenLimit;
        int historyTokenBudget =
            tokenLimit -
            _promptOptions.ResponseTokenLimit -
            Utilities.TokenCount(string.Join("\n", new string[]
                {
                    _promptOptions.SystemDescription,
                    _promptOptions.SystemIntent,
                    _promptOptions.SystemIntentContinuation,
                }));

        // Clone the context to avoid modifying the original context variables.
        SKContext intentExtractionContext = Utilities.CopyContextWithVariablesClone(context);
        intentExtractionContext.Variables.Set("tokenLimit", historyTokenBudget.ToString(new NumberFormatInfo()));
        intentExtractionContext.Variables.Set("knowledgeCutoff", _promptOptions.KnowledgeCutoffDate);

        ISKFunction completionFunction = _kernel.CreateSemanticFunction(
            _promptOptions.SystemIntentExtraction,
            skillName: nameof(ChatSkill),
            description: "Complete the prompt.");

        SKContext result = await completionFunction.InvokeAsync(
            intentExtractionContext,
            settings: CreateIntentCompletionSettings());

        if (result.ErrorOccurred)
        {
            context.Log.LogError("{0}: {1}", result.LastErrorDescription, result.LastException);
            _ = context.Fail(result.LastErrorDescription);
            return string.Empty;
        }

        return $"User intent: {result}";
    }

    /// <summary>
    /// Converts to formattedstring.
    /// </summary>
    /// <param name="conversationItem">The conversation item.</param>
    /// <returns>System.String.</returns>
    public string ToFormattedString(ConversationItem conversationItem)
        => $"[{conversationItem.Date.ToString("G", CultureInfo.CurrentCulture)}] {conversationItem.Participant}: {conversationItem.Content}";

    /// <summary>
    /// Helper function create the correct context variables to acquire external information.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="userIntent">The user intent.</param>
    /// <param name="tokenLimit">The token limit.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    private Task<string> AcquireExternalInformationAsync(SKContext context, string userIntent, int tokenLimit)
    {
        ContextVariables contextVariables = context.Variables.Clone();
        contextVariables.Set("tokenLimit", tokenLimit.ToString(new NumberFormatInfo()));
        if (context.Variables.Get("proposedPlan", out string? proposedPlan))
        {
            contextVariables.Set("proposedPlan", proposedPlan);
        }

        SKContext planContext = new(
            contextVariables,
            context.Memory,
            context.Skills,
            context.Log,
            context.CancellationToken);

        Task<string> plan = _externalInformationSkill.AcquireExternalInformationAsync(userIntent, planContext);

        // Propagate the error
        if (planContext.ErrorOccurred)
        {
            _ = context.Fail(planContext.LastErrorDescription);
        }

        return plan;
    }

    /// <summary>
    /// Create a completion settings object for chat response. Parameters are read from the PromptSettings class.
    /// </summary>
    /// <returns>CompleteRequestSettings.</returns>
    private CompleteRequestSettings CreateChatResponseCompletionSettings()
    {
        CompleteRequestSettings completionSettings = new()
        {
            MaxTokens = _promptOptions.ResponseTokenLimit,
            Temperature = _promptOptions.ResponseTemperature,
            TopP = _promptOptions.ResponseTopP,
            FrequencyPenalty = _promptOptions.ResponseFrequencyPenalty,
            PresencePenalty = _promptOptions.ResponsePresencePenalty,
        };

        return completionSettings;
    }

    /// <summary>
    /// Create a completion settings object for intent response. Parameters are read from the PromptSettings class.
    /// </summary>
    /// <returns>CompleteRequestSettings.</returns>
    private CompleteRequestSettings CreateIntentCompletionSettings()
    {
        CompleteRequestSettings completionSettings = new()
        {
            MaxTokens = _promptOptions.ResponseTokenLimit,
            Temperature = _promptOptions.IntentTemperature,
            TopP = _promptOptions.IntentTopP,
            FrequencyPenalty = _promptOptions.IntentFrequencyPenalty,
            PresencePenalty = _promptOptions.IntentPresencePenalty,
            StopSequences = new string[] { "] bot:" },
        };

        return completionSettings;
    }

    /// <summary>
    /// Calculate the remaining token budget for the chat response prompt.
    /// This is the token limit minus the token count of the user intent and the system commands.
    /// </summary>
    /// <param name="userIntent">The user intent returned by the model.</param>
    /// <returns>The remaining token limit.</returns>
    private int GetChatContextTokenLimit(string userIntent)
    {
        int tokenLimit = _promptOptions.CompletionTokenLimit;
        int remainingToken =
            tokenLimit -
            Utilities.TokenCount(userIntent) -
            _promptOptions.ResponseTokenLimit -
            Utilities.TokenCount(string.Join("\n", new string[]
                {
                            _promptOptions.SystemDescription,
                            _promptOptions.SystemResponse,
                            _promptOptions.SystemChatContinuation,
                }));

        return remainingToken;
    }

    /// <summary>
    /// Helper function create the correct context variables to
    /// extract chat history messages from the conversation history.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="tokenLimit">The token limit.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    private Task<string> GetChatHistoryAsync(SKContext context, int tokenLimit)
    {
        ContextVariables contextVariables = new();
        contextVariables.Set("chatId", context["chatId"]);
        contextVariables.Set("tokenLimit", tokenLimit.ToString(new NumberFormatInfo()));

        SKContext chatHistoryContext = new(
            contextVariables,
            context.Memory,
            context.Skills,
            context.Log,
            context.CancellationToken);

        Task<string> chatHistory = ExtractChatHistoryAsync(chatHistoryContext);

        // Propagate the error
        if (chatHistoryContext.ErrorOccurred)
        {
            _ = context.Fail(chatHistoryContext.LastErrorDescription);
        }

        return chatHistory;
    }

    /// <summary>
    /// Generate the necessary chat context to create a prompt then invoke the model to get a response.
    /// </summary>
    /// <param name="chatContext">The SKContext.</param>
    /// <returns>A response from the model.</returns>
    private async Task<string> GetChatResponseAsync(SKContext chatContext)
    {
        // 1. Extract user intent from the conversation history.
        string userIntent = await GetUserIntentAsync(chatContext);
        if (chatContext.ErrorOccurred)
        {
            return string.Empty;
        }

        // 2. Calculate the remaining token budget.
        int remainingToken = GetChatContextTokenLimit(userIntent);

        // 3. Acquire external information from planner
        int externalInformationTokenLimit = (int)(remainingToken * _promptOptions.ExternalInformationContextWeight);
        string planResult = await AcquireExternalInformationAsync(chatContext, userIntent, externalInformationTokenLimit);
        if (chatContext.ErrorOccurred)
        {
            return string.Empty;
        }

        // If plan is suggested, send back to user for approval before running
        if (_externalInformationSkill.ProposedPlan != null)
        {
            return JsonSerializer.Serialize<ProposedPlan>(
                new ProposedPlan(_externalInformationSkill.ProposedPlan));
        }

        // 4. Query relevant semantic memories
        int chatMemoriesTokenLimit = (int)(remainingToken * _promptOptions.MemoriesResponseContextWeight);
        string chatMemories = await QueryChatMemoriesAsync(chatContext, userIntent, chatMemoriesTokenLimit);
        if (chatContext.ErrorOccurred)
        {
            return string.Empty;
        }

        // 5. Query relevant document memories
        int documentContextTokenLimit = (int)(remainingToken * _promptOptions.DocumentContextWeight);
        string documentMemories = await QueryDocumentsAsync(chatContext, userIntent, documentContextTokenLimit);
        if (chatContext.ErrorOccurred)
        {
            return string.Empty;
        }

        // 6. Fill in the chat history if there is any token budget left
        string chatContextText = string.Join("\n", new string[] { chatMemories, documentMemories, planResult });
        int chatContextTextTokenCount = remainingToken - Utilities.TokenCount(chatContextText);
        if (chatContextTextTokenCount > 0)
        {
            string chatHistory = await GetChatHistoryAsync(chatContext, chatContextTextTokenCount);
            if (chatContext.ErrorOccurred)
            {
                return string.Empty;
            }

            chatContextText = $"{chatContextText}\n{chatHistory}";
        }

        // Invoke the model
        chatContext.Variables.Set("UserIntent", userIntent);
        chatContext.Variables.Set("ChatContext", chatContextText);

        ISKFunction completionFunction = _kernel.CreateSemanticFunction(
            _promptOptions.SystemChatPrompt,
            skillName: nameof(ChatSkill),
            description: "Complete the prompt.");

        chatContext = await completionFunction.InvokeAsync(
            context: chatContext,
            settings: CreateChatResponseCompletionSettings());

        return chatContext.ErrorOccurred ? string.Empty : chatContext.Result;
    }

    /// <summary>
    /// Helper function create the correct context variables to
    /// extract user intent from the conversation history.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
    private async Task<string> GetUserIntentAsync(SKContext context)
    {
        if (!context.Variables.Get("planUserIntent", out string? userIntent))
        {
            ContextVariables contextVariables = new();
            contextVariables.Set("chatId", context["chatId"]);
            contextVariables.Set("audience", context["userName"]);

            SKContext intentContext = new(
                contextVariables,
                context.Memory,
                context.Skills,
                context.Log,
                context.CancellationToken);

            userIntent = await ExtractUserIntentAsync(intentContext);

            // Propagate the error
            if (intentContext.ErrorOccurred)
            {
                _ = context.Fail(intentContext.LastErrorDescription);
            }
        }

        return userIntent;
    }

    /// <summary>
    /// Helper function create the correct context variables to
    /// query chat memories from the chat memory store.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="userIntent">The user intent.</param>
    /// <param name="tokenLimit">The token limit.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    private Task<string> QueryChatMemoriesAsync(SKContext context, string userIntent, int tokenLimit)
    {
        ContextVariables contextVariables = new();
        contextVariables.Set("chatId", context["chatId"]);
        contextVariables.Set("tokenLimit", tokenLimit.ToString(new NumberFormatInfo()));

        SKContext chatMemoriesContext = new(
            contextVariables,
            context.Memory,
            context.Skills,
            context.Log,
            context.CancellationToken);

        Task<string> chatMemories = _semanticChatMemorySkill.QueryMemoriesAsync(userIntent, chatMemoriesContext);

        // Propagate the error
        if (chatMemoriesContext.ErrorOccurred)
        {
            _ = context.Fail(chatMemoriesContext.LastErrorDescription);
        }

        return chatMemories;
    }

    /// <summary>
    /// Helper function create the correct context variables to
    /// query document memories from the document memory store.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="userIntent">The user intent.</param>
    /// <param name="tokenLimit">The token limit.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    private Task<string> QueryDocumentsAsync(SKContext context, string userIntent, int tokenLimit)
    {
        ContextVariables contextVariables = new();
        contextVariables.Set("chatId", context["chatId"]);
        contextVariables.Set("tokenLimit", tokenLimit.ToString(new NumberFormatInfo()));

        SKContext documentMemoriesContext = new(
            contextVariables,
            context.Memory,
            context.Skills,
            context.Log,
            context.CancellationToken);

        Task<string> documentMemories = _documentMemorySkill.QueryDocumentsAsync(userIntent, documentMemoriesContext);

        // Propagate the error
        if (documentMemoriesContext.ErrorOccurred)
        {
            _ = context.Fail(documentMemoriesContext.LastErrorDescription);
        }

        return documentMemories;
    }

    /// <summary>
    /// Save a new message to the chat history.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="chatId">The chat ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task SaveNewMessageAsync(string message, string userId, string userName, string chatId)
        => await _conversationThreadService.AddMessageAsync(chatId, userId, userName, message, CancellationToken.None);

    /// <summary>
    /// Save a new response to the chat history.
    /// </summary>
    /// <param name="response">Response from the chat.</param>
    /// <param name="chatId">The chat ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task SaveNewResponseAsync(string response, string chatId)
        => await _conversationThreadService.AddMessageAsync(chatId, "AI", "Fority", response, CancellationToken.None);
}