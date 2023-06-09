// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="SemanticChatMemoryExtractor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;

/// <summary>
/// Helper class to extract and create semantic memory from chat history.
/// </summary>
internal static class SemanticChatMemoryExtractor
{
    /// <summary>
    /// Create a memory item in the memory collection.
    /// If there is already a memory item that has a high similarity score with the new item, it will be skipped.
    /// </summary>
    /// <param name="item">A SemanticChatMemoryItem instance.</param>
    /// <param name="chatId">The ID of the chat the memories belong to.</param>
    /// <param name="context">The context that contains the memory.</param>
    /// <param name="memoryName">Name of the memory.</param>
    /// <param name="options">The prompts options.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    internal static async Task CreateMemoryAsync(
        SemanticChatMemoryItem item,
        string chatId,
        SKContext context,
        string memoryName,
        PromptsSettings options)
    {
        string memoryCollectionName = MemoryCollectionName(chatId, memoryName);

        List<Microsoft.SemanticKernel.Memory.MemoryQueryResult> memories = await context.Memory.SearchAsync(
                collection: memoryCollectionName,
                query: item.ToFormattedString(),
                limit: 1,
                minRelevanceScore: options.SemanticMemoryMinRelevance,
                cancellationToken: context.CancellationToken)
            .ToListAsync(context.CancellationToken)
            .ConfigureAwait(false);

        if (memories.Count == 0)
        {
            _ = await context.Memory.SaveInformationAsync(
                collection: memoryCollectionName,
                text: item.ToFormattedString(),
                id: Guid.NewGuid().ToString(),
                description: memoryName,
                cancellationToken: context.CancellationToken);
        }
    }

    /// <summary>
    /// Extracts the semantic chat memory from the chat session.
    /// </summary>
    /// <param name="memoryName">Name of the memory category.</param>
    /// <param name="kernel">The semantic kernel.</param>
    /// <param name="context">The SKContext.</param>
    /// <param name="options">The prompts options.</param>
    /// <returns>A SemanticChatMemory object.</returns>
    /// <exception cref="System.ArgumentException">Memory name {memoryName} is not supported.</exception>
    internal static async Task<SemanticChatMemory> ExtractCognitiveMemoryAsync(
        string memoryName,
        IKernel kernel,
        SKContext context,
        PromptsSettings options)
    {
        if (!options.MemoryMap.TryGetValue(memoryName, out string? memoryPrompt))
        {
            throw new ArgumentException($"Memory name {memoryName} is not supported.");
        }

        // Token limit for chat history
        int tokenLimit = options.CompletionTokenLimit;
        int remainingToken =
            tokenLimit -
            options.ResponseTokenLimit -
            Utilities.TokenCount(memoryPrompt);

        SKContext memoryExtractionContext = Utilities.CopyContextWithVariablesClone(context);
        memoryExtractionContext.Variables.Set("tokenLimit", remainingToken.ToString(new NumberFormatInfo()));
        memoryExtractionContext.Variables.Set("memoryName", memoryName);
        memoryExtractionContext.Variables.Set("format", options.MemoryFormat);
        memoryExtractionContext.Variables.Set("knowledgeCutoff", options.KnowledgeCutoffDate);

        Microsoft.SemanticKernel.SkillDefinition.ISKFunction completionFunction = kernel.CreateSemanticFunction(memoryPrompt);
        SKContext result = await completionFunction.InvokeAsync(
            context: memoryExtractionContext,
            settings: CreateMemoryExtractionSettings(options));

        SemanticChatMemory memory = SemanticChatMemory.FromJson(result.ToString());
        return memory;
    }

    /// <summary>
    /// Extract and save semantic memory.
    /// </summary>
    /// <param name="chatId">The Chat ID.</param>
    /// <param name="kernel">The semantic kernel.</param>
    /// <param name="context">The context containing the memory.</param>
    /// <param name="options">The prompts options.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    internal static async Task ExtractSemanticChatMemoryAsync(
        string chatId,
        IKernel kernel,
        SKContext context,
        PromptsSettings options)
    {
        foreach (string memoryName in options.MemoryMap.Keys)
        {
            try
            {
                SemanticChatMemory semanticMemory = await ExtractCognitiveMemoryAsync(
                    memoryName,
                    kernel,
                    context,
                    options);
                foreach (SemanticChatMemoryItem item in semanticMemory.Items)
                {
                    await CreateMemoryAsync(item, chatId, context, memoryName, options);
                }
            }
            catch (Exception ex) when (!ex.IsCriticalException())
            {
                // Skip semantic memory extraction for this item if it fails.
                // We cannot rely on the model to response with perfect Json each time.
                context.Log.LogInformation("Unable to extract semantic memory for {0}: {1}. Continuing...", memoryName, ex.Message);
                continue;
            }
        }
    }

    /// <summary>
    /// Returns the name of the semantic text memory collection that stores chat semantic memory.
    /// </summary>
    /// <param name="chatId">Chat ID that is persistent and unique for the chat session.</param>
    /// <param name="memoryName">Name of the memory category.</param>
    /// <returns>System.String.</returns>
    internal static string MemoryCollectionName(string chatId, string memoryName) => $"{chatId}-{memoryName}";

    /// <summary>
    /// Create a completion settings object for chat response. Parameters are read from the PromptSettings class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns>CompleteRequestSettings.</returns>
    private static CompleteRequestSettings CreateMemoryExtractionSettings(PromptsSettings options)
    {
        CompleteRequestSettings completionSettings = new()
        {
            MaxTokens = options.ResponseTokenLimit,
            Temperature = options.ResponseTemperature,
            TopP = options.ResponseTopP,
            FrequencyPenalty = options.ResponseFrequencyPenalty,
            PresencePenalty = options.ResponsePresencePenalty,
        };

        return completionSettings;
    }
}