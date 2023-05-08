// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-29-2023
// ***********************************************************************
// <copyright file="ArtificialIntelligenceServiceSettings.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.SemanticBot.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Artificial Intelligence Service Settings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class ArtificialIntelligenceServiceSettings : ISettings
{
    /// <summary>
    /// Gets or sets the chat model service.
    /// </summary>
    /// <value>The chat model service.</value>
    public AiModelConfiguration? ChatModelService { get; set; }

    /// <summary>
    /// Gets or sets the text model service.
    /// </summary>
    /// <value>The text model service.</value>
    public AiModelConfiguration? TextModelService { get; set; }

    /// <summary>
    /// Configurations the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string ConfigurationName() => "AI";
}