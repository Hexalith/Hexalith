// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-23-2023
// ***********************************************************************
// <copyright file="ArtificialIntelligenceServiceSettings.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.SemanticBot.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Artificial Intelligence Model Service Configuration.
/// Implements the <see cref="ISettings" />.
/// </summary>
public class AiModelConfiguration
{
    /// <summary>
    /// Gets or sets the application key.
    /// </summary>
    /// <value>The application key.</value>
    public string? ApplicationKey { get; set; }

    /// <summary>
    /// Gets or sets the Azure OpenAI server endpoint.
    /// </summary>
    /// <value>The endpoint.</value>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the LLM model name.
    /// </summary>
    /// <value>The model.</value>
    public string? Model { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the organization ID for OpenAI accounts with multiple orgs.
    /// </summary>
    /// <value>The organization.</value>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    public CompletionServiceType? Type { get; set; }
}