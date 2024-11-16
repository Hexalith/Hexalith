// <copyright file="HexalithWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Application definition base class.
/// </summary>
public abstract class HexalithWebServerApplication : HexalithApplication, IWebServerApplication
{
    private IEnumerable<Type>? _modules;
    private IEnumerable<Assembly>? _presentationAssemblies;

    /// <inheritdoc/>
    public override ApplicationType ApplicationType => ApplicationType.WebServer;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. WebServerModules
        .Union(WebAppApplication?.Modules ?? [])
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => _presentationAssemblies
        ??= [.. WebServerModules
        .Select(p => p.Assembly)
        .Union(WebAppApplication?.PresentationAssemblies ?? [])
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public abstract Type WebAppApplicationType { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> WebServerModules { get; }
}