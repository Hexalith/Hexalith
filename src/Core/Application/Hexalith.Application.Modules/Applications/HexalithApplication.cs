// <copyright file="HexalithApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.Extensions.Reflections;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Represents the base class for Hexalith applications, providing core functionality for application configuration,
/// service registration, and module management. This abstract class serves as a foundation for different types of
/// applications within the Hexalith framework, including API servers, web applications, and shared assets.
/// </summary>
public abstract class HexalithApplication : IApplication
{
    private static readonly ISharedUIElementsApplication? _sharedUIElementsApplication;
    private static IApiServerApplication? _apiServerApplication;
    private static IWebAppApplication? _webAppApplication;
    private static IWebServerApplication? _webServerApplication;

    /// <summary>
    /// Gets the singleton instance of the API server application.
    /// </summary>
    /// <remarks>
    /// The instance is created lazily upon first access. If no implementation is found,
    /// an InvalidOperationException is thrown.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no API server application implementation is found in the application domain.
    /// </exception>
    public static IApiServerApplication? ApiServerApplication
        => _apiServerApplication ??= GetApplication<IApiServerApplication>();

    /// <summary>
    /// Gets the singleton instance of the web application.
    /// </summary>
    /// <remarks>
    /// The instance is created based on the following priority:
    /// 1. If WebServerApplication exists, uses its WebAppApplicationType
    /// 2. Attempts to find a standalone implementation.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no web application implementation can be found or instantiated.
    /// </exception>
    public static IWebAppApplication? WebAppApplication
    {
        get
        {
            _webAppApplication ??= WebServerApplication?.WebAppApplicationType is not null
                ? Activator.CreateInstance(WebServerApplication.WebAppApplicationType) as IWebAppApplication
                : GetApplication<IWebAppApplication>();

            return _webAppApplication ??= GetApplication<IWebAppApplication>();
        }
    }

    /// <summary>
    /// Gets the singleton instance of the web server application.
    /// </summary>
    /// <remarks>
    /// The instance is created lazily upon first access. If no implementation is found,
    /// an InvalidOperationException is thrown.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no web server application implementation is found in the application domain.
    /// </exception>
    public static IWebServerApplication? WebServerApplication
        => _webServerApplication ??= GetApplication<IWebServerApplication>();

    /// <inheritdoc/>
    IApiServerApplication? IApplication.ApiServerApplication => ApiServerApplication;

    /// <inheritdoc/>
    public abstract ApplicationType ApplicationType { get; }

    /// <inheritdoc/>
    public virtual string Description => $"{Name} {ApplicationType} application";

    /// <inheritdoc/>
    public virtual string HomePath => ShortName;

    /// <inheritdoc/>
    public abstract string Id { get; }

    /// <inheritdoc/>
    public virtual string LoginPath => ".auth/login";

    /// <inheritdoc/>
    public virtual string LogoutPath => ".auth/logout";

    /// <inheritdoc/>
    public abstract IEnumerable<Type> Modules { get; }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public virtual string SessionCookieName => $".{Id}.Session";

    /// <inheritdoc/>
    public abstract string ShortName { get; }

    /// <inheritdoc/>
    public virtual string Version => "1.0";

    /// <inheritdoc/>
    IWebAppApplication? IApplication.WebAppApplication => WebAppApplication;

    /// <inheritdoc/>
    IWebServerApplication? IApplication.WebServerApplication => WebServerApplication;

    /// <summary>
    /// Registers API server services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    public static void AddApiServerServices(IServiceCollection services, IConfiguration configuration)
        => ApiServerApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers web application services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    public static void AddWebAppServices(IServiceCollection services, IConfiguration configuration)
        => WebAppApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers web server services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    public static void AddWebServerServices(IServiceCollection services, IConfiguration configuration)
        => WebServerApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers application services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    public virtual void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        if (WebServerApplication is not null)
        {
            services.TryAddSingleton<IApplication>(WebServerApplication);
            services.TryAddSingleton(WebServerApplication);
        }
        else if (WebAppApplication is not null)
        {
            services.TryAddSingleton<IApplication>(WebAppApplication);
            services.TryAddSingleton(WebAppApplication);
        }

        if (ApiServerApplication is not null)
        {
            services.TryAddSingleton<IApplication>(ApiServerApplication);
            services.TryAddSingleton(ApiServerApplication);
        }

        foreach (Type module in Modules)
        {
            _ = services.AddScoped(typeof(IApplicationModule), module);
            MethodInfo? moduleMethod = module.GetMethod(
                nameof(AddServices),
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(IServiceCollection), typeof(IConfiguration)],
                null);
            if (moduleMethod == null)
            {
                Debug.WriteLine(
                    $"The services for module {module.Name} are not added. The module does not have the following static method:" +
                    $" {nameof(AddServices)}(IServiceCollection services, IConfiguration configuration).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [services, configuration]);
                Debug.WriteLine($"The services for module {module.Name} have been added.");
            }
        }
    }

    /// <summary>
    /// Gets the application instance of the specified type.
    /// </summary>
    /// <typeparam name="TApplication">The type of application to retrieve.</typeparam>
    /// <returns>An instance of the specified application type, or null if not found.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when multiple implementations of the specified application type are found.
    /// </exception>
    private static TApplication? GetApplication<TApplication>()
        where TApplication : IApplication
    {
        TApplication[] applications = [.. ReflectionHelper.GetInstantiableObjectsOf<TApplication>()];
        return applications.Length > 1
            ? throw new InvalidOperationException($"Multiple implementations of {typeof(TApplication).Name} found: {string.Join("; ", applications.Select(p => p.GetType().Name))}. Only one implementation is allowed.")
            : applications.FirstOrDefault();
    }
}