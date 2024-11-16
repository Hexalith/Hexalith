// <copyright file="HexalithWebAppApplication.cs" company="ITANEO">
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
    public IEnumerable<Assembly> PresentationAssemblies => _presentationAssemblies ??= [.. WebAppModules
        .Select(p => p.Assembly)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public abstract IEnumerable<Type> WebAppModules { get; }
}