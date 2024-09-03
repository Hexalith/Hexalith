// <copyright file="Dynamics365FinanceApplicationHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using System.Diagnostics.CodeAnalysis;

using Aspire.Hosting.ApplicationModel;

/// <summary>
/// Helper class for managing distributed Dynamics 365 for finance applications.
/// </summary>
public static class Dynamics365FinanceApplicationHelper
{
    /// <summary>
    /// Adds Dynamics 365 Finance environment settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <returns>The project resource builder reference.</returns>
    public static IResourceBuilder<ProjectResource> WithDynamics365Finance([NotNull] this IResourceBuilder<ProjectResource> project)
    {
        ArgumentNullException.ThrowIfNull(project);
        return project

            // .WithReference(project.ApplicationBuilder.AddQueueBinding("d365fnocustomersbinding"))
            // .WithReference(project.ApplicationBuilder.AddQueueBinding("d365fnopartnerinventorybinding"))
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Instance")
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Identity__Tenant")
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Identity__ApplicationId")
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Identity__ApplicationSecret")
            .WithEnvironmentFromConfiguration("Dynamics365FinanceClient-standard__AttemptTimeout__Timeout")
            .WithEnvironmentFromConfiguration("Dynamics365FinanceClient-standard__TotalRequestTimeout__Timeout")
            .WithEnvironmentFromConfiguration("Dynamics365FinanceClient-standard__CircuitBreaker__SamplingDuration");
    }
}