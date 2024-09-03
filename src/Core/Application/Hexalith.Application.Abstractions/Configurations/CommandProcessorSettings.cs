// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="CommandProcessorSettings.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Configurations;

using Hexalith.Application.Tasks;

/// <summary>
/// Class CommandProcessorSettings.
/// </summary>
public class CommandProcessorSettings
{
    /// <summary>
    /// Gets or sets the cleanup period.
    /// </summary>
    /// <value>The cleanup period.</value>
    public TimeSpan ActiveCommandCheckPeriod { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Gets or sets the resiliency policy.
    /// </summary>
    /// <value>The resiliency policy.</value>
    public ResiliencyPolicy ResiliencyPolicy { get; set; } = ResiliencyPolicy.CreateDefaultExponentialRetry();
}