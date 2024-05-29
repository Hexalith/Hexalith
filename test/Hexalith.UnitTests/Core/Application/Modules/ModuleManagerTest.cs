// <copyright file="ModuleManagerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Modules;

using FluentAssertions;

using Hexalith.Application.Modules;
using Hexalith.Application.Modules.Configurations;
using Hexalith.Application.Modules.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

public class ModuleManagerTest
{
    [Fact]
    public void DummyServicesFromModulesShouldBeAdded()
    {
        ServiceCollection services = [];
        Mock<IConfiguration> configurationMock = new();

        ModuleManager.AddSharedModulesServices(services, configurationMock.Object);

        // Check that the services have been added
        _ = services
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void ModuleManagerShouldInstanciateClientModule()
        => ModuleManager
            .GetModule<IClientApplicationModule>(typeof(DummyClientModule))
            .Should()
            .BeOfType<DummyClientModule>();

    [Fact]
    public void ModuleManagerShouldInstanciateServerModule()
        => ModuleManager
            .GetModule<IServerApplicationModule>(typeof(DummyServerModule))
            .Should()
            .BeOfType<DummyServerModule>();

    [Fact]
    public void ModuleManagerShouldInstanciateSharedModule()
        => ModuleManager
            .GetModule<ISharedApplicationModule>(typeof(DummySharedModule))
            .Should()
            .BeOfType<DummySharedModule>();

    [Fact]
    public void ModuleManagerShouldInstanciateStoreAppModule()
        => ModuleManager
            .GetModule<IStoreAppApplicationModule>(typeof(DummyStoreAppModule))
            .Should()
            .BeOfType<DummyStoreAppModule>();

    [Fact]
    public void ModuleManagerShouldReturnClientAssemby()
    {
        ILogger<ModuleManager> logger = Mock.Of<ILogger<ModuleManager>>();
        IOptions<ModuleSettings> options = Options.Create(new ModuleSettings());

        ModuleManager manager = new([], options, logger);
        _ = ModuleManager.ClientModuleTypes.Should().HaveCount(1);
        _ = manager
            .ClientPresentationAssemblies
            .Should()
            .Contain(typeof(DummyClientModule).Assembly);
    }

    [Fact]
    public void ModuleManagerShouldReturnClientModule()
    {
        ILogger<ModuleManager> logger = Mock.Of<ILogger<ModuleManager>>();
        IOptions<ModuleSettings> options = Options.Create(new ModuleSettings());

        ModuleManager manager = new([], options, logger);
        _ = ModuleManager.ClientModuleTypes.Should().HaveCount(1);
        _ = manager
            .ClientModules
            .Select(p => p.Value)
            .OfType<DummyClientModule>()
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void ModuleManagerShouldReturnServerAssemby()
    {
        ILogger<ModuleManager> logger = Mock.Of<ILogger<ModuleManager>>();
        IOptions<ModuleSettings> options = Options.Create(new ModuleSettings());

        ModuleManager manager = new([], options, logger);
        _ = ModuleManager.ServerModuleTypes.Should().HaveCount(1);
        _ = manager
            .ServerPresentationAssemblies
            .Should()
            .Contain(typeof(DummyServerModule).Assembly);
    }

    [Fact]
    public void ModuleManagerShouldReturnServerModule()
    {
        ILogger<ModuleManager> logger = Mock.Of<ILogger<ModuleManager>>();
        IOptions<ModuleSettings> options = Options.Create(new ModuleSettings());

        ModuleManager manager = new([], options, logger);
        _ = ModuleManager.ServerModuleTypes.Should().HaveCount(1);
        _ = manager
            .ServerModules
            .Select(p => p.Value)
            .OfType<DummyServerModule>()
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void ModuleManagerShouldReturnSharedModule()
    {
        ILogger<ModuleManager> logger = Mock.Of<ILogger<ModuleManager>>();
        IOptions<ModuleSettings> options = Options.Create(new ModuleSettings());

        ModuleManager manager = new([], options, logger);
        _ = ModuleManager.SharedModuleTypes.Should().HaveCount(1);
        _ = manager
            .SharedModules
            .Select(p => p.Value)
            .OfType<DummySharedModule>()
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void ModuleManagerShouldReturnStoreAppModule()
    {
        ILogger<ModuleManager> logger = Mock.Of<ILogger<ModuleManager>>();
        IOptions<ModuleSettings> options = Options.Create(new ModuleSettings());

        ModuleManager manager = new([], options, logger);
        _ = ModuleManager.StoreAppModuleTypes.Should().HaveCount(1);
        _ = manager
            .StoreAppModules
            .Select(p => p.Value)
            .OfType<DummyStoreAppModule>()
            .Should()
            .HaveCount(1);
    }

    [Fact]
    public void ReflectionShouldFindClientModuleType()
    {
        _ = ModuleManager.ClientModuleTypes.Should().HaveCount(1);
        _ = ModuleManager.ClientModuleTypes.FirstOrDefault(p => p == typeof(DummyClientModule));
    }

    [Fact]
    public void ReflectionShouldFindServerModuleType()
    {
        _ = ModuleManager.ServerModuleTypes.Should().HaveCount(1);
        _ = ModuleManager.ServerModuleTypes.FirstOrDefault(p => p == typeof(DummyServerModule));
    }

    [Fact]
    public void ReflectionShouldFindSharedModuleType()
    {
        _ = ModuleManager.SharedModuleTypes.Should().HaveCount(1);
        _ = ModuleManager.SharedModuleTypes.FirstOrDefault(p => p == typeof(DummySharedModule));
    }

    [Fact]
    public void ReflectionShouldFindStoreAppModuleType()
    {
        _ = ModuleManager.StoreAppModuleTypes.Should().HaveCount(1);
        _ = ModuleManager.StoreAppModuleTypes.FirstOrDefault(p => p == typeof(DummyStoreAppModule));
    }

    [Fact]
    public void TestModuleShouldBeModule()
        => ModuleManager
            .IsModule<IApplicationModule>(typeof(DummySharedModule))
            .Should()
            .BeTrue();
}