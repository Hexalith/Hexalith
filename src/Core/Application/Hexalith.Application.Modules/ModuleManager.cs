// <copyright file="ModuleManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Hexalith.Application.Modules.Configurations;
using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Modules.Routes;
using Hexalith.Extensions.Configuration;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Manages the application modules.
/// </summary>
public partial class ModuleManager
{
    private static Assembly[]? _applicationAssemblies;
    private static IEnumerable<Type>? _clientModuleTypes;
    private static IEnumerable<Type>? _serverModuleTypes;
    private static IEnumerable<Type>? _sharedModuleTypes;
    private static IEnumerable<Type>? _storeAppModuleTypes;

    private readonly string _homePageModule;
    private readonly ILogger _logger;
    private IDictionary<string, IClientApplicationModule>? _clientModules;
    private IEnumerable<Assembly>? _clientPresentationAssemblies;
    private string? _homeRoute;
    private IDictionary<string, IServerApplicationModule>? _serverModules;
    private IEnumerable<Assembly>? _serverPresentationAssemblies;
    private IDictionary<string, ISharedApplicationModule>? _sharedModules;
    private IDictionary<string, IStoreAppApplicationModule>? _storeAppModules;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleManager"/> class.
    /// </summary>
    /// <param name="routes">The collection of route providers.</param>
    /// <param name="options">The module settings options.</param>
    /// <param name="logger">The logger.</param>
    public ModuleManager(
        IEnumerable<IRouteProvider> routes,
        IOptions<ModuleSettings> options,
        ILogger<ModuleManager> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(routes);
        ArgumentNullException.ThrowIfNull(logger);
        Routes = routes;
        _logger = logger;
        SettingsException<ModuleSettings>.ThrowIfNullOrWhiteSpace(options.Value.HomePageModule);
        _homePageModule = options.Value.HomePageModule;
    }

    /// <summary>
    /// Gets the application module types.
    /// </summary>
    /// <value>The module types.</value>
    public static IEnumerable<Type> ClientModuleTypes
        => _clientModuleTypes ??= GetModuleTypesFromReflection<IClientApplicationModule>();

    /// <summary>
    /// Gets the application module types.
    /// </summary>
    /// <value>The module types.</value>
    public static IEnumerable<Type> ServerModuleTypes
        => _serverModuleTypes ??= GetModuleTypesFromReflection<IServerApplicationModule>();

    /// <summary>
    /// Gets the application module types.
    /// </summary>
    /// <value>The module types.</value>
    public static IEnumerable<Type> SharedModuleTypes
        => _sharedModuleTypes ??= GetModuleTypesFromReflection<ISharedApplicationModule>();

    /// <summary>
    /// Gets the application module types.
    /// </summary>
    /// <value>The module types.</value>
    public static IEnumerable<Type> StoreAppModuleTypes
        => _storeAppModuleTypes ??= GetModuleTypesFromReflection<IStoreAppApplicationModule>();

    /// <summary>
    /// Gets the client application module types.
    /// </summary>
    /// <value>The module types.</value>
    public IDictionary<string, IClientApplicationModule> ClientModules
        => _clientModules ??= ClientModuleTypes
        .Select(GetModule<IClientApplicationModule>)
        .ToImmutableDictionary(p => p.Id);

    /// <summary>
    /// Gets the collection of client presentation assemblies.
    /// </summary>
    public IEnumerable<Assembly> ClientPresentationAssemblies
        => _clientPresentationAssemblies ??= [.. ClientModules
        .SelectMany(p => p.Value.PresentationAssemblies)
        .Union(SharedModules.SelectMany(p => p.Value.PresentationAssemblies))
        .Distinct()
        .OrderBy(p => p.GetName().Name)];

    /// <summary>
    /// Gets the home page module.
    /// </summary>
    public IApplicationModule HomePageModule
    {
        get
        {
            IApplicationModule module = SharedModules
                .FirstOrDefault(p => p.Key.Equals(_homePageModule, StringComparison.OrdinalIgnoreCase))
                .Value;
            if (module != null)
            {
                return module;
            }

            module = ClientModules
                .FirstOrDefault(p => p.Key.Equals(_homePageModule, StringComparison.OrdinalIgnoreCase))
                .Value;
            if (module != null)
            {
                return module;
            }

            module = ServerModules
                .FirstOrDefault(p => p.Key.Equals(_homePageModule, StringComparison.OrdinalIgnoreCase))
                .Value;
            return module ?? throw new InvalidOperationException("Home page route not defined.");
        }
    }

