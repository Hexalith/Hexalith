// <copyright file="AspireProjectHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Helper class for managing distributed applications in Hexalith.
/// </summary>
public static class HexalithDistributedApplicationHelper
{
    /// <summary>
    /// The constant representing the Aspire Launch setting.
    /// </summary>
    public const string AspireDeploy = "ASPIRE_DEPLOY";

    /// <summary>
    /// Checks if the specified project is enabled.
    /// </summary>
    /// <typeparam name="T">The type of the project resource.</typeparam>
    /// <param name="app">The HexalithDistributedApplication instance.</param>
    /// <returns>True if the project is enabled, otherwise false.</returns>
    public static bool IsProjectEnabled<T>(this HexalithDistributedApplication app)
        where T : IProjectMetadata
    {
        ArgumentNullException.ThrowIfNull(app);
        string name = typeof(T).Name;
        return app
            .Builder
            .Configuration
            .GetValue<bool>(AspireDeploy)
            ? !app
                .Builder
                .Configuration
                .GetValue<bool>($"{app.Builder.Environment.ApplicationName}:{name}:DeployDisabled")
            : !app
                .Builder
                .Configuration
                .GetValue<bool>($"{app.Builder.Environment.ApplicationName}:{name}:ExecuteDisabled");
    }

    /// <summary>
    /// Adds Bergen environment settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <returns>The project resource builder reference.</returns>
    public static IResourceBuilder<ProjectResource> WithBergen(this IResourceBuilder<ProjectResource> project)
    {
        return project
            .WithEnvironmentFromConfiguration("Bergen__ServiceUrl")
            .WithEnvironmentFromConfiguration("Bergen__Domain")
            .WithEnvironmentFromConfiguration("Bergen__UserName")
            .WithEnvironmentFromConfiguration("Bergen__Password")
            .WithEnvironmentFromConfiguration("Bergen__TokenLifetimeSeconds")
            .WithEnvironmentFromConfiguration("Bergen__EnableNonSecureHttp");
    }

    /// <summary>
    /// Adds BSPK environment settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <returns>The project resource builder reference.</returns>
    public static IResourceBuilder<ProjectResource> WithBspk(this IResourceBuilder<ProjectResource> project)
    {
        return project
            .WithEnvironmentFromConfiguration("Bspk__BspkCompanyName")
            .WithEnvironmentFromConfiguration("Bspk__AwsBucketName")
            .WithEnvironmentFromConfiguration("Bspk__AwsRegion")
            .WithEnvironmentFromConfiguration("Bspk__AlternateOriginId")
            .WithEnvironmentFromConfiguration("Bspk__BspkApiUrl")
            .WithEnvironmentFromConfiguration("Bspk__AwsAccessKey")
            .WithEnvironmentFromConfiguration("Bspk__AwsSecretKey")
            .WithEnvironmentFromConfiguration("Bspk__BspkApiSecret")
            .WithEnvironmentFromConfiguration("Bspk__BspkBearerToken")
            .WithEnvironmentFromConfiguration("Bspk__ApimSubscriptionKey");
    }

    /// <summary>
    /// Adds Dynamics 365 Finance environment settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <returns>The project resource builder reference.</returns>
    public static IResourceBuilder<ProjectResource> WithDynamics365Finance(this IResourceBuilder<ProjectResource> project)
    {
        return project
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Instance")
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Identity__Tenant")
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Identity__ApplicationId")
            .WithEnvironmentFromConfiguration("Dynamics365Finance__Identity__ApplicationSecret")
            .WithEnvironmentFromConfiguration("Dynamics365FinanceClient-standard__AttemptTimeout__Timeout")
            .WithEnvironmentFromConfiguration("Dynamics365FinanceClient-standard__TotalRequestTimeout__Timeout")
            .WithEnvironmentFromConfiguration("Dynamics365FinanceClient-standard__CircuitBreaker__SamplingDuration");
    }

    /// <summary>
    /// Adds environment settings to the project resource.
    /// </summary>
    /// <typeparam name="TType">The type of the project resource.</typeparam>
    /// <param name="project">The project resource builder.</param>
    /// <param name="name">The name of the environment setting.</param>
    /// <param name="required">Indicates whether the setting is required.</param>
    /// <returns>The project resource builder reference.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the project is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the name is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the setting is required but not found.</exception>
    public static IResourceBuilder<ProjectResource> WithEnvironmentFromConfiguration(this IResourceBuilder<ProjectResource> project, string name, bool required = true)
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        string settingName = (project.ApplicationBuilder.Environment.EnvironmentName + "__" + name).Replace("__", ":", StringComparison.OrdinalIgnoreCase);
        string? value = project.ApplicationBuilder.Configuration[settingName];
        return string.IsNullOrWhiteSpace(value)
            ? required ? throw new InvalidOperationException($"The setting {settingName} is required.") : project
            : project.WithEnvironment(name, value);
    }

    /// <summary>
    /// Adds organization settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <param name="partitionId">The default partition ID.</param>
    /// <param name="companyId">The default company ID.</param>
    /// <param name="originId">The default origin ID.</param>
    /// <returns>The project resource builder reference.</returns>
    public static IResourceBuilder<ProjectResource> WithOrganization(this IResourceBuilder<ProjectResource> project, string partitionId, string companyId, string originId)
    {
        return project
            .WithEnvironment("Organization__DefaultPartitionId", partitionId)
            .WithEnvironment("Organization__DefaultCompanyId", companyId)
            .WithEnvironment("Organization__DefaultOriginId", originId);
    }

    /// <summary>
    /// Adds Spring environment settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <returns>The project resource builder reference.</returns>
    public static IResourceBuilder<ProjectResource> WithSpring(this IResourceBuilder<ProjectResource> project)
    {
        return project
            .WithEnvironmentFromConfiguration("Spring__Instance")
            .WithEnvironmentFromConfiguration("Spring__ApplicationId")
            .WithEnvironmentFromConfiguration("Spring__ApplicationSecret");
    }
}