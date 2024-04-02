// <copyright file="HexalithDistributedApplication.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using Aspire.Hosting;
using Aspire.Hosting.Azure;
using Aspire.Hosting.Dapr;

using Hexalith.Infrastructure.AspireService.Hosting;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Class HexalithProjectHelper.
/// </summary>
public class HexalithDistributedApplication
{
    private readonly IResourceBuilder<AzureApplicationInsightsResource>? _applicationInsights;
    private readonly string _deploymentName;
    private readonly IResourceBuilder<AzureKeyVaultResource>? _keyVault;
    private IResourceBuilder<IDaprComponentResource>? _commandBus;
    private IResourceBuilder<IDaprComponentResource>? _eventBus;
    private IResourceBuilder<IDaprComponentResource>? _notificationBus;
    private IResourceBuilder<IDaprComponentResource>? _requestBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="HexalithDistributedApplication"/> class.
    /// </summary>
    /// <param name="args">program args.</param>
    public HexalithDistributedApplication(string[] args)
    {
        Builder = DistributedApplication
            .CreateBuilder(args)
            .AddDapr(options => options.EnableTelemetry = true);
        _deploymentName = Builder.Configuration.GetSection(AspireHostConstants.DeploymentName).Value
            ?? throw new InvalidOperationException($"{AspireHostConstants.DeploymentName} variable not defined in settings.");
    }

    /// <summary>
    /// Gets the builder.
    /// </summary>
    /// <value>The builder.</value>
    public IDistributedApplicationBuilder Builder { get; }

    private IResourceBuilder<AzureApplicationInsightsResource> ApplicationInsights
        => _applicationInsights
            ?? Builder
                .AddAzureApplicationInsights(_deploymentName + "-telemetry");

    private IResourceBuilder<AzureKeyVaultResource> KeyVault
        => _keyVault
            ?? Builder
                .AddAzureKeyVault(_deploymentName + "-secrets");

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
#pragma warning disable CA1308 // Normalize strings to uppercase
        string name = projectName.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
        IResourceBuilder<ProjectResource> project = Builder.AddProject<TProject>(_deploymentName + '-' + name);
        string statestore = name + "-statestore";
        if (Builder.Environment.IsDevelopment())
        {
            project = project.WithDaprSidecar(name)
                .WithReference(_eventBus ??= Builder.AddDaprPubSub(AspireHostConstants.EventBusComponentName))
                .WithReference(_requestBus ??= Builder.AddDaprPubSub(AspireHostConstants.RequestBusComponentName))
                .WithReference(_commandBus ??= Builder.AddDaprPubSub(AspireHostConstants.CommandBusComponentName))
                .WithReference(_notificationBus ??= Builder.AddDaprPubSub(AspireHostConstants.NotificationBusComponentName))
                .WithReference(Builder.AddDaprStateStore(statestore));
        }
        else
        {
            string commonComponentPath = Path.Combine(Builder.Environment.ContentRootPath, "Components", "Common", Builder.Environment.EnvironmentName);
            string projectComponentPath = Path.Combine(Builder.Environment.ContentRootPath, "Components", name, Builder.Environment.EnvironmentName);

            project = project.WithDaprSidecar(new DaprSidecarOptions { AppId = name, ResourcesPaths = [commonComponentPath, projectComponentPath] });

            if (Builder.ExecutionContext.IsPublishMode)
            {
                _ = project.WithReference(ApplicationInsights);
                _ = project.WithReference(KeyVault);
            }
        }

        project = project
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", Builder.Environment.EnvironmentName)
            .WithEnvironment("ASPNETCORE_DETAILEDERRORS", Builder.Environment.IsProduction() ? "false" : "true")
            .WithEnvironment("ASPNETCORE_SHUTDOWNTIMEOUTSECONDS", "60")
            .WithEnvironment("Statestore__Name", statestore);
        _ = project
            .WithEnvironmentFromConfiguration("Cosmos__ConnectionString")
            .WithEnvironmentFromConfiguration("Cosmos__DatabaseName");
        return project;
    }
}