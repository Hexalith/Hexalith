// <copyright file="ModuleManagerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Modules;

using FluentAssertions;

using Hexalith.Application.Modules;
using Hexalith.Application.Modules.Modules;

public class ModuleManagerTest
{
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