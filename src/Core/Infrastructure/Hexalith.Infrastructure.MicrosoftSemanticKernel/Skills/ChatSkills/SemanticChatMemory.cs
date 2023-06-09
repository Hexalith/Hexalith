// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="SemanticChatMemory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// A collection of semantic chat memory.
/// </summary>
public class SemanticChatMemory
{
    /// <summary>
    /// Gets or sets the chat memory items.
    /// </summary>
    /// <value>The items.</value>
    [JsonPropertyName("items")]
    public List<SemanticChatMemoryItem> Items { get; set; } = new List<SemanticChatMemoryItem>();

    /// <summary>
    /// Create a semantic chat memory from a Json string.
    /// </summary>
    /// <param name="json">Json string to deserialize.</param>
    /// <returns>A semantic chat memory.</returns>
    /// <exception cref="System.ArgumentException">Failed to deserialize chat memory to json.</exception>
    public static SemanticChatMemory FromJson(string json)
    {
        SemanticChatMemory? result = JsonSerializer.Deserialize<SemanticChatMemory>(json);
        return result ?? throw new ArgumentException("Failed to deserialize chat memory to json.");
    }

    /// <summary>
    /// Create and add a chat memory item.
    /// </summary>
    /// <param name="label">Label for the chat memory item.</param>
    /// <param name="details">Details for the chat memory item.</param>
    public void AddItem(string label, string details) => Items.Add(new SemanticChatMemoryItem(label, details));

    /// <summary>
    /// Serialize the chat memory to a Json string.
    /// </summary>
    /// <returns>A Json string representing the chat memory.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}