// <copyright file="HexalithClientApplication.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Application definition base class.
/// </summary>
public abstract class HexalithClientApplication : HexalithApplication, IClientApplication
{
    private IEnumerable<Type>? _modules;
    private IEnumerable<Assembly>? _presentationAssemblies;

    /// <inheritdoc/>
    public abstract IEnumerable<Type> ClientModules { get; }

    /// <inheritdoc/>
    public override string HomePath => Shared.HomePath;

    /// <inheritdoc/>
    public override string Id => Shared.Id;

    /// <inheritdoc/>
    public override bool IsClient => true;

    /// <inheritdoc/>
    public override bool IsServer => false;

    /// <inheritdoc/>
    public override string LoginPath => Shared.LoginPath;

    /// <inheritdoc/>
    public override string LogoutPath => Shared.LogoutPath;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. ClientModules
        .Union(Shared.SharedModules)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public override string Name => Shared.Name;

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => _presentationAssemblies ??= [.. ClientModules
        .Select(p => p.Assembly)
        .Union(Shared.PresentationAssemblies)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public abstract Type SharedApplicationType { get; }

    /// <inheritdoc/>
    public override string Version => Shared.Version;
}