// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-15-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-22-2023
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
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Reflections;

using Humanizer;

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
            skill.Add(m.Key, kernel.ImportCommandSkill(m.Value.GetType()));
        }

        return skill;
    }

    /// <summary>
    /// Imports the command skill.
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <param name="kernel">The kernel.</param>
    /// <returns>ISKFunction.</returns>
    public static ISKFunction ImportCommandSkill<TCommand>(this IKernel kernel)
        where TCommand : BaseCommand
        => kernel.ImportCommandSkill(typeof(TCommand));

    /// <summary>
    /// Imports the command skill.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="commandType">Type of the command.</param>
    /// <returns>ISKFunction.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ISKFunction ImportCommandSkill(this IKernel kernel, Type commandType)
    {
        ArgumentNullException.ThrowIfNull(kernel);
        BaseCommand command = (BaseCommand)commandType.CreateExample();
        string skillName = command.AggregateName;
        string functionName = command.TypeName;
        PromptTemplateConfig config = new()
        {
            Type = "completion",
            Description = GetDescription(command.GetType()),
            Input = GetInputParameters(GetProperties(command)),
            Completion = new()
            {
                MaxTokens = 2000,
            },
        };
        IEnumerable<CommandProperty> props = GetProperties(command);
        string templatePrompt = GetTemplate(command, config.Description, GetExampleName(command.GetType()), props);

        // Load prompt template
        PromptTemplate template = new(
            templatePrompt,
            config,
            kernel.PromptTemplateEngine);

        SemanticFunctionConfig functionConfig = new(config, template);

        kernel.Log.LogTrace("Registering command skill {0}/{1} :\n{2}", skillName, functionName, templatePrompt);
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

        return type.Name.Humanize();
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
                ? throw new InvalidOperationException($"The property {property.Name} description attribute on class {property.DeclaringType?.FullName} is empty.")
                : descriptionAttribute.Description;
        }

        // Get the display attribute
        DisplayAttribute? displayAttribute = property.GetCustomAttribute<DisplayAttribute>(false);
        if (displayAttribute != null)
        {
            // Get the description from the display attribute
            return string.IsNullOrWhiteSpace(displayAttribute.Description)
                ? throw new InvalidOperationException(
                    $"The property {property.Name} description in the display attribute on class {property.DeclaringType?.Name} is empty.")
                : displayAttribute.Description;
        }

        return property.Name.Humanize();
    }

    private static string GetExampleName(Type type)
    {
        // Get the example value attribute
        ExampleNameAttribute? attribute = type.GetCustomAttribute<ExampleNameAttribute>();
        return attribute is not null ? attribute.Name : string.Empty;
    }

    /// <summary>
    /// Gets the ignore.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    private static bool GetIgnore(PropertyInfo property)
            => property.GetCustomAttribute<JsonIgnoreAttribute>() != null || property.GetCustomAttribute<IgnoreDataMemberAttribute>() != null;

    /// <summary>
    /// Gets the input parameters.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>Microsoft.SemanticKernel.SemanticFunctions.PromptTemplateConfig.InputConfig.</returns>
    private static PromptTemplateConfig.InputConfig GetInputParameters(IEnumerable<CommandProperty> properties)
    {
        List<PromptTemplateConfig.InputParameter> parameters = properties.Select(p => new PromptTemplateConfig.InputParameter
        {
            Name = p.Name,
            Description = p.Description,
            DefaultValue = p.DefaultValue,
        }).ToList();
        return new PromptTemplateConfig.InputConfig
        {
            Parameters = parameters,
        };
    }

    /// <summary>
    /// Gets the is required.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>bool.</returns>
    private static bool GetIsRequired(PropertyInfo property)
        => property.GetCustomAttribute<RequiredAttribute>() != null;

    /// <summary>
    /// Gets the properties.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers.CommandSkillHelper.CommandProperty&gt;.</returns>
    private static IEnumerable<CommandProperty> GetProperties(BaseCommand command)
    {
        // Get all public properties with a setter
        PropertyInfo[] properties = command.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
        return properties
            .Where(p => !GetIgnore(p) || !p.CanWrite)
            .Select(p => new CommandProperty(
            p.Name,
            GetDescription(p),
            GetDefaultValue(p),
            GetIsRequired(p))).ToList();
    }

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="commandDescription">The description.</param>
    /// <param name="properties">The properties.</param>
    /// <returns>string.</returns>
    private static string GetTemplate(BaseCommand commandExample, string commandDescription, string exampleDescription, IEnumerable<CommandProperty> properties)
    {
        IEnumerable<(string, bool)> requiredProperties = properties
            .Select(a =>
                ($"{a.Name}: Enter the {a.Description.ToLowerInvariant()}. {(string.IsNullOrWhiteSpace(a.DefaultValue) ? string.Empty : " Default value is " + a.DefaultValue + ".")}", a.Required));
        return $$"""
        You are an AI assistant tasked with assisting the user in crafting a JSON command message. Your role is to complete any missing information or prompt the user for necessary details that haven't been provided yet.
        The command is  : {{commandDescription.ToLowerInvariant()}}.

        Please follow these steps to {{commandDescription.ToLowerInvariant()}} in the system:

        Enter the following required properties:
        {{string.Join("  \n", requiredProperties.Where(p => p.Item2).Select(p => p.Item1))}}

        Enter the following optional properties if available:
        {{string.Join("  \n", requiredProperties.Where(p => !p.Item2).Select(p => p.Item1))}}

        Review the entered information in the __INPUT__ section. Once you have entered all necessary details, please ensure they are correct.

        Validate the information. After reviewing the information, confirm that it is correct.

        If you have completed all the steps and validated the information, the system will return the complete JSON for the {{commandDescription.ToLowerInvariant()}} command, as shown in the example below for {{exampleDescription}}:

        __JSON__
        {{JsonSerializer.Serialize<BaseCommand>(commandExample, new JsonSerializerOptions { WriteIndented = true })}}

        __INPUT__
        """ + "\n{{$input}}";
    }

    private record CommandProperty(string Name, string Description, string DefaultValue, bool Required)
    {
    }
}