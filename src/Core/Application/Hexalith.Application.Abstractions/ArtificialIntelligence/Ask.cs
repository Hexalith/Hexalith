// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-30-2023
// ***********************************************************************
// <copyright file="Ask.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.ArtificialIntelligence;

using System.Collections.Generic;

/// <summary>
/// Class Ask.
/// </summary>
public class Ask
{
    /// <summary>
    /// Gets or sets the input.
    /// </summary>
    /// <value>The input.</value>
    public string Input { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the variables.
    /// </summary>
    /// <value>The variables.</value>
    public IEnumerable<KeyValuePair<string, string>> Variables { get; set; } = [];
}