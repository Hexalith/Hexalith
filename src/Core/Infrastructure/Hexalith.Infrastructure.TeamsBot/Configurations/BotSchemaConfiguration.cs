// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="BotSchemaConfiguration.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.TeamsBot.Configurations;

using System.ComponentModel.DataAnnotations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Configuration options for the bot file schema that is supported by this application.
/// </summary>
public class BotSchemaConfiguration : ISettings
{
    /// <summary>
    /// Gets or sets the name of the schema.
    /// </summary>
    /// <value>The name.</value>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the version of the schema.
    /// </summary>
    /// <value>The version.</value>
    [Range(0, int.MaxValue)]
    public int Version { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "BotSchema";
}