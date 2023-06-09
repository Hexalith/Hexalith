// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="SemanticChatMemorySkill.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;

/// <summary>
/// This skill provides the functions to query the semantic chat memory.
/// </summary>
public class SemanticChatMemorySkill
{
    /// <summary>
    /// Prompt settings.
    /// </summary>
    private readonly PromptsSettings _promptOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticChatMemorySkill" /> class.
    /// Create a new instance of SemanticChatMemorySkill.
    /// </summary>
    /// <param name="promptOptions">The prompt options.</param>
    public SemanticChatMemorySkill(PromptsSettings promptOptions) => _promptOptions = promptOptions;

    /// <summary>
    /// Query relevant memories based on the query.
    /// </summary>
    /// <param name="query">Query to match.</param>
    /// <param name="context">The SKContext.</param>
    /// <returns>A string containing the relevant memories.</returns>
    [SKFunction("Query chat memories")]
    [SKFunctionName("QueryMemories")]
    [SKFunctionInput(Description = "Query to match.")]
    [SKFunctionContextParameter(Name = "chatId", Description = "Chat ID to query history from")]
    [SKFunctionContextParameter(Name = "tokenLimit", Description = "Maximum number of tokens")]
    public async Task<string> QueryMemoriesAsync(string query, SKContext context)
    {
        string chatId = context["chatId"];
        int tokenLimit = int.Parse(context["tokenLimit"], new NumberFormatInfo());
        int remainingToken = tokenLimit;

        // Search for relevant memories.
        List<MemoryQueryResult> relevantMemories = new();
        foreach (string memoryName in _promptOptions.MemoryMap.Keys)
        {
            IAsyncEnumerable<MemoryQueryResult> results = context.Memory.SearchAsync(
                SemanticChatMemoryExtractor.MemoryCollectionName(chatId, memoryName),
                query,
                limit: 100,
                minRelevanceScore: _promptOptions.SemanticMemoryMinRelevance);
            await foreach (MemoryQueryResult memory in results)
            {
                relevantMemories.Add(memory);
            }
        }

        relevantMemories = relevantMemories.OrderByDescending(m => m.Relevance).ToList();

        string memoryText = string.Empty;
        foreach (MemoryQueryResult memory in relevantMemories)
        {
            int tokenCount = Utilities.TokenCount(memory.Metadata.Text);
            if (remainingToken - tokenCount > 0)
            {
                memoryText += $"\n[{memory.Metadata.Description}] {memory.Metadata.Text}";
                remainingToken -= tokenCount;
            }
            else
            {
                break;
            }
        }

        return $"Past memories (format: [memory type] <label>: <details>):\n{memoryText.Trim()}";
    }
}