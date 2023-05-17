// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-15-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-15-2023
// ***********************************************************************
// <copyright file="CommandSkillHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

using global::Hexalith.Application.Commands;
using global::Hexalith.Extensions.Reflections;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;
using Microsoft.SemanticKernel.SkillDefinition;

/// <summary>
/// Class CommandSkillHelper.
/// </summary>
public static class CommandSkillHelper
{
    /// <summary>
    /// Imports the semantic skill from commands.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <returns>System.Collections.Generic.IDictionary&lt;string, Microsoft.SemanticKernel.Orchestration.ISKFunction&gt;.</returns>
    public static IDictionary<string, ISKFunction> ImportApplicationCommandsAsSkills(this IKernel kernel)
    {
        Dictionary<string, ISKFunction> skill = new();

        Dictionary<string, BaseCommand> map = TypeMapper<BaseCommand>.GetMap();

        foreach (KeyValuePair<string, BaseCommand> m in map)
        {
            skill.Add(m.Key, kernel.ImportCommandSkill(m.Value));
        }

        return skill;
    }

    /// <summary>
    /// Imports the semantic skill from command.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="command">The command.</param>
    /// <returns>Microsoft.SemanticKernel.Orchestration.ISKFunction.</returns>
    public static ISKFunction ImportCommandSkill(this IKernel kernel, BaseCommand command)
    {
        _ = command.GetType();
        string skillName = command.AggregateName;
        string functionName = command.TypeName;
        PromptTemplateConfig config = new()
        {
            Type = "completion",
            Description = GetDescription(command.GetType()),
            Input = GetInputParameters(command),
            Completion = new()
            {
                MaxTokens = 2000,
            },
        };
        kernel.Log.LogTrace("Adding command skill {0}/{1}", skillName, functionName);

        // Load prompt template
        PromptTemplate template = new(
            GetTemplate(command),
            config,
            kernel.PromptTemplateEngine);

        SemanticFunctionConfig functionConfig = new(config, template);

        return kernel.RegisterSemanticFunction(skillName, functionName, functionConfig);
    }

    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>string.</returns>
    private static string GetDefaultValue(PropertyInfo property)
    {
        // Get the default value attribute
        DefaultValueAttribute? defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
        if (defaultValueAttribute is not null && defaultValueAttribute.Value is not null)
        {
            // Get the description from the description attribute
            return JsonSerializer.Serialize(defaultValueAttribute.Value);
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>string.</returns>
    /// <exception cref="InvalidOperationException">$"The command {type.Name} should have a description. Use the 'Description' or 'Display' attribute on the class to define the command action description needed for the semantic kernel function.</exception>
    private static string GetDescription(Type type)
    {
        // Get the description attribute
        DescriptionAttribute? descriptionAttribute = type.GetCustomAttribute<DescriptionAttribute>(false);
        if (descriptionAttribute != null)
        {
            // Get the description from the description attribute
            return string.IsNullOrWhiteSpace(descriptionAttribute.Description)
                ? throw new InvalidOperationException($"The command description in the description attribute on class {type.FullName} is empty. It must describe the command action description needed for the semantic kernel function.")
                : descriptionAttribute.Description;
        }

        // Get the display attribute
        DisplayAttribute? displayAttribute = type.GetCustomAttribute<DisplayAttribute>(false);
        if (displayAttribute != null)
        {
            // Get the description from the display attribute
            return string.IsNullOrWhiteSpace(displayAttribute.Description)
                ? throw new InvalidOperationException($"The command description in the display attribute on class {type.FullName} is empty. It must describe the command action description needed for the semantic kernel function.")
                : displayAttribute.Description;
        }

        throw new InvalidOperationException($"The command {type.Name} should have a description. Use the 'Description' or 'Display' attribute on the class to define the command action description needed for the semantic kernel function.");
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>string.</returns>
    /// <exception cref="InvalidOperationException">$"The command {property.Name} should have a description. Use the 'Description' or 'Display' attribute on the class to define the command action description needed for the semantic kernel function.</exception>
    private static string GetDescription(PropertyInfo property)
    {
        // Get the description attribute
        DescriptionAttribute? descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>(false);
        if (descriptionAttribute != null)
        {
            // Get the description from the description attribute
            return string.IsNullOrWhiteSpace(descriptionAttribute.Description)
                ? throw new InvalidOperationException($"The command description in the description attribute on class {property.DeclaringType?.FullName} is empty. It must describe the command action description needed for the semantic kernel function.")
                : descriptionAttribute.Description;
        }

        // Get the display attribute
        DisplayAttribute? displayAttribute = property.GetCustomAttribute<DisplayAttribute>(false);
        if (displayAttribute != null)
        {
            // Get the description from the display attribute
            return string.IsNullOrWhiteSpace(displayAttribute.Description)
                ? throw new InvalidOperationException($"The command description in the display attribute on class {property.DeclaringType?.FullName} is empty. It must describe the command action description needed for the semantic kernel function.")
                : displayAttribute.Description;
        }

        throw new InvalidOperationException($"The command {property.Name} should have a description. Use the 'Description' or 'Display' attribute on the class to define the command action description needed for the semantic kernel function.");
    }

    /// <summary>
    /// Gets the input parameters.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>Microsoft.SemanticKernel.SemanticFunctions.PromptTemplateConfig.InputConfig.</returns>
    private static PromptTemplateConfig.InputConfig GetInputParameters(BaseCommand command)
    {
        // Get all public properties with a setter
        PropertyInfo[] properties = command.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
        List<PromptTemplateConfig.InputParameter> parameters = properties.Select(p => new PromptTemplateConfig.InputParameter
        {
            Name = p.Name,
            Description = GetDescription(p),
            DefaultValue = GetDefaultValue(p),
        }).ToList();
        return new PromptTemplateConfig.InputConfig
        {
            Parameters = parameters,
        };
    }

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>string.</returns>
    private static string GetTemplate(BaseCommand value)
        => $$"""
        {type.Name}
        """;
}