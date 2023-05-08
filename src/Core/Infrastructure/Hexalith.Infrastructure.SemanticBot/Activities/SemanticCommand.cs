// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.SemanticBot
// Author           : Jérôme Piquot
// Created          : 05-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-08-2023
// ***********************************************************************
// <copyright file="SemanticCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.SemanticBot.Activities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

using Hexalith.Application.Abstractions.Commands;

/// <summary>
/// Class SemanticCommand.
/// Implements the <see cref="Hexalith.Infrastructure.SemanticBot.Activities.ISemanticActivity" />.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <seealso cref="Hexalith.Infrastructure.SemanticBot.Activities.ISemanticActivity" />
public abstract class SemanticCommand<TCommand> : ISemanticActivity
    where TCommand : BaseCommand, new()
{
    /// <summary>
    /// The command.
    /// </summary>
    private static readonly TCommand _command = new();

    private object? _classificationPrompt;

    private string? _name;

    /// <inheritdoc/>
    public string Classification => (string)(_classificationPrompt ??= GenerateClassificationPrompt());

    /// <inheritdoc/>
    public string Name
        => _name ??= string.IsNullOrWhiteSpace(_command?.TypeName)
        ? throw new InvalidOperationException($"Command {typeof(TCommand).Name} type name is not defined.")
        : _command.TypeName;

    /// <inheritdoc/>
    public string Prompt => $$"""
    Answer using the json example in __JSONRESULT__ section if you have all the information in the __MESSAGE__ section.
    If a property is not defined and it's not a required value, it will be set to the value defined in __JSONRESULT__ section.
    If a property is not defined and it's a required property, ask the user to complete the information.
    If information complete but not validated, ask the user to validate the gathered information.
    If the user validate the information, answer with the completed JSON using the example in __JSONRESULT__ section.
    __JSONRESULT__
    {{JsonSerializer.Serialize(_command, new JsonSerializerOptions { WriteIndented = true })}}
    __MESSAGE__
    """;

    /// <inheritdoc/>
    public Type ResponseType => typeof(TCommand);

    private string GenerateClassificationPrompt()
    {
        Type commandType = typeof(TCommand);
        DescriptionAttribute? description = commandType.GetCustomAttribute<DescriptionAttribute>();
        if (description is not null)
        {
            return CheckValue(description.Description, nameof(DescriptionAttribute));
        }

        DisplayAttribute? display = commandType.GetCustomAttribute<DisplayAttribute>();
        if (display is not null)
        {
            return string.IsNullOrWhiteSpace(display.Description)
            ? CheckValue(display.Name, nameof(DisplayAttribute))
            : CheckValue(display.Description, nameof(DisplayAttribute));
        }

        DisplayNameAttribute? displayName = commandType.GetCustomAttribute<DisplayNameAttribute>();
        return displayName is not null
            ? CheckValue(displayName.DisplayName, nameof(DisplayNameAttribute))
            : throw new InvalidOperationException($"The command {commandType.Name} does not have any description (DescriptionAttribute) or display attribute (DisplayAttribute or DisplayNameAttribute).");
        string CheckValue(string? value, string attributeType) => string.IsNullOrWhiteSpace(value)
                ? throw new InvalidOperationException($"The command {commandType.Name} attribute {attributeType} has an empty value.")
                : value;
    }
}