// <copyright file="HexalithApplication.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
/// Application definition base class.
/// </summary>
public abstract class HexalithApplication : IApplication
{
    private static IClientApplication? _clientApplication;
    private static IServerApplication? _serverApplication;
    private static ISharedApplication? _sharedApplication;

    /// <summary>
    /// Gets the client application.
    /// </summary>
    public static IClientApplication Client
        => ClientApplication
            ?? throw new InvalidOperationException($"No client application found. Please add a class implementing {nameof(IClientApplication)}.");

    /// <summary>
    /// Gets the server application.
    /// </summary>
    public static IServerApplication Server
        => ServerApplication
            ?? throw new InvalidOperationException($"No server application found. Please check if you are server side or add a class implementing {nameof(IServerApplication)}.");

    /// <summary>
    /// Gets the shared application.
    /// </summary>
    public static ISharedApplication Shared
        => SharedApplication
            ?? throw new InvalidOperationException($"No shared application found. Please add a class implementing {nameof(ISharedApplication)}.");

    /// <inheritdoc/>
    public abstract string HomePath { get; }

    /// <inheritdoc/>
    public abstract string Id { get; }

    /// <inheritdoc/>
    public abstract bool IsClient { get; }

    /// <inheritdoc/>
    public abstract bool IsServer { get; }

    /// <inheritdoc/>
    public abstract string LoginPath { get; }

    /// <inheritdoc/>
    public abstract string LogoutPath { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> Modules { get; }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract string Version { get; }

    /// <inheritdoc/>
    IClientApplication IApplication.Client => Client;

    /// <inheritdoc/>
    IServerApplication IApplication.Server => Server;

    /// <inheritdoc/>
    ISharedApplication IApplication.Shared => Shared;

    /// <summary>
    /// Gets the client application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    private static IClientApplication? ClientApplication
        => _clientApplication ??= (ServerApplication is null)
                ? GetApplication<IClientApplication>() :
                (IClientApplication?)Activator.CreateInstance(Server.ClientApplicationType);

    /// <summary>
    /// Gets the client application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    private static IServerApplication? ServerApplication
        => _serverApplication ??= GetApplication<IServerApplication>();

    /// <summary>
    /// Gets the shared application.
    /// </summary>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">No application found.</exception>
    private static ISharedApplication? SharedApplication
        => _sharedApplication ??= (ServerApplication is null)
                ? (ClientApplication is null)
                    ? GetApplication<ISharedApplication>()
                    : (ISharedApplication?)Activator.CreateInstance(Client.SharedApplicationType)
                : (ISharedApplication?)Activator.CreateInstance(Server.SharedApplicationType);

    /// <summary>
    /// Adds client services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddClientServices(IServiceCollection services, IConfiguration configuration)
        => Client.AddServices(services, configuration);

    /// <summary>
    /// Adds server services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServerServices(IServiceCollection services, IConfiguration configuration)
        => Server.AddServices(services, configuration);

    /// <summary>
    /// Adds shared services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddSharedServices(IServiceCollection services, IConfiguration configuration)
        => Shared.AddServices(services, configuration);

    /// <summary>
    /// Gets the application of the specified type.
    /// </summary>
    /// <typeparam name="TApplication">The application type.</typeparam>
    /// <returns>The application instance.</returns>
    /// <exception cref="InvalidOperationException">Found more than one application.</exception>
    public static TApplication? GetApplication<TApplication>()
            where TApplication : IApplication
    {
        TApplication[] applications = [.. ReflectionHelper.GetInstantiableObjectsOf<TApplication>()];
        return applications.Length > 1
            ? throw new InvalidOperationException($"Found more than one application of type {nameof(TApplication)} : {string.Join("; ", applications.Select(p => p.GetType().Name))}.")
            : applications.FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        if (ServerApplication is not null)
        {
            services.TryAddSingleton<IApplication>(Server);
            services.TryAddSingleton(Server);
        }

        if (ClientApplication is not null)
        {
            services.TryAddSingleton<IApplication>(Client);
            services.TryAddSingleton(Client);
        }

        if (SharedApplication is not null)
        {
            services.TryAddSingleton(Shared);
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
}