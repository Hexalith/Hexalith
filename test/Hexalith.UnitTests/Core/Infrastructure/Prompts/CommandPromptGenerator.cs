// <copyright file="CommandPromptGenerator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Prompts;

using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Prompts;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class CommandPromptGenerator.
/// Implements the <see cref="ICommandPromptGenerator" />.
/// </summary>
/// <seealso cref="ICommandPromptGenerator" />
public class CommandPromptGenerator : ICommandPromptGenerator
{
    private static readonly ConcurrentDictionary<Type, string> _prompts = new();

    /// <inheritdoc/>
    public async Task<string> GeneratePromptAsync<TCommand>(
        string assistantEmail,
        string assistantName,
        string userEmail,
        string userName,
        string language,
        string correlationId)
        where TCommand : class, new()
    {
        if (_prompts.TryGetValue(typeof(TCommand), out string prompt))
        {
            return prompt;
        }

        TCommand command = ExampleHelper.CreateExample<TCommand>();
        MessageMetadata metadata = new(command, DateTimeOffset.UtcNow);
        return await Task.FromResult($$"""
        You are {{assistantName}} an AI assistant. Your email is {{assistantEmail}}.
        You can help {{userName}} generate a command in the JSON format.
        {{userName}} email is {{userEmail}}.
        The command JSON example is in __{{metadata.Name}}_EXAMPLE__ section.
        The conversation correlation id (CorrelationId) is '{{correlationId}}'.
        If the __CONVERSATION__ section contains all the required information to generate the {{metadata.Name}} command,
        ask the user to confirm the JSON text of the command.
        If the command has been confirmed, show the JSON text of the command to the user.
        __{{metadata.Name}}_EXAMPLE__
        {{JsonSerializer.Serialize(command, new JsonSerializerOptions() { WriteIndented = true })}}
        __CONVERSATION__
        """);
    }
}