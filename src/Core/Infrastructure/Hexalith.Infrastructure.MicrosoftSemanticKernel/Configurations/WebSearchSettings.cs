// <copyright file="WebSearchSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;

using Hexalith.Extensions.Configuration;

public class WebSearchSettings : ISettings
{
    public string? BingApiKey { get; set; }

    public string? GoogleApiKey { get; set; }

    public string? GoogleEngineId { get; set; }

    public static string ConfigurationName() => "WebSearch";
}