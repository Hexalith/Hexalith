// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="SemanticChatMemoryItem.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Text.Json.Serialization;

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

/// <summary>
/// A single entry in the chat memory.
/// </summary>
public class SemanticChatMemoryItem
{
    /// <summary>
    /// Create a new chat memory item.
    /// </summary>
    /// <param name="label">Label of the item.</param>
    /// <param name="details">Details of the item.</param>
    public SemanticChatMemoryItem(string label, string details)
    {
        Label = label;
        Details = details;
    }

    /// <summary>
    /// Details for the chat memory item.
    /// </summary>
    /// <value>The details.</value>
    [JsonPropertyName("details")]
    public string Details { get; set; }

    /// <summary>
    /// Label for the chat memory item.
    /// </summary>
    /// <value>The label.</value>
    [JsonPropertyName("label")]
    public string Label { get; set; }

    /// <summary>
    /// Format the chat memory item as a string.
    /// </summary>
    /// <returns>A formatted string representing the item.</returns>
    public string ToFormattedString()
    {
        return $"{Label}: {Details}";
    }
}