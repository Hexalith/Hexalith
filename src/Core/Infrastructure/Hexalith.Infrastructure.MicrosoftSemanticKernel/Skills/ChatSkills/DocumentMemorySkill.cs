// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="DocumentMemorySkill.cs" company="Fiveforty SAS Paris France">
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
/// This skill provides the functions to query the document memory.
/// </summary>
public class DocumentMemorySkill
{
    /// <summary>
    /// Configuration settings for importing documents to memory.
    /// </summary>
    private readonly DocumentMemorySettings _documentImportOptions;

    /// <summary>
    /// Prompt settings.
    /// </summary>
    private readonly PromptsSettings _promptOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentMemorySkill" /> class.
    /// Create a new instance of DocumentMemorySkill.
    /// </summary>
    /// <param name="promptOptions">The prompt options.</param>
    /// <param name="documentImportOptions">The document import options.</param>
    public DocumentMemorySkill(PromptsSettings promptOptions, DocumentMemorySettings documentImportOptions)
    {
        _promptOptions = promptOptions;
        _documentImportOptions = documentImportOptions;
    }

    /// <summary>
    /// Query the document memory collection for documents that match the query.
    /// </summary>
    /// <param name="query">Query to match.</param>
    /// <param name="context">The SkContext.</param>
    /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
    [SKFunction("Query documents in the memory given a user message")]
    [SKFunctionName("QueryDocuments")]
    [SKFunctionInput(Description = "Query to match.")]
    [SKFunctionContextParameter(Name = "chatId", Description = "ID of the chat that owns the documents")]
    [SKFunctionContextParameter(Name = "tokenLimit", Description = "Maximum number of tokens")]
    public async Task<string> QueryDocumentsAsync(string query, SKContext context)
    {
        string chatId = context.Variables["chatId"];
        int tokenLimit = int.Parse(context.Variables["tokenLimit"], new NumberFormatInfo());
        int remainingToken = tokenLimit;

        // Search for relevant document snippets.
        string[] documentCollections = new string[]
        {
            _documentImportOptions.ChatDocumentCollectionNamePrefix + chatId,
            _documentImportOptions.GlobalDocumentCollectionName,
        };

        List<MemoryQueryResult> relevantMemories = new();
        foreach (string documentCollection in documentCollections)
        {
            IAsyncEnumerable<MemoryQueryResult> results = context.Memory.SearchAsync(
                documentCollection,
                query,
                limit: 100,
                minRelevanceScore: _promptOptions.DocumentMemoryMinRelevance);
            await foreach (MemoryQueryResult memory in results)
            {
                relevantMemories.Add(memory);
            }
        }

        relevantMemories = relevantMemories.OrderByDescending(m => m.Relevance).ToList();

        // Concatenate the relevant document snippets.
        string documentsText = string.Empty;
        foreach (MemoryQueryResult memory in relevantMemories)
        {
            int tokenCount = Utilities.TokenCount(memory.Metadata.Text);
            if (remainingToken - tokenCount > 0)
            {
                documentsText += $"\n\nSnippet from {memory.Metadata.Description}: {memory.Metadata.Text}";
                remainingToken -= tokenCount;
            }
            else
            {
                break;
            }
        }

        if (string.IsNullOrEmpty(documentsText))
        {
            // No relevant documents found
            return string.Empty;
        }

        return $"User has also shared some document snippets:\n{documentsText}";
    }
}