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
    public override string HomePath => SharedAssetsApplication.HomePath;

    /// <inheritdoc/>
    public override string Id => SharedAssetsApplication.Id;

    /// <inheritdoc/>
    public override bool IsClient => true;

    /// <inheritdoc/>
    public override bool IsServer => false;

    /// <inheritdoc/>
    public override string LoginPath => SharedAssetsApplication.LoginPath;

    /// <inheritdoc/>
    public override string LogoutPath => SharedAssetsApplication.LogoutPath;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. WebAppModules
        .Union(SharedAssetsApplication.SharedAssetsModules)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public override string Name => SharedAssetsApplication.Name;

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => _presentationAssemblies ??= [.. WebAppModules
        .Select(p => p.Assembly)
        .Union(SharedAssetsApplication.PresentationAssemblies)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public abstract Type SharedAssetsApplicationType { get; }

    /// <inheritdoc/>
    public override string Version => SharedAssetsApplication.Version;

    /// <inheritdoc/>
    public abstract IEnumerable<Type> WebAppModules { get; }
}