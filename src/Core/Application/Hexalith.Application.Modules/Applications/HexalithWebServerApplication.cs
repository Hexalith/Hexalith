// <copyright file="HexalithWebServerApplication.cs" company="ITANEO">
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
public abstract class HexalithWebServerApplication : HexalithApplication, IWebServerApplication
{
    private IEnumerable<Assembly>? _presentationAssemblies;

    /// <inheritdoc/>
    public override ApplicationType ApplicationType => ApplicationType.WebServer;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules =>
        WebServerModules.Distinct().OrderBy(p => p.FullName);

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
    {
        get
        {
            if (_presentationAssemblies is null)
            {
                List<Assembly> assemblies = [];
                foreach (Type module in WebServerModules.Distinct())
                {
                    if (!typeof(IUIApplicationModule).IsAssignableFrom(module))
                    {
                        continue;
                    }

                    IUIApplicationModule uiModule =
                        Activator.CreateInstance(module) as IUIApplicationModule
                        ?? throw new InvalidOperationException(
                            $"Unable to create an instance of {module.FullName}");
                    assemblies.AddRange(uiModule.PresentationAssemblies);
                }

                _presentationAssemblies = assemblies.Distinct();
            }

            return _presentationAssemblies
                .Concat(WebAppApplication?.PresentationAssemblies ?? [])
                .Distinct();
        }
    }

    /// <inheritdoc/>
    public abstract Type WebAppApplicationType { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> WebServerModules { get; }

    /// <inheritdoc/>
    public override Action<object> ConfigureAuthentication()
    {
        return options =>
        {
            foreach (Type module in Modules)
            {
                if (!typeof(IWebServerApplicationModule).IsAssignableFrom(module))
                {
                    continue;
                }

                IWebServerApplicationModule webServerModule =
                    Activator.CreateInstance(module) as IWebServerApplicationModule
                    ?? throw new InvalidOperationException(
                        $"Unable to create an instance of {module.FullName}");
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
                if (!typeof(IWebServerApplicationModule).IsAssignableFrom(module))
                {
                    continue;
                }

                IWebServerApplicationModule webServerModule =
                    Activator.CreateInstance(module) as IWebServerApplicationModule
                    ?? throw new InvalidOperationException(
                        $"Unable to create an instance of {module.FullName}");
                webServerModule.ConfigureAuthorization(options);
                foreach (
                    KeyValuePair<
                        string,
                        AuthorizationPolicy
                    > policy in webServerModule.AuthorizationPolicies)
                {
                    authorizationOptions.AddPolicy(policy.Key, policy.Value);
                }
            }
        };
    }
}