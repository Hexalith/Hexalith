﻿// <copyright file="DummyStoreAppModule.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class DummyStoreAppModule : IStoreAppApplicationModule
{
    public IEnumerable<string> Dependencies => [];

    public string Description => "Test module description";

    public string Id => "StoreAppTest";

    public string Name => "StoreApp Test";

    public int OrderWeight => 66;

    public string Path { get; }

    public IEnumerable<Assembly> PresentationAssemblies => [GetType().Assembly];

    public string Version => "2.1";

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration == null)
        {
            return;
        }

        _ = services.AddSingleton<IStoreAppApplicationModule, DummyStoreAppModule>();
    }

    public void UseModule(object builder) => throw new NotImplementedException();
}