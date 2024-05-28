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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Manages the application modules.
/// </summary>
public partial class ModuleManager
{
    private static IEnumerable<Type>? _clientModuleTypes;
    private static IEnumerable<Type>? _serverModuleTypes;
    private static IEnumerable<Type>? _sharedModuleTypes;
    private static IEnumerable<Type>? _storeAppModuleTypes;

    private readonly string _homePageModule;
    private readonly ILogger _logger;

    private IDictionary<string, IClientApplicationModule>? _clientModules;
    private string? _homeRoute;
    private IDictionary<string, IServerApplicationModule>? _serverModules;
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
        => ClientModules
        .SelectMany(p => p.Value.PresentationAssemblies)
        .ToImmutableList();

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
    public IEnumerable<Assembly> ServerPresentationAssemblies => ServerModules
        .Values
        .SelectMany(p => p.PresentationAssemblies)
        .ToImmutableList();

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

    /// <summary>
    /// Gets the collection of server presentation assemblies.
    /// </summary>
    public IEnumerable<Assembly> StoreAppPresentationAssemblies => StoreAppModules
        .Values
        .SelectMany(p => p.PresentationAssemblies)
        .ToImmutableList();

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

        if (type.IsAbstract || type.GetConstructor(Type.EmptyTypes)?.IsPublic != true)
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
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            // try
            // {
            modules.AddRange(assembly.GetTypes().Where(IsModule<TModule>));

            // }
            // catch (ReflectionTypeLoadException)
            // {
            //    // ignore types that cannot be loaded

            // }
        }

        return [.. modules];
    }
}