// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-23-2023
// ***********************************************************************
// <copyright file="AiModelConfiguration.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

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