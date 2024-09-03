// <copyright file="DaprComponentHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using System.Diagnostics.CodeAnalysis;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Dapr;

using Hexalith.Application.Buses;
using Hexalith.Extensions.Helpers;

using AppHelper = HexalithDistributedApplicationHelper;

/// <summary>
/// Helper class for managing distributed applications in Hexalith.
/// </summary>
public static class DaprComponentHelper
{
    private static readonly Dictionary<string, IResourceBuilder<IDaprComponentResource>> _components = [];

    /// <summary>
    /// Adds a bus to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The name of the bus.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for the bus.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddBus(
        [NotNull] this IDistributedApplicationBuilder builder,
        [NotNull] string name,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return builder.GetOrAddDaprComponent(
            string.Empty,
            string.Empty,
            name,
            applicationName);
    }

    /// <summary>
    /// Adds a cron binding to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The name of the cron binding.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for the cron binding.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddCronBinding(
        [NotNull] this IDistributedApplicationBuilder builder,
        [NotNull] string name,
        string? applicationName = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return builder.GetOrAddDaprComponent(
            string.Empty,
            string.Empty,
            name,
            applicationName);
    }

    /// <summary>
    /// Adds Dapr commands to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for Dapr commands.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddDaprCommands(
        [NotNull] this IDistributedApplicationBuilder builder,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        CommandBusSettings settings = builder.Configuration.GetSettings<CommandBusSettings>();
        return builder.AddBus(settings.Name, applicationName);
    }

    /// <summary>
    /// Adds Dapr events to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for Dapr events.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddDaprEvents(
        [NotNull] this IDistributedApplicationBuilder builder,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        EventBusSettings settings = builder.Configuration.GetSettings<EventBusSettings>();
        return builder.AddBus(settings.Name, applicationName);
    }

    /// <summary>
    /// Adds Dapr notifications to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for Dapr notifications.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddDaprNotifications(
        [NotNull] this IDistributedApplicationBuilder builder,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        NotificationBusSettings settings = builder.Configuration.GetSettings<NotificationBusSettings>();
        return builder.AddBus(settings.Name, applicationName);
    }

    /// <summary>
    /// Adds Dapr requests to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for Dapr requests.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddDaprRequests(
        [NotNull] this IDistributedApplicationBuilder builder,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        RequestBusSettings settings = builder.Configuration.GetSettings<RequestBusSettings>();
        return builder.AddBus(settings.Name, applicationName);
    }

    /// <summary>
    /// Adds a queue binding to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The name of the queue binding.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for the queue binding.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddQueueBinding(
        [NotNull] this IDistributedApplicationBuilder builder,
        [NotNull] string name,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return builder.GetOrAddDaprComponent(
            string.Empty,
            string.Empty,
            name,
            applicationName);
    }

    /// <summary>
    /// Adds a secret store to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The name of the secret store.</param>
    /// <param name="applicationName">The application name of the bus.</param>
    /// <returns>The resource builder for the secret store.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddSecretStore(
        [NotNull] this IDistributedApplicationBuilder builder,
        [NotNull] string name,
        string? applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return builder.GetOrAddDaprComponent(
            string.Empty,
            string.Empty,
            name,
            applicationName);
    }

    /// <summary>
    /// Adds a state store to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="applicationName">The name of the application.</param>
    /// <returns>The resource builder for the state store.</returns>
    public static IResourceBuilder<IDaprComponentResource> AddStateStore(
        [NotNull] this IDistributedApplicationBuilder builder,
        [NotNull] string applicationName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        string name = AppHelper.NormalizeBicepResourceName($"{applicationName}statestore");
        return builder.GetOrAddDaprComponent(
            string.Empty,
            string.Empty,
            name,
            applicationName);
    }

    /// <summary>
    /// Gets or adds a Dapr component to the distributed application builder.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="publishType">The publish type of the component.</param>
    /// <param name="runType">The run type of the component.</param>
    /// <param name="name">The name of the component.</param>
    /// <param name="applicationName">The name of the application.</param>
    /// <returns>The resource builder for the Dapr component.</returns>
    public static IResourceBuilder<IDaprComponentResource> GetOrAddDaprComponent(
        this IDistributedApplicationBuilder builder,
        string publishType,
        string runType,
        string name,
        string? applicationName)
    {
        return GetOrAddComponent(name, () => builder
            .AddDaprComponent(
                name,
                builder.ExecutionContext.IsPublishMode ? publishType : runType,
                new DaprComponentOptions
                {
                    LocalPath = builder.GetComponentLocalPath(name, applicationName),
                }));
    }

    private static string GetComponentLocalPath(
        this IDistributedApplicationBuilder builder,
        string name,
        string? applicationName = null)
    {
        string subdirectory = builder.ExecutionContext.IsPublishMode
            ? AspireApplicationConstants.PublishPath
            : AspireApplicationConstants.RunPath;

        return applicationName == null
            ? Path.Combine(AppHelper.GetCommonComponentPath(builder), subdirectory, name + ".yaml")
            : Path.Combine(AppHelper.GetApplicationComponentPath(builder, applicationName), subdirectory, name + ".yaml");
    }

    private static IResourceBuilder<IDaprComponentResource> GetOrAddComponent(string name, Func<IResourceBuilder<IDaprComponentResource>> create)
    {
        string normalizedName = AppHelper.NormalizeBicepResourceName(name);
        if (_components.TryGetValue(normalizedName, out IResourceBuilder<IDaprComponentResource>? component))
        {
            return component;
        }

        IResourceBuilder<IDaprComponentResource> c = create();
        _components.Add(normalizedName, c);
        return c;
    }
}