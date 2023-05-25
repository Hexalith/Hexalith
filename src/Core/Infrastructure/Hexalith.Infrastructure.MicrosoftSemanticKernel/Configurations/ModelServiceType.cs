// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-23-2023
// ***********************************************************************
// <copyright file="ModelServiceType.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

/// <summary>
/// Enum CompletionServiceType.
/// </summary>
public enum ModelServiceType
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