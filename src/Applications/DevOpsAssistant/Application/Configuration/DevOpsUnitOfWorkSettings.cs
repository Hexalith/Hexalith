// ***********************************************************************
// Assembly         : DevOpsAssistant
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="DevOpsUnitOfWorkSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DevOpsAssistant.Application.Configuration;

using DevOpsAssistant.Domain.Aggregates;

using Hexalith.Application.Abstractions.Tasks;
using Hexalith.Extensions.Configuration;

/// <summary>
/// Class DevOpsUnitOfWorkSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class DevOpsUnitOfWorkSettings : ISettings
{
    /// <summary>
    /// Gets or sets the command execution resiliency policy.
    /// </summary>
    /// <value>The resiliency policy.</value>
    public ResiliencyPolicy? ExecuteCommandResiliencyPolicy { get; set; }

    /// <summary>
    /// Configurations the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string ConfigurationName()
    {
        return nameof(DevOpsUnitOfWork);
    }
}