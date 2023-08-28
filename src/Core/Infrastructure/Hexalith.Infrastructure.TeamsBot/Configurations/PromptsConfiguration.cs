// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="PromptsConfiguration.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.TeamsBot.Configurations;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Configuration options for the chat.
/// </summary>
public class PromptsConfiguration : ISettings
{
    /// <summary>
    /// The property name.
    /// </summary>
    public const string PropertyName = "Prompts";

    // Chat commands

    /// <summary>
    /// The system chat continuation.
    /// </summary>
    private readonly string _systemChatContinuation = "SINGLE RESPONSE FROM BOT TO USER:\n[{{TimeSkill.Now}} {{timeSkill.Second}}] bot:";

    /// <summary>
    /// Gets or sets token limit of the chat model.
    /// </summary>
    /// <value>The completion token limit.</value>
    /// <remarks>https://platform.openai.com/docs/models/overview for token limits.</remarks>
    [Required]
    [Range(0, int.MaxValue)]
    public int CompletionTokenLimit { get; set; }

    /// <summary>
    /// Gets or sets the initial bot message.
    /// </summary>
    /// <value>The initial bot message.</value>
    [Required]
    public string InitialBotMessage { get; set; } = string.Empty;

    // System

    /// <summary>
    /// Gets or sets the knowledge cutoff date.
    /// </summary>
    /// <value>The knowledge cutoff date.</value>
    [Required]
    public string KnowledgeCutoffDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the long term memory extraction.
    /// </summary>
    /// <value>The long term memory extraction.</value>
    [Required]
    public string LongTermMemoryExtraction { get; set; } = string.Empty;

    // Long-term memory

