// <copyright file="HexalithWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;

/// <summary>
/// Application definition base class.
/// </summary>
public abstract class HexalithWebServerApplication : HexalithApplication, IWebServerApplication
{
    /// <inheritdoc/>
    public override ApplicationType ApplicationType => ApplicationType.WebServer;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => WebServerModules.Distinct().OrderBy(p => p.FullName);

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
    {
        get
        {
            IEnumerable<Type> modules = WebServerModules;

            if (WebAppApplication is not null)
            {
                modules = modules.Concat(WebAppApplication.Modules);
            }

            List<Assembly> assemblies = [];
            foreach (Type module in modules.Distinct())
            {
                if (Activator.CreateInstance(module) is not IApplicationModule applicationModule)
                {
                    throw new InvalidOperationException($"The module {module.FullName} must implement {nameof(IApplicationModule)}.");
                }

                if (applicationModule is IUIApplicationModule uiModule)
                {
                    assemblies.AddRange(uiModule.PresentationAssemblies);
                }
            }

            return assemblies.Distinct();
        }
    }

    /// <inheritdoc/>
    public abstract Type WebAppApplicationType { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> WebServerModules { get; }
}