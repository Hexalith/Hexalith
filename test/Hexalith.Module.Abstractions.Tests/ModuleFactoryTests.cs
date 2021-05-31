namespace Hexalith.Module.Abstractions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules;
    using Hexalith.Infrastructure.Modules.Definitions;
    using Hexalith.Infrastructure.Modules.Exceptions;
    using Hexalith.Module.Abstractions.Tests.Fixture;

    using FluentAssertions;

    using Microsoft.Extensions.Configuration;

    using Moq;

    using Xunit;

    public class ModuleFactoryTests
    {
        [Fact]
        public async Task GetModules_FromOneLoaderAndOneActivator()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeModuleDefinitionLoader1()
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator1(configuration)
            });
            (await factory.GetModules().ConfigureAwait(false))
                .Should()
                .HaveCount(6);
        }

        [Fact]
        public async Task GetModules_FromTwoActivators()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeModuleDefinitionLoader1(),
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator2(configuration),
                () => new FakeModuleActivator3(configuration)
            });
            (await factory.GetModules().ConfigureAwait(false))
                .Should()
                .HaveCount(6);
        }

        [Fact]
        public Task GetModules_WithCircularDependency()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeModuleDefinitionWithCircularDependenciesLoader(),
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator1(configuration)
            });
            factory
                .Invoking(p => p.GetModules())
                .Should()
                .Throw<ModuleDefinitionCircularDependencyException>()
                .WithMessage("*module1 with dependency module6*");
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetModules_WithDependencies()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeModuleDefinitionWithDependenciesLoader(),
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator1(configuration)
            });
            var modules = await factory.GetModules().ConfigureAwait(false);
            modules
                .Select(p => p.ModuleDefinition.Name)
                .Should()
                .ContainInOrder("Module1", "Module2", "Module3", "Module4", "Module5", "Module6");
            modules
                .Should()
                .HaveCount(6);
        }

        [Fact]
        public async Task GetModules_WithDuplicateActivations()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeModuleDefinitionLoader1()
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator1(configuration),
                () => new FakeModuleActivator2(configuration)
            });
            (await factory.GetModules().ConfigureAwait(false))
                .Should()
                .HaveCount(6);
        }

        [Fact]
        public Task GetModules_WithDuplicateModuleDefinitions()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeDuplicatesModuleDefinitionLoader(),
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator1(configuration)
            });
            factory
                .Invoking(p => p.GetModules())
                .Should()
                .Throw<DuplicateModuleDefinitionException>()
                .WithMessage("*Module1*Module4*");
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetModules_WithPriority()
        {
            IConfiguration configuration = new Mock<IConfiguration>().Object;
            var factory = new ModuleFactory(
                new List<Func<IModuleDefinitionLoader>> {
                () => new FakeModuleDefinitionWithPrioriryLoader(),
                },
                new List<Func<IModuleActivator>> {
                () => new FakeModuleActivator1(configuration)
            });
            var modules = await factory.GetModules().ConfigureAwait(false);
            modules
                .Should()
                .HaveCount(6);
            modules
                .Select(p => p.ModuleDefinition.Name)
                .Should()
                .ContainInOrder("Module1", "Module2", "Module3", "Module4", "Module5", "Module6");
        }
    }
}