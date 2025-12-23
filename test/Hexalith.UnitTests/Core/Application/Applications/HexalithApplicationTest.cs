// <copyright file="HexalithApplicationTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using Hexalith.Application.Modules.Applications;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Shouldly;

public class HexalithApplicationTest
{
    [Fact]
    public void DummyServicesFromClientModulesShouldBeAdded()
    {
        ServiceCollection services = [];
        IConfiguration configuration = Substitute.For<IConfiguration>();

        HexalithApplication.AddWebAppServices(services, configuration);

        // Check that the services have been added
        services
            .Where(p => p.ImplementationType == typeof(DummyClientService))
            .Count()
            .ShouldBe(1);
        services
            .Where(p => p.ImplementationType == typeof(DummyServerService))
            .ShouldBeEmpty();
    }

    [Fact]
    public void DummyServicesFromServerModulesShouldBeAdded()
    {
        ServiceCollection services = [];
        IConfiguration configuration = Substitute.For<IConfiguration>();

        HexalithApplication.AddWebServerServices(services, configuration);

        // Check that the services have been added
        services
            .Where(p => p.ImplementationType == typeof(DummyServerService))
            .Count()
            .ShouldBe(1);
        services
            .Where(p => p.ImplementationType == typeof(DummyClientService))
            .ShouldBeEmpty();
    }

    [Fact]
    public void HexalithApplicationWebAppModuleShouldBeInstantiated()
        => HexalithApplication
            .WebAppApplication
            .WebAppModules
            .ShouldContain(typeof(DummyClientModule));

    [Fact]
    public void HexalithApplicationWebServerModuleShouldBeInstantiated()
        => HexalithApplication
            .WebServerApplication
            .WebServerModules
            .ShouldContain(typeof(DummyServerModule));
}
