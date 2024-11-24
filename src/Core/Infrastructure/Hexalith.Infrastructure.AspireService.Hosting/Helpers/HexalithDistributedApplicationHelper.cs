// <copyright file="HexalithDistributedApplicationHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Helper class for managing distributed applications in Hexalith.
/// </summary>
public static class HexalithDistributedApplicationHelper
{
    /// <summary>
    /// Get application identifier.
    /// </summary>
    /// <param name="project">The project.</param>
    /// <param name="defaultAppId">The default application identifier.</param>
    /// <returns>The application identifier if exists in the configuration, else the default identifier.</returns>
    public static string GetAppId([NotNull] this IResourceBuilder<ProjectResource> project, string defaultAppId)
    {
        ArgumentNullException.ThrowIfNull(project);
        string name = project.Resource.Name;
        return project
                .ApplicationBuilder
                .Configuration
                .GetValue<string?>($"{project.ApplicationBuilder.GetEnvironmentName()}:{name}:AppId") ?? defaultAppId;
    }

    /// <summary>
    /// Gets the application component path.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the builder is null.</exception>
    public static string GetApplicationComponentPath(this IDistributedApplicationBuilder builder, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return Path.Combine(builder.Environment.ContentRootPath, "./Components", applicationName, builder.GetEnvironmentName());
    }

    /// <summary>
    /// Gets the common component path.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the builder is null.</exception>
    public static string GetCommonComponentPath([NotNull] this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return Path.Combine(builder.Environment.ContentRootPath, "./Components/Common", builder.GetEnvironmentName());
    }

    /// <summary>
    /// Gets the database server name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The database name.</returns>
    public static string GetDatabaseName([NotNull] this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.GetEnvironmentName();
    }

    /// <summary>
    /// Gets the database server name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The database server name.</returns>
    public static string GetDatabaseServerName([NotNull] this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        string name = builder.GetEnvironmentName();
        return builder.Configuration.GetDeploymentPrefix() + (name == "production" ? name : "staging");
    }

    /// <summary>
    /// Gets the deployment prefix.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The deployment prefix.</returns>
    public static string GetDeploymentPrefix([NotNull] this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.GetValue<string>("DeploymentPrefix") ?? throw new InvalidOperationException("Setting 'DeploymentPrefix' not defined.");
    }

    /// <summary>
    /// Gets the environment name.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The environment name.</returns>
    public static string GetEnvironmentName([NotNull] this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return (
            builder.ExecutionContext.IsRunMode
                ? builder.Environment.EnvironmentName
                : builder.Configuration.GetValue<string>("DeploymentName")
                    ?? throw new InvalidOperationException("'DeploymentName' setting is empty. Set the value with the environment name (Development, Staging, Production, etc.)"))
                    .ToLowerInvariant();
    }

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
        return app.Builder.ExecutionContext.IsPublishMode
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
    /// Normalizes the Bicep resource name.
    /// </summary>
    /// <param name="name">The resource name.</param>
    /// <returns>The normalized resource name.</returns>
    public static string NormalizeBicepResourceName(this string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return name.Replace(
                "-",
                string.Empty,
                StringComparison.InvariantCultureIgnoreCase)
            .ToLowerInvariant();
    }

    /// <summary>
    /// Adds environment settings to the project resource.
    /// </summary>
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
        string settingName = (project.ApplicationBuilder.GetEnvironmentName() + "__" + name).Replace("__", ":", StringComparison.OrdinalIgnoreCase);
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
    /// Adds environment settings to the project resource.
    /// </summary>
    /// <param name="project">The project resource builder.</param>
    /// <param name="name">The name of the environment setting.</param>
    /// <param name="required">Indicates whether the setting is required.</param>
    /// <returns>The project resource builder reference.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the project is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the name is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the setting is required but not found.</exception>
    public static IResourceBuilder<ProjectResource> WithSecretFromConfiguration(this IResourceBuilder<ProjectResource> project, string name, bool required = true)
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        string settingName = (project.ApplicationBuilder.GetEnvironmentName() + "__" + name).Replace("__", ":", StringComparison.OrdinalIgnoreCase);
        string? value = project.ApplicationBuilder.Configuration[settingName];
        return string.IsNullOrWhiteSpace(value)
            ? required ? throw new InvalidOperationException($"The setting {settingName} is required.") : project
            : project.WithEnvironment("SEC_" + name, value);
    }
}