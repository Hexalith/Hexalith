// <copyright file="CommandProcessorSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

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