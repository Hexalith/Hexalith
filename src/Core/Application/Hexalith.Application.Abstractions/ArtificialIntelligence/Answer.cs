// <copyright file="Answer.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.ArtificialIntelligence;

using System.Collections.Generic;

/// <summary>
/// Class Answer.
/// </summary>
public class Answer
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the variables.
    /// </summary>
    /// <value>The variables.</value>
    public IEnumerable<KeyValuePair<string, string>>? Variables { get; set; } = [];
}