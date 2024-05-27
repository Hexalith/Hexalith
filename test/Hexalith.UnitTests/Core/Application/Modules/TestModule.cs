// <copyright file="TestModule.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class TestModule : IApplicationModule
{
    public IEnumerable<string> Dependencies => [];

    public string Description => "Test module description";

    public string Id => "Test1";

    public ModuleType ModuleType => ModuleType.Shared;

    public string Name => "Test";

    public int OrderWeight => 66;

    public string Path { get; }

    public IEnumerable<Assembly> PresentationAssemblies => [GetType().Assembly];

    public string Version => "2.1";

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
    }
}