namespace Hexalith.Module.Abstractions.Tests.Fixture
{
    using System;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules.Definitions;

    internal class FakeDuplicatesModuleDefinitionLoader : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module1", "Module1Type"),
                new ModuleDefinition("Module2", "Module2Type"),
                new ModuleDefinition("Module1", "Module2Type"),
                new ModuleDefinition("Module3", "Module3Type"),
                new ModuleDefinition("Module4", "Module4Type"),
                new ModuleDefinition("Module4", "Module4Type"),
                new ModuleDefinition("Module5", "Module5Type")
            });
        }
    }

    internal class FakeEmptyModuleDefinitionLoader2 : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(Array.Empty<ModuleDefinition>());
        }
    }

    internal class FakeModuleDefinitionLoader1 : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module1", "FakeModule1"),
                new ModuleDefinition("Module2", "FakeModule2"),
                new ModuleDefinition("Module3", "FakeModule3"),
                new ModuleDefinition("Module4", "FakeModule4"),
                new ModuleDefinition("Module5", "FakeModule5"),
                new ModuleDefinition("Module6", "FakeModule6")
            });
        }
    }

    internal class FakeModuleDefinitionLoader2 : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module4", "FakeModule4"),
                new ModuleDefinition("Module5", "FakeModule5"),
            });
        }
    }

    internal class FakeModuleDefinitionLoader3 : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module1", "FakeModule1"),
                new ModuleDefinition("Module2", "FakeModule2"),
                new ModuleDefinition("Module3", "FakeModule3"),
                new ModuleDefinition("Module6", "FakeModule6")
            });
        }
    }

    internal class FakeModuleDefinitionWithCircularDependenciesLoader : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module6", "FakeModule6", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module4"), ModuleDefinition.NormalizeName("Module5") }),
                new ModuleDefinition("Module5", "FakeModule5"),
                new ModuleDefinition("Module4", "FakeModule4", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module3") }),
                new ModuleDefinition("Module2", "FakeModule2"),
                new ModuleDefinition("Module1", "FakeModule1", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module6") }),
                new ModuleDefinition("Module3", "FakeModule3", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module1") , ModuleDefinition.NormalizeName("Module2") })
            });
        }
    }

    internal class FakeModuleDefinitionWithDependenciesLoader : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module6", "FakeModule6", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module4"), ModuleDefinition.NormalizeName("Module5") }),
                new ModuleDefinition("Module5", "FakeModule5"),
                new ModuleDefinition("Module4", "FakeModule4", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module3") }),
                new ModuleDefinition("Module2", "FakeModule2"),
                new ModuleDefinition("Module1", "FakeModule1"),
                new ModuleDefinition("Module3", "FakeModule3", null,
                        new string[]{ ModuleDefinition.NormalizeName("Module1") , ModuleDefinition.NormalizeName("Module2") })
            });
        }
    }

    internal class FakeModuleDefinitionWithPrioriryLoader : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
        {
            return Task.FromResult(new ModuleDefinition[] {
                new ModuleDefinition("Module5", "FakeModule5"){Priority = 0},
                new ModuleDefinition("Module6", "FakeModule6"){Priority = 0},
                new ModuleDefinition("Module4", "FakeModule4"){Priority = 100},
                new ModuleDefinition("Module2", "FakeModule2"){Priority = 9000},
                new ModuleDefinition("Module1", "FakeModule1"){Priority = 10000},
                new ModuleDefinition("Module3", "FakeModule3"){Priority = 500}
            });
        }
    }
}