    /// <summary>
    /// Gets the home route.
    /// </summary>
    public string HomeRoute
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_homeRoute))
            {
                _homeRoute = HomePageModule.Path;
            }

            return _homeRoute;
        }
    }

    /// <summary>
    /// Gets the collection of route providers.
    /// </summary>
    public IEnumerable<IRouteProvider> Routes { get; }

    /// <summary>
    /// Gets the server application module types.
    /// </summary>
    /// <value>The module types.</value>
    public IDictionary<string, IServerApplicationModule> ServerModules
        => _serverModules ??= ServerModuleTypes
        .Select(GetModule<IServerApplicationModule>)
        .ToImmutableDictionary(p => p.Id);

    /// <summary>
    /// Gets the collection of server presentation assemblies.
    /// </summary>
    public IEnumerable<Assembly> ServerPresentationAssemblies
        => _serverPresentationAssemblies ??= [.. ServerModules
        .SelectMany(p => p.Value.PresentationAssemblies)
        .Union(ClientPresentationAssemblies)
        .Distinct()
        .OrderBy(p => p.GetName().Name)];

    /// <summary>
    /// Gets the shared application module types.
    /// </summary>
    /// <value>The module types.</value>
    public IDictionary<string, ISharedApplicationModule> SharedModules
        => _sharedModules ??= SharedModuleTypes
        .Select(GetModule<ISharedApplicationModule>)
        .ToImmutableDictionary(p => p.Id);

    /// <summary>
    /// Gets the store application module types.
    /// </summary>
    /// <value>The module types.</value>
    public IDictionary<string, IStoreAppApplicationModule> StoreAppModules
        => _storeAppModules ??= StoreAppModuleTypes
        .Select(GetModule<IStoreAppApplicationModule>)
        .ToImmutableDictionary(p => p.Id);

    private static Assembly[] ApplicationAssemblies => _applicationAssemblies ??= LoadAssemblies();

    /// <summary>
    /// Add the client modules services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    public static void AddClientModulesServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (Type type in ClientModuleTypes)
        {
            MethodInfo? moduleMethod = type.GetMethod(
                nameof(AddClientModulesServices),
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(IServiceCollection), typeof(IConfiguration)],
                null);
            if (moduleMethod == null)
            {
                Debug.WriteLine(
                    $"The services for module {type.Name} are not added. The module does not have the following static method:"
                    + $" {nameof(AddClientModulesServices)}(IServiceCollection services, IConfiguration configuration).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [services, configuration]);
                Debug.WriteLine($"The services for module {type.Name} are have been added.");
            }
        }
    }

    /// <summary>
    /// Add the Server modules services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    public static void AddServerModulesServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (Type type in ServerModuleTypes)
        {
            MethodInfo? moduleMethod = type.GetMethod(
                nameof(AddServerModulesServices),
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(IServiceCollection), typeof(IConfiguration)],
                null);
            if (moduleMethod == null)
            {
                Debug.WriteLine(
                    $"The services for module {type.Name} are not added. The module does not have the following static method:"
                    + $" {nameof(AddServerModulesServices)}(IServiceCollection services, IConfiguration configuration).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [services, configuration]);
                Debug.WriteLine($"The services for module {type.Name} are have been added.");
            }
        }
    }

    /// <summary>
    /// Add the shared modules services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    public static void AddSharedModulesServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (Type type in SharedModuleTypes)
        {
            MethodInfo? moduleMethod = type.GetMethod(
                nameof(AddSharedModulesServices),
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(IServiceCollection), typeof(IConfiguration)],
                null);
            if (moduleMethod == null)
            {
                Debug.WriteLine(
                    $"The services for module {type.Name} are not added. The module does not have the following static method:"
                    + $" {nameof(AddSharedModulesServices)}(IServiceCollection services, IConfiguration configuration).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [services, configuration]);
                Debug.WriteLine($"The services for module {type.Name} are have been added.");
            }
        }
    }

    /// <summary>
    /// Add the StoreApp modules services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    public static void AddStoreAppModulesServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (Type type in StoreAppModuleTypes)
        {
            MethodInfo? moduleMethod = type.GetMethod(
                nameof(AddStoreAppModulesServices),
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(IServiceCollection), typeof(IConfiguration)],
                null);
            if (moduleMethod == null)
            {
                Debug.WriteLine(
                    $"The services for module {type.Name} are not added. The module does not have the following static method:"
                    + $" {nameof(AddStoreAppModulesServices)}(IServiceCollection services, IConfiguration configuration).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [services, configuration]);
                Debug.WriteLine($"The services for module {type.Name} are have been added.");
            }
        }
    }

    /// <summary>
    /// Gets the store application module types.
    /// </summary>
    /// <typeparam name="TModule">The module type.</typeparam>
    /// <param name="type">The type to check if it's a module.</param>
    /// <returns>The module.</returns>
    public static TModule GetModule<TModule>(Type type)
        where TModule : IApplicationModule
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!typeof(TModule).IsAssignableFrom(type) || !type.IsClass)
        {
            // This not a module type
            throw new InvalidOperationException($"Type '{type.Name}' is not a valid module.");
        }
        else
        {
            return type.IsAbstract || type.GetConstructor(Type.EmptyTypes)?.IsPublic != true
            ? throw new InvalidOperationException($"Type '{type.Name}' is not a valid module. It may be abstract or lack a parameterless public constructor.")
            : (TModule)(Activator.CreateInstance(type)
            ?? throw new InvalidOperationException($"Type '{type.Name}' is not a valid module."));
        }
    }

    /// <summary>
    /// Checks if the specified type is a valid module.
    /// </summary>
    /// <typeparam name="TModule">The module type.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type is a valid module; otherwise, <c>false</c>.</returns>
    public static bool IsModule<TModule>([NotNull] Type type)
        where TModule : IApplicationModule
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!typeof(TModule).IsAssignableFrom(type) || !type.IsClass)
        {
            return false;
        }

        ConstructorInfo? constructor = type.GetConstructor(Type.EmptyTypes);
        if (type.IsAbstract || constructor == null || !constructor.IsPublic)
        {
            Debug.WriteLine($"Application module '{type.Name}' ignored. It may be abstract or lack a parameterless public constructor.");
            return false;
        }

        Debug.WriteLine($"Application module '{type.Name}' added.");

        return true;
    }

    /// <summary>
    /// Logs the debug information about the module list.
    /// </summary>
    /// <param name="moduleCount">The number of modules.</param>
    /// <param name="moduleList">The list of module IDs.</param>
    [LoggerMessage(LogLevel.Debug, Message = "Found {ModuleCount} application modules: {ModuleList}.")]
    public partial void LogModuleListDebugInformation(int moduleCount, string moduleList);

    /// <summary>
    /// Gets the module types from reflection.
    /// </summary>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Type&gt;.</returns>
    private static ImmutableList<Type> GetModuleTypesFromReflection<TModule>()
        where TModule : IApplicationModule
    {
        List<Type> modules = [];
        foreach (Assembly assembly in ApplicationAssemblies)
        {
            try
            {
                modules.AddRange(assembly.GetTypes().Where(IsModule<TModule>));
            }
            catch (ReflectionTypeLoadException ex)
            {
                Debug.WriteLine($"{ex.FullMessage}");
            }
        }

        return [.. modules];
    }

    private static Assembly[] LoadAssemblies()
    {
        // Get the application base directory
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Get all DLL files in the base directory
        string[] dllFiles = Directory.GetFiles(baseDirectory, "*.dll");

        // Load each DLL file
        foreach (string dll in dllFiles)
        {
            try
            {
                Assembly loadedAssembly = Assembly.LoadFrom(dll);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading assembly {dll}: {ex.Message}");
            }
        }

        // List all assemblies currently loaded in the AppDomain
        return [.. AppDomain.CurrentDomain.GetAssemblies().OrderBy(p => p.FullName)];
    }
}