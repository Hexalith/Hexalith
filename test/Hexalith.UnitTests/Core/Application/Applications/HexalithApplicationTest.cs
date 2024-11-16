// <copyright file="HexalithApplicationTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using FluentAssertions;

using Hexalith.Application.Modules.Applications;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moq;

public class HexalithApplicationTest
{
    [Fact]
    public void DummyServicesFromClientModulesShouldBeAdded()
    {
        ServiceCollection services = [];
        Mock<IConfiguration> configurationMock = new();

        HexalithApplication.AddWebAppServices(services, configurationMock.Object);

        // Check that the services have been added
        _ = services
            .Where(p => p.ImplementationType == typeof(DummyClientService))
            .Should()
            .HaveCount(1);
        _ = services
            .Where(p => p.ImplementationType == typeof(DummySharedService))
            .Should()
            .HaveCount(1);
        _ = services
            .Where(p => p.ImplementationType == typeof(DummyServerService))
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void DummyServicesFromServerModulesShouldBeAdded()
    {
        ServiceCollection services = [];
        Mock<IConfiguration> configurationMock = new();

        HexalithApplication.AddWebServerServices(services, configurationMock.Object);

        // Check that the services have been added
        _ = services
            .Where(p => p.ImplementationType == typeof(DummyServerService))
            .Should()
            .HaveCount(1);
        _ = services
            .Where(p => p.ImplementationType == typeof(DummySharedService))
            .Should()
            .HaveCount(1);
        _ = services
            .Where(p => p.ImplementationType == typeof(DummyClientService))
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void HexalithApplicationWebAppModuleShouldBeInstantiated()
        => HexalithApplication
            .WebAppApplication
            .WebAppModules
            .Should()
            .Contain(typeof(DummyClientModule));

    [Fact]
    public void HexalithApplicationWebServerModuleShouldBeInstantiated()
        => HexalithApplication
            .WebServerApplication
            .WebServerModules
            .Should()
            .Contain(typeof(DummyServerModule));
}