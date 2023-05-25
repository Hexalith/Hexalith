// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftBot
// Author           : Jérôme Piquot
// Created          : 05-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-17-2023
// ***********************************************************************
// <copyright file="SemanticKernelHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.CoreSkills;

/// <summary>
/// Class MicrosoftBotHelper.
/// </summary>
public static class SemanticKernelHelper
{
    /// <summary>
    /// Adds the completion service.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>KernelConfig.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static KernelConfig AddCompletionService([NotNull] this KernelConfig config, [NotNull] ArtificialIntelligenceServiceSettings settings)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(settings);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService.Type);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService.Name);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService.ApplicationKey);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService.Model);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.TextEmbeddingModelService);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.TextEmbeddingModelService.Type);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.TextEmbeddingModelService.Name);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.TextEmbeddingModelService.ApplicationKey);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.TextEmbeddingModelService.Model);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.ChatModelService);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.ChatModelService.Type);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.ChatModelService.Name);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.ChatModelService.ApplicationKey);
        _ = SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.ChatModelService.Model);
        config = settings.CompletionModelService.Type switch
        {
            ModelServiceType.AzureOpenAI =>
                    config.AddAzureTextCompletionService(
                    settings.CompletionModelService.Name,
                    SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService.Endpoint)!,
                    settings.CompletionModelService.ApplicationKey),
            ModelServiceType.OpenAI =>
                _ = config.AddOpenAITextCompletionService(
                    settings.CompletionModelService.Model,
                    settings.CompletionModelService.ApplicationKey,
                    settings.CompletionModelService.OrganizationId),
            _ => throw new InvalidOperationException($"Unknown completion service type {settings.CompletionModelService.Type}"),
        };
        config = settings.ChatModelService.Type switch
        {
            ModelServiceType.AzureOpenAI =>
                    config.AddAzureChatCompletionService(
                    settings.CompletionModelService.Name,
                    SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.CompletionModelService.Endpoint)!,
                    settings.CompletionModelService.ApplicationKey),
            ModelServiceType.OpenAI =>
                _ = config.AddOpenAIChatCompletionService(
                    settings.CompletionModelService.Model,
                    settings.CompletionModelService.ApplicationKey,
                    settings.CompletionModelService.OrganizationId),
            _ => throw new InvalidOperationException($"Unknown completion service type {settings.CompletionModelService.Type}"),
        };
        return settings.TextEmbeddingModelService.Type switch
        {
            ModelServiceType.AzureOpenAI =>
                    config.AddAzureTextEmbeddingGenerationService(
                    settings.TextEmbeddingModelService.Name,
                    settings.TextEmbeddingModelService.Model,
                    SettingsException<ArtificialIntelligenceServiceSettings>.ThrowIfUndefined(settings.TextEmbeddingModelService.Endpoint)!,
                    settings.TextEmbeddingModelService.ApplicationKey),
            ModelServiceType.OpenAI =>
                _ = config.AddAzureTextEmbeddingGenerationService(
                    settings.TextEmbeddingModelService.Name,
                    settings.TextEmbeddingModelService.Model,
                    settings.TextEmbeddingModelService.ApplicationKey,
                    settings.TextEmbeddingModelService.OrganizationId),
            _ => throw new InvalidOperationException($"Unknown completion service type {settings.TextEmbeddingModelService.Type}"),
        };
    }

    /// <summary>
    /// Adds the hexalith.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>IKernel.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IKernel AddHexalith([NotNull] this KernelConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        // _ = kernel.ImportSkill(cloudDriveSkill, nameof(Microsoft.SemanticKernel.CoreSkills.CloudDriveSkill));
        return config.AddHexalith();
    }

    /// <summary>
    /// Adds the Microsoft semantic kernel services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<ArtificialIntelligenceService>()
            .ConfigureSettings<ArtificialIntelligenceServiceSettings>(configuration);
    }

    /// <summary>
    /// Adds the skills.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <returns>IKernel.</returns>
    public static IKernel AddSkills(this IKernel kernel)
    {
        ConversationSummarySkill conversationSummarySkill = new(kernel);
        _ = kernel.ImportSkill(conversationSummarySkill, nameof(ConversationSummarySkill));
        FileIOSkill fileIoSkill = new();
        _ = kernel.ImportSkill(fileIoSkill, nameof(FileIOSkill));
        HttpSkill httpSkill = new();
        _ = kernel.ImportSkill(httpSkill, nameof(HttpSkill));
        MathSkill mathSkill = new();
        _ = kernel.ImportSkill(mathSkill, nameof(MathSkill));
        TextMemorySkill textMemorySkill = new();
        _ = kernel.ImportSkill(textMemorySkill, nameof(TextMemorySkill));
        TextSkill textSkill = new();
        _ = kernel.ImportSkill(textSkill, nameof(TextSkill));
        TimeSkill timeSkill = new();
        _ = kernel.ImportSkill(timeSkill, nameof(TimeSkill));
        WaitSkill waitSkill = new();
        _ = kernel.ImportSkill(waitSkill, nameof(WaitSkill));
        _ = kernel.ImportApplicationCommandsAsSkills();
        _ = kernel.ImportSemanticSkillFromDirectory(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Skills"));
        return kernel;
    }
}