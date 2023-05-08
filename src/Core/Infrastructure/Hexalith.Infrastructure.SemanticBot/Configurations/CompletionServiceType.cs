// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-23-2023
// ***********************************************************************
// <copyright file="CompletionServiceType.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.SemanticBot.Configurations;

/// <summary>
/// Enum CompletionServiceType.
/// </summary>
public enum CompletionServiceType
{
    /// <summary>
    /// The Microsoft Azure Open AI service.
    /// </summary>
    AzureOpenAI,

    /// <summary>
    /// The Open AI service.
    /// </summary>
    OpenAI,
}