    /// <summary>
    /// Gets or sets the long name of the term memory.
    /// </summary>
    /// <value>The long name of the term memory.</value>
    [Required]
    public string LongTermMemoryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the memory anti hallucination.
    /// </summary>
    /// <value>The memory anti hallucination.</value>
    [Required]
    public string MemoryAntiHallucination { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the memory continuation.
    /// </summary>
    /// <value>The memory continuation.</value>
    [Required]
    public string MemoryContinuation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the memory format.
    /// </summary>
    /// <value>The memory format.</value>
    [Required]
    public string MemoryFormat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token count left for the model to generate text after the prompt.
    /// </summary>
    /// <value>The response token limit.</value>
    [Required]
    [Range(0, int.MaxValue)]
    public int ResponseTokenLimit { get; set; }

    // Memory extraction

    /// <summary>
    /// Gets or sets the system cognitive.
    /// </summary>
    /// <value>The system cognitive.</value>
    [Required]
    public string SystemCognitive { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the system description.
    /// </summary>
    /// <value>The system description.</value>
    [Required]
    public string SystemDescription { get; set; } = string.Empty;

    // Intent extraction

    /// <summary>
    /// Gets or sets the system intent.
    /// </summary>
    /// <value>The system intent.</value>
    [Required]
    public string SystemIntent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the system intent continuation.
    /// </summary>
    /// <value>The system intent continuation.</value>
    [Required]
    public string SystemIntentContinuation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the system response.
    /// </summary>
    /// <value>The system response.</value>
    [Required]
    public string SystemResponse { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the working memory extraction.
    /// </summary>
    /// <value>The working memory extraction.</value>
    [Required]
    public string WorkingMemoryExtraction { get; set; } = string.Empty;

    // Working memory

    /// <summary>
    /// Gets or sets the name of the working memory.
    /// </summary>
    /// <value>The name of the working memory.</value>
    [Required]
    public string WorkingMemoryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets weight of documents in the contextual part of the final prompt.
    /// Contextual prompt excludes all the system commands.
    /// </summary>
    /// <value>The document context weight.</value>
    internal double DocumentContextWeight { get; } = 0.3;

    /// <summary>
    /// Gets minimum relevance of a document memory to be included in the final prompt.
    /// The higher the value, the answer will be more relevant to the user intent.
    /// </summary>
    /// <value>The document memory minimum relevance.</value>
    internal double DocumentMemoryMinRelevance { get; } = 0.8;

    /// <summary>
    /// Gets the intent frequency penalty.
    /// </summary>
    /// <value>The intent frequency penalty.</value>
    internal double IntentFrequencyPenalty { get; } = 0.5;

    /// <summary>
    /// Gets the intent presence penalty.
    /// </summary>
    /// <value>The intent presence penalty.</value>
    internal double IntentPresencePenalty { get; } = 0.5;

    /// <summary>
    /// Gets the intent temperature.
    /// </summary>
    /// <value>The intent temperature.</value>
    internal double IntentTemperature { get; } = 0.7;

    /// <summary>
    /// Gets the intent top p.
    /// </summary>
    /// <value>The intent top p.</value>
    internal double IntentTopP { get; } = 1;

    /// <summary>
    /// Gets the long term memory.
    /// </summary>
    /// <value>The long term memory.</value>
    internal string LongTermMemory => string.Join("\n", LongTermMemoryPromptComponents);

    /// <summary>
    /// Gets the long term memory prompt components.
    /// </summary>
    /// <value>The long term memory prompt components.</value>
    internal string[] LongTermMemoryPromptComponents => new string[]
        {
        SystemCognitive,
        $"{LongTermMemoryName} Description:\n{LongTermMemoryExtraction}",
        MemoryAntiHallucination,
        $"Chat Description:\n{SystemDescription}",
        "{{ChatSkill.ExtractChatHistory}}",
        MemoryContinuation,
        };

    /// <summary>
    /// Gets weight of memories in the contextual part of the final prompt.
    /// Contextual prompt excludes all the system commands.
    /// </summary>
    /// <value>The memories response context weight.</value>
    internal double MemoriesResponseContextWeight { get; } = 0.3;

    // Memory map

    /// <summary>
    /// Gets the memory map.
    /// </summary>
    /// <value>The memory map.</value>
    internal IDictionary<string, string> MemoryMap => new Dictionary<string, string>()
    {
        { LongTermMemoryName, LongTermMemory },
        { WorkingMemoryName, WorkingMemory },
    };

    /// <summary>
    /// Gets weight of information returned from planner (i.e., responses from OpenAPI skills).
    /// Percentage calculated from remaining token limit after memories response and document context have already been allocated.
    /// Contextual prompt excludes all the system commands.
    /// </summary>
    /// <value>The related information context weight.</value>
    internal double RelatedInformationContextWeight { get; } = 0.75;

    /// <summary>
    /// Gets the response frequency penalty.
    /// </summary>
    /// <value>The response frequency penalty.</value>
    internal double ResponseFrequencyPenalty { get; } = 0.5;

    /// <summary>
    /// Gets the response presence penalty.
    /// </summary>
    /// <value>The response presence penalty.</value>
    internal double ResponsePresencePenalty { get; } = 0.5;

    /// <summary>
    /// Gets the response temperature.
    /// </summary>
    /// <value>The response temperature.</value>
    internal double ResponseTemperature { get; } = 0.7;

    /// <summary>
    /// Gets the response top p.
    /// </summary>
    /// <value>The response top p.</value>
    internal double ResponseTopP { get; } = 1;

    /// <summary>
    /// Gets minimum relevance of a semantic memory to be included in the final prompt.
    /// The higher the value, the answer will be more relevant to the user intent.
    /// </summary>
    /// <value>The semantic memory minimum relevance.</value>
    internal double SemanticMemoryMinRelevance { get; } = 0.8;

    /// <summary>
    /// Gets the system chat prompt.
    /// </summary>
    /// <value>The system chat prompt.</value>
    internal string SystemChatPrompt => string.Join("\n", SystemChatPromptComponents);

    /// <summary>
    /// Gets the system chat prompt components.
    /// </summary>
    /// <value>The system chat prompt components.</value>
    internal string[] SystemChatPromptComponents => new string[]
    {
        SystemDescription,
        SystemResponse,
        "{{$userIntent}}",
        "{{ChatSkill.ExtractUserMemories}}",
        "{{DocumentMemorySkill.QueryDocuments $INPUT}}",
        "{{ChatSkill.AcquireExternalInformation}}",
        "{{ChatSkill.ExtractChatHistory}}",
        _systemChatContinuation,
    };

    /// <summary>
    /// Gets the system intent extraction.
    /// </summary>
    /// <value>The system intent extraction.</value>
    internal string SystemIntentExtraction => string.Join("\n", SystemIntentPromptComponents);

    /// <summary>
    /// Gets the system intent prompt components.
    /// </summary>
    /// <value>The system intent prompt components.</value>
    internal string[] SystemIntentPromptComponents => new string[]
                {
        SystemDescription,
        SystemIntent,
        "{{ChatSkill.ExtractChatHistory}}",
        SystemIntentContinuation,
                };

    /// <summary>
    /// Gets the working memory.
    /// </summary>
    /// <value>The working memory.</value>
    internal string WorkingMemory => string.Join("\n", WorkingMemoryPromptComponents);

    /// <summary>
    /// Gets the working memory prompt components.
    /// </summary>
    /// <value>The working memory prompt components.</value>
    internal string[] WorkingMemoryPromptComponents => new string[]
        {
        SystemCognitive,
        $"{WorkingMemoryName} Description:\n{WorkingMemoryExtraction}",
        MemoryAntiHallucination,
        $"Chat Description:\n{SystemDescription}",
        "{{ChatSkill.ExtractChatHistory}}",
        MemoryContinuation,
        };

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "Prompts";
}