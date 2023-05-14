// ***********************************************************************
// Assembly         : Hexalith.AI.AzureBot
// Author           : Jérôme Piquot
// Created          : 04-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-29-2023
// ***********************************************************************
// <copyright file="ArtificialIntelligenceService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Services;

using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

/// <summary>
/// Class ArtificialIntelligenceService.
/// </summary>
public class ArtificialIntelligenceService
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<ArtificialIntelligenceService> _logger;

    /// <summary>
    /// The settings.
    /// </summary>
    private readonly ArtificialIntelligenceServiceSettings _settings;

    /// <summary>
    /// The kernel.
    /// </summary>
    private IKernel? _kernel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtificialIntelligenceService" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    public ArtificialIntelligenceService(IOptions<ArtificialIntelligenceServiceSettings> settings, ILogger<ArtificialIntelligenceService> logger)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);
        _settings = settings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Gets the kernel.
    /// </summary>
    /// <value>The kernel.</value>
    public IKernel Kernel => _kernel ??= CreateKernel();

    /// <summary>
    /// Creates the kernel.
    /// </summary>
    /// <returns>System.Nullable&lt;IKernel&gt;.</returns>
    private IKernel CreateKernel()
    {
        KernelConfig config = new();
        config = config.AddCompletionService(_settings);
        return Microsoft.SemanticKernel.Kernel
            .Builder
            .WithLogger(_logger)
            .WithConfiguration(config)
            .Build();
    }
}