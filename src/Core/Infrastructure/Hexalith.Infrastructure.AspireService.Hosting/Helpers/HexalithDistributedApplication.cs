// <copyright file="HexalithDistributedApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Dapr;

/// <summary>
/// Class HexalithProjectHelper.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HexalithDistributedApplication"/> class.
/// </remarks>
/// <param name="args">program args.</param>
public class HexalithDistributedApplication(string[] args)
{
    /// <summary>
    /// Gets the builder.
    /// </summary>
    /// <value>The builder.</value>
    public IDistributedApplicationBuilder Builder { get; } = DistributedApplication
            .CreateBuilder(args)
            .AddDapr(options => options.EnableTelemetry = true);

    // private IResourceBuilder<AzureKeyVaultResource> KeyVault
    //    => _keyVault
    //        ?? Builder
    //            .AddAzureKeyVault(Builder.Configuration.GetDeploymentName() + "secrets");

    /// <summary>
    /// Adds the project.
    /// </summary>
    /// <typeparam name="TProject">The type of the t project.</typeparam>
    /// <param name="projectName">Name of the project.</param>
    /// <returns>Aspire.Hosting.ApplicationModel.IResourceBuilder&lt;Aspire.Hosting.ApplicationModel.ProjectResource&gt;.</returns>
    public IResourceBuilder<ProjectResource> AddProject<TProject>(string projectName)
        where TProject : IProjectMetadata, new()
    {
        ArgumentNullException.ThrowIfNull(Builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectName);
        string name = projectName.ToLowerInvariant();
        IResourceBuilder<ProjectResource> project = Builder.AddProject<TProject>(name);
        string subFolder = Builder.ExecutionContext.IsPublishMode
            ? AspireApplicationConstants.PublishPath
            : AspireApplicationConstants.RunPath;
        string appId = project.GetAppId(projectName);
        project = project.WithDaprSidecar(new DaprSidecarOptions
        {
            AppId = appId,
            Config = Path.Combine(Builder.GetApplicationComponentPath(name), "config.yaml"),
            ResourcesPaths = [
                Path.Combine(Builder.GetApplicationComponentPath(name), subFolder),
                Path.Combine(Builder.GetCommonComponentPath(), subFolder),
            ],
        });

        string environmentName = Builder.GetEnvironmentName();
        project = project
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environmentName)
            .WithEnvironment("ASPNETCORE_DETAILEDERRORS", environmentName == "Production" ? "false" : "true")
            .WithEnvironment("ASPNETCORE_SHUTDOWNTIMEOUTSECONDS", "60")
            .WithEnvironment("Statestore__Name", name + "statestore")
            .WithEnvironmentFromConfiguration("Cosmos__ConnectionString")
            .WithEnvironmentFromConfiguration("Cosmos__DatabaseName")
            .WithSecretFromConfiguration("CommandBus__Password", false)
            .WithSecretFromConfiguration("RequestBus__Password", false)
            .WithSecretFromConfiguration("NotificationBus__Password", false)
            .WithSecretFromConfiguration("EventBus__Password", false);
        return project;
    }
}