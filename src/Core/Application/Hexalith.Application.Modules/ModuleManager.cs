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
using System.Linq;
using System.Reflection;

using Hexalith.Application.Modules.Modules;

/// <summary>
/// Manages the application modules.
/// </summary>
public static class ModuleManager
{
    private static IDictionary<string, IApplicationModule>? _modules;

    /// <summary>
    /// Gets the collection of application modules.
    /// </summary>
    public static IDictionary<string, IApplicationModule> Modules
    {
        get
        {
            if (_modules != null)
            {
                return _modules;
            }

            Dictionary<string, IApplicationModule> modules = [];
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
            {
                try
                {
                    foreach (Type type in assembly.GetTypes().Where(IsModule<IApplicationModule>))
                    {
                        modules.AddModule(type);
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // ignore types that cannot be loaded
                }
            }

            return _modules = modules.ToImmutableDictionary();
        }
    }

    private static void AddModule(this Dictionary<string, IApplicationModule> modules, Type type)
    {
        // create an instance of the module
        IApplicationModule module = (IApplicationModule)(
            Activator.CreateInstance(type)
                ?? throw new InvalidOperationException($"Could not create an instance of {type.Name} ({type.FullName})."));
        if (!modules.TryAdd(module.Id, module))
        {
            throw new InvalidOperationException($"A module with the name {module.Name} already exists.");
        }

        Trace.TraceInformation($"Application module {module.Name} (Id:{module.Id};Type:{module.ModuleType}).");
    }

    private static bool IsModule<TModule>(Type type)
    {
        return type.IsClass
            && !type.IsAbstract
            && typeof(TModule).IsAssignableFrom(type)
            && type.GetConstructor(Type.EmptyTypes)?.IsPublic == true;
    }
}