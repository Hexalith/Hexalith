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

using System.ComponentModel.DataAnnotations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Configuration options for handling memorized documents.
/// </summary>
public class DocumentMemorySettings : ISettings
{
    /// <summary>
    /// Gets or sets the prefix for the chat document collection name.
    /// </summary>
    [Required]
    public string ChatDocumentCollectionNamePrefix { get; set; } = "chat-documents-";

    /// <summary>
    /// Gets or sets the maximum number of tokens to use when splitting a document into lines.
    /// Default token limits are suggested by OpenAI:
    /// https://help.openai.com/en/articles/4936856-what-are-tokens-and-how-to-count-them.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int DocumentLineSplitMaxTokens { get; set; } = 30;

    /// <summary>
    /// Gets or sets the maximum number of lines to use when combining lines into paragraphs.
    /// Default token limits are suggested by OpenAI:
    /// https://help.openai.com/en/articles/4936856-what-are-tokens-and-how-to-count-them.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int DocumentParagraphSplitMaxLines { get; set; } = 100;

    /// <summary>
    /// Gets or sets maximum size in bytes of a document to be allowed for importing.
    /// Prevent large uploads by setting a file size limit (in bytes) as suggested here:
    /// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int FileSizeLimit { get; set; } = 1000000;

    /// <summary>
    /// Gets or sets the name of the global document collection.
    /// </summary>
    [Required]
    public string GlobalDocumentCollectionName { get; set; } = "global-documents";

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "DocumentMemory";
}