﻿// <copyright file="DummyServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class DummyServerModule : IWebServerApplicationModule
{
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => new Dictionary<string, AuthorizationPolicy>();

    public IEnumerable<string> Dependencies => [];

    public string Description => "Test module description";

    public string Id => "ServerTest";

    public string Name => "Server Test";

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

        _ = services.AddSingleton<DummyServerService>();
    }

    public void UseModule(object application)
    {
    }

    public void UseSecurity(object application)
    {
    }
}