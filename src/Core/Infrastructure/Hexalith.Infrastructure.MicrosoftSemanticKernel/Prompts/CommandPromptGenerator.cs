// <copyright file="CommandPromptGenerator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Prompts;

using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Prompts;

public class CommandPromptGenerator : ICommandPromptGenerator
{
    /// <inheritdoc/>
    public Task<string> GeneratePromptAsync(BaseCommand baseCommand, Metadata metadata) => throw new NotImplementedException();
}