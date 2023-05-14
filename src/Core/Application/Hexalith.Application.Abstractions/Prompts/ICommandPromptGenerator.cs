// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 05-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-14-2023
// ***********************************************************************
// <copyright file="ICommandPromptGenerator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Prompts;

using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;

/// <summary>
/// Interface ICommandPromptGenerator.
/// </summary>
public interface ICommandPromptGenerator
{
    /// <summary>
    /// Generates the prompt asynchronous.
    /// </summary>
    /// <param name="baseCommand">The base command.</param>
    /// <param name="metadata">The metadata.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    public Task<string> GeneratePromptAsync(BaseCommand baseCommand, Metadata metadata);
}