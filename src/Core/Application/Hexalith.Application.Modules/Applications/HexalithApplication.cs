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
    private static IApiServerApplication? _apiServerApplication;
    private static ISharedUIElementsApplication? _sharedUIElementsApplication;
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

    // ?? throw new InvalidOperationException("No API server application implementation found. Ensure a class implementing IApiServerApplication exists and is accessible.");

    /// <summary>
    /// Gets the singleton instance of the shared assets application.
    /// </summary>
    /// <remarks>
    /// The instance is created based on the following priority:
    /// 1. If WebServerApplication exists, uses its SharedUIElementsApplicationType
    /// 2. If WebAppApplication exists, uses its SharedUIElementsApplicationType
    /// 3. Attempts to find a standalone implementation.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no shared assets application implementation can be found or instantiated.
    /// </exception>
    public static ISharedUIElementsApplication? SharedUIElementsApplication
    {
        get
        {
            if (_sharedUIElementsApplication is null)
            {
                if (WebServerApplication?.SharedUIElementsApplicationType is not null)
                {
                    _sharedUIElementsApplication = Activator.CreateInstance(WebServerApplication.SharedUIElementsApplicationType) as ISharedUIElementsApplication;
                }
                else if (WebAppApplication?.SharedUIElementsApplicationType is not null)
                {
                    _sharedUIElementsApplication = Activator.CreateInstance(WebAppApplication.SharedUIElementsApplicationType) as ISharedUIElementsApplication;
                }
            }

            return _sharedUIElementsApplication ??= GetApplication<ISharedUIElementsApplication>(); // ?? throw new InvalidOperationException("No shared assets application implementation found. Ensure a class implementing ISharedUIElementsApplication exists and is accessible.");
        }
    }

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

            return _webAppApplication ??= GetApplication<IWebAppApplication>(); // ?? throw new InvalidOperationException("No web app implementation found. Ensure a class implementing ISharedUIElementsApplication exists and is accessible.");
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
        => _webServerApplication ??= GetApplication<IWebServerApplication>(); // ?? throw new InvalidOperationException("No web server application implementation found. Ensure a class implementing IWebServerApplication exists and is accessible.");

    /// <inheritdoc/>
    IApiServerApplication? IApplication.ApiServerApplication => ApiServerApplication;

    /// <summary>
    /// Gets the application's home path.
    /// </summary>
    /// <value>
    /// The root URL path where the application is hosted.
    /// </value>
    public abstract string HomePath { get; }

    /// <summary>
    /// Gets the unique identifier for the application.
    /// </summary>
    /// <value>
    /// A string that uniquely identifies this application instance.
    /// </value>
    public abstract string Id { get; }

    /// <summary>
    /// Gets a value indicating whether this is a client-side application.
    /// </summary>
    /// <value>
    /// true if this is a client-side application; otherwise, false.
    /// </value>
    public abstract bool IsClient { get; }

    /// <summary>
    /// Gets a value indicating whether this is a server-side application.
    /// </summary>
    /// <value>
    /// true if this is a server-side application; otherwise, false.
    /// </value>
    public abstract bool IsServer { get; }

    /// <summary>
    /// Gets the login path for the application.
    /// </summary>
    /// <value>
    /// The URL path where users are redirected for authentication.
    /// </value>
    public abstract string LoginPath { get; }

    /// <summary>
    /// Gets the logout path for the application.
    /// </summary>
    /// <value>
    /// The URL path where users are redirected to end their session.
    /// </value>
    public abstract string LogoutPath { get; }

    /// <summary>
    /// Gets the collection of module types that this application supports.
    /// </summary>
    /// <value>
    /// An enumerable collection of Type objects representing the application modules.
    /// </value>
    public abstract IEnumerable<Type> Modules { get; }

    /// <summary>
    /// Gets the display name of the application.
    /// </summary>
    /// <value>
    /// A human-readable name for the application.
    /// </value>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the session cookie name for the application.
    /// </summary>
    /// <value>
    /// A string in the format ".{SharedUIElementsApplication.Id}.Session" used to identify the session cookie.
    /// </value>
    public string SessionCookieName => $".{SharedUIElementsApplication?.Id}.Session";

    /// <inheritdoc/>
    ISharedUIElementsApplication? IApplication.SharedUIElementsApplication => SharedUIElementsApplication;

    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    /// <value>
    /// A string representing the application's version number.
    /// </value>
    public abstract string Version { get; }

    /// <inheritdoc/>
    IWebAppApplication? IApplication.WebAppApplication => WebAppApplication;

    /// <inheritdoc/>
    IWebServerApplication? IApplication.WebServerApplication => WebServerApplication;

    /// <summary>
    /// Registers API server services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    /// <remarks>
    /// This method delegates the service registration to the ApiServerApplication instance.
    /// </remarks>
    public static void AddApiServerServices(IServiceCollection services, IConfiguration configuration)
        => ApiServerApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers shared assets services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    /// <remarks>
    /// This method delegates the service registration to the SharedUIElementsApplication instance.
    /// </remarks>
    public static void AddSharedUIElementsServices(IServiceCollection services, IConfiguration configuration)
        => SharedUIElementsApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers web application services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    /// <remarks>
    /// This method delegates the service registration to the WebAppApplication instance.
    /// </remarks>
    public static void AddWebAppServices(IServiceCollection services, IConfiguration configuration)
        => WebAppApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers web server services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    /// <remarks>
    /// This method delegates the service registration to the WebServerApplication instance.
    /// </remarks>
    public static void AddWebServerServices(IServiceCollection services, IConfiguration configuration)
        => WebServerApplication?.AddServices(services, configuration);

    /// <summary>
    /// Registers application services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration instance containing service settings.</param>
    /// <remarks>
    /// This method performs the following registrations:
    /// 1. Registers web server application if available
    /// 2. Registers web application if available
    /// 3. Registers shared assets application if available
    /// 4. Registers all application modules and their services.
    /// </remarks>
    public virtual void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        if (WebServerApplication is not null)
        {
            services.TryAddSingleton<IApplication>(WebServerApplication);
            services.TryAddSingleton(WebServerApplication);
        }

        if (WebAppApplication is not null)
        {
            services.TryAddSingleton<IApplication>(WebAppApplication);
            services.TryAddSingleton(WebAppApplication);
        }

        if (SharedUIElementsApplication is not null)
        {
            services.TryAddSingleton(SharedUIElementsApplication);
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
                    $"The services for module {module.Name} are not added. The module does not have the following static method:"
                    + $" {nameof(AddServices)}(IServiceCollection services, IConfiguration configuration).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [services, configuration]);
                Debug.WriteLine($"The services for module {module.Name} are have been added.");
            }
        }
    }

    /// <summary>
    /// Gets the application instance of the specified type.
    /// </summary>
    /// <typeparam name="TApplication">The type of application to retrieve.</typeparam>
    /// <returns>An instance of the specified application type, or null if not found.</returns>
    /// <remarks>
    /// This method uses reflection to find and instantiate application implementations.
    /// Only one implementation of each application type should exist in the application domain.
    /// </remarks>
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