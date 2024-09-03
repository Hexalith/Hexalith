// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 05-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-14-2023
// ***********************************************************************
// <copyright file="ICommandPromptGenerator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Prompts;

using System.Threading.Tasks;

using Hexalith.Application.Commands;

/// <summary>
/// Interface ICommandPromptGenerator.
/// </summary>
public interface ICommandPromptGenerator
{
    /// <summary>
    /// Generates the prompt asynchronous.
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <param name="assistantEmail">The assistant email.</param>
    /// <param name="assistantName">Name of the assistant.</param>
    /// <param name="userEmail">The user email.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="language">The language.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    Task<string> GeneratePromptAsync<TCommand>(
        string assistantEmail,
        string assistantName,
        string userEmail,
        string userName,
        string language,
        string correlationId)
        where TCommand : BaseCommand, new();
}