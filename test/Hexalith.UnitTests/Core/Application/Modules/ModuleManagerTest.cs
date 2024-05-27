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
    public void ReflectionShouldFindModule()
    {
        _ = ModuleManager.Modules.Should().HaveCount(1);
        _ = ModuleManager.Modules["Test1"].Should().BeOfType<TestModule>();
    }

    [Fact]
    public void TestModuleShouldBeModule()
        => ModuleManager
            .IsModule<IApplicationModule>(typeof(TestModule))
            .Should()
            .BeTrue();
}