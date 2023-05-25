// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="DocumentMemoryConfiguration.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Configuration options for handling memorized documents.
/// </summary>
public class DocumentMemoryConfiguration : ISettings
{
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
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "DocumentMemory";
}