// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="ArtificialIntelligenceServiceSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Artificial Intelligence Service Settings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class ArtificialIntelligenceServiceSettings : ISettings
{
    /// <summary>
    /// Gets or sets the bot user identifier.
    /// </summary>
    /// <value>The bot user identifier.</value>
    public string BotUserId { get; set; } = "AI";

    /// <summary>
    /// Gets or sets the name of the bot user.
    /// </summary>
    /// <value>The name of the bot user.</value>
    public string BotUserName { get; set; } = "Bot";

    /// <summary>
    /// Gets or sets the text model service.
    /// </summary>
    /// <value>The text model service.</value>
    public ArtificialIntelligenceModelSettings? ChatModelService { get; set; }

    /// <summary>
    /// Gets or sets the chat model service.
    /// </summary>
    /// <value>The chat model service.</value>
    public ArtificialIntelligenceModelSettings? CompletionModelService { get; set; }

    /// <summary>
    /// Gets or sets the document memory.
    /// </summary>
    /// <value>The document memory.</value>
    public DocumentMemorySettings? DocumentMemory { get; set; }

    /// <summary>
    /// Gets or sets the prompts.
    /// </summary>
    /// <value>The prompts.</value>
    public PromptsSettings? Prompts { get; set; }

    /// <summary>
    /// Gets or sets the text model service.
    /// </summary>
    /// <value>The text model service.</value>
    public ArtificialIntelligenceModelSettings? TextEmbeddingModelService { get; set; }

    /// <summary>
    /// Configurations the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string ConfigurationName() => "ArtificialIntelligence";
}