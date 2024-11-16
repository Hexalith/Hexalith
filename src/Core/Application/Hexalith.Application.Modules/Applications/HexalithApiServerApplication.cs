﻿// <copyright file="HexalithApiServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Application definition base class.
/// </summary>
public abstract class HexalithApiServerApplication : HexalithApplication, IApiServerApplication
{
    private IEnumerable<Type>? _modules;

    /// <inheritdoc/>
    public abstract IEnumerable<Type> ApiServerModules { get; }

    /// <inheritdoc/>
    public override ApplicationType ApplicationType => ApplicationType.ApiServer;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. ApiServerModules
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <summary>
    /// Registers the actors associated with the application.
    /// </summary>
    /// <param name="actors">The actor collection.</param>
    public void RegisterActors(object actors)
    {
        ArgumentNullException.ThrowIfNull(actors, nameof(actors));

        foreach (Type module in ApiServerModules)
        {
            MethodInfo? moduleMethod = module.GetMethod(
                nameof(RegisterActors),
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(object)],
                null);
            if (moduleMethod == null)
            {
                Debug.WriteLine(
                    $"The actors for module {module.Name} are not registered. The module does not have the following static method:"
                    + $" {nameof(RegisterActors)}(object actors).");
            }
            else
            {
                _ = moduleMethod.Invoke(null, [actors]);
                Debug.WriteLine($"The actors for module {module.Name} are have been added.");
            }
        }
    }
}