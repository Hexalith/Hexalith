// <copyright file="HexalithServerApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// Application definition base class.
/// </summary>
public abstract class HexalithServerApplication : HexalithApplication, IServerApplication
{
    private IEnumerable<Type>? _modules;
    private IEnumerable<Assembly>? _presentationAssemblies;

    /// <summary>
    /// Gets the client application type.
    /// </summary>
    public abstract Type ClientApplicationType { get; }

    /// <inheritdoc/>
    public override string HomePath => Shared.HomePath;

    /// <inheritdoc/>
    public override string Id => Shared.Id;

    /// <inheritdoc/>
    public override bool IsClient => false;

    /// <inheritdoc/>
    public override bool IsServer => true;

    /// <inheritdoc/>
    public override string LoginPath => Shared.LoginPath;

    /// <inheritdoc/>
    public override string LogoutPath => Shared.LogoutPath;

    /// <inheritdoc/>
    public override IEnumerable<Type> Modules => _modules ??=
        [.. ServerModules
        .Union(Shared.SharedModules)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public override string Name => Shared.Name;

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => _presentationAssemblies ??= [.. ServerModules
        .Select(p => p.Assembly)
        .Union(Shared.PresentationAssemblies)
        .Union(Client.PresentationAssemblies)
        .Distinct()
        .OrderBy(p => p.FullName)];

    /// <inheritdoc/>
    public abstract IEnumerable<Type> ServerModules { get; }

    /// <inheritdoc/>
    public abstract Type SharedApplicationType { get; }

    /// <inheritdoc/>
    public override string Version => Shared.Version;

    /// <summary>
    /// Registers the actors associated with the application.
    /// </summary>
    /// <param name="actors">The actor collection.</param>
    public void RegisterActors(object actors)
    {
        ArgumentNullException.ThrowIfNull(actors, nameof(actors));

        foreach (Type module in ServerModules)
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