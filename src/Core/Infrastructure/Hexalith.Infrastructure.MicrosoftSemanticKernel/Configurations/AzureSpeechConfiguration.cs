// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="AzureSpeechConfiguration.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Configuration options for Azure speech recognition.
/// </summary>
public sealed class AzureSpeechConfiguration : ISettings
{
    /// <summary>
    /// Gets or sets key to access the Azure speech service.
    /// </summary>
    /// <value>The key.</value>
    public string? Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets location of the Azure speech service to use (e.g. "South Central US").
    /// </summary>
    /// <value>The region.</value>
    public string? Region { get; set; } = string.Empty;

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "AzureSpeech";
}