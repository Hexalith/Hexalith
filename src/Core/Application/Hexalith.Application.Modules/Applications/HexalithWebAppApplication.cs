// <copyright file="HexalithWebAppApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Application definition base class.
/// </summary>
public abstract class HexalithWebAppApplication : HexalithApplication, IWebAppApplication
{
    private IEnumerable<Type>? _modules;
    private IEnumerable<Assembly>? _presentationAssemblies;

    /// <inheritdoc/>
    public override ApplicationType ApplicationType => ApplicationType.WebApp;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. WebAppModules
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
    {
        get
        {
            if (_presentationAssemblies is null)
            {
                List<Assembly> assemblies = [GetType().Assembly];
                foreach (Type module in WebAppModules.Distinct())
                {
                    if (!typeof(IUIApplicationModule).IsAssignableFrom(module))
                    {
                        continue;
                    }

                    IUIApplicationModule uiModule = Activator.CreateInstance(module) as IUIApplicationModule
                        ?? throw new InvalidOperationException($"Unable to create an instance of {module.FullName}");
                    assemblies.AddRange(uiModule.PresentationAssemblies);
                }

                _presentationAssemblies = assemblies.Distinct();
            }

            return _presentationAssemblies;
        }
    }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> WebAppModules { get; }

    /// <inheritdoc/>
    public override Action<object> ConfigureAuthentication()
    {
        return options =>
        {
            foreach (Type module in Modules)
            {
                if (!typeof(IWebAppApplicationModule).IsAssignableFrom(module))
                {
                    continue;
                }

                IWebAppApplicationModule webServerModule = Activator.CreateInstance(module) as IWebAppApplicationModule
                    ?? throw new InvalidOperationException($"Unable to create an instance of {module.FullName}");
                webServerModule.ConfigureAuthorization(options);
            }
        };
    }

    /// <inheritdoc/>
    public override Action<object> ConfigureAuthorization()
    {
        return options =>
        {
            if (options is not AuthorizationOptions authorizationOptions)
            {
                return;
            }

            foreach (Type module in Modules)
            {
                if (!typeof(IWebAppApplicationModule).IsAssignableFrom(module))
                {
                    continue;
                }

                IWebAppApplicationModule webServerModule = Activator.CreateInstance(module) as IWebAppApplicationModule
                    ?? throw new InvalidOperationException($"Unable to create an instance of {module.FullName}");
                webServerModule.ConfigureAuthorization(options);
                foreach (KeyValuePair<string, AuthorizationPolicy> policy in webServerModule.AuthorizationPolicies)
                {
                    authorizationOptions.AddPolicy(policy.Key, policy.Value);
                }
            }
        };
    }
}