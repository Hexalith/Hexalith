// <copyright file="HexalithUIComponentsSharedModule.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.UI.Components.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents the HexalithWeb module for customer management.
/// </summary>
public class HexalithUIComponentsSharedModule : ISharedApplicationModule
{
    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith Fluent UI components module";

    /// <inheritdoc/>
    public string Id => "Hexalith.UI.Components";

    /// <inheritdoc/>
    public string Name => "Hexalith Fluent UI";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => "hexalith/ui";

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
        => [GetType().Assembly];

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <inheritdoc/>
    public void AddServices(IServiceCollection services, IConfiguration configuration)
        => _ = services.AddFluentUITheme(configuration);
}