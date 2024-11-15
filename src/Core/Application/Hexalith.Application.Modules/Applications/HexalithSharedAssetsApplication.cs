// <copyright file="HexalithSharedUIElementsApplication.cs" company="ITANEO">
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
public abstract class HexalithSharedUIElementsApplication : HexalithApplication, ISharedUIElementsApplication
{
    private IEnumerable<Type>? _modules;
    private IEnumerable<Assembly>? _presentationAssemblies;

    /// <inheritdoc/>
    public override bool IsClient => false;

    /// <inheritdoc/>
    public override bool IsServer => false;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. SharedUIElementsModules
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
        => _presentationAssemblies ??= SharedUIElementsModules
            .Select(x => x.Assembly)
            .Distinct()
            .OrderBy(p => p.FullName);

    /// <inheritdoc/>
    public abstract IEnumerable<Type> SharedUIElementsModules { get; }
}