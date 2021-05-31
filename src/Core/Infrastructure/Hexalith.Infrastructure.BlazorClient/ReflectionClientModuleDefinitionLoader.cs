namespace Hexalith.Infrastructure.BlazorClient
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules.Definitions;

    public class ReflectionClientModuleDefinitionLoader : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
            => Task.FromResult(AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && typeof(IClientModule).IsAssignableFrom(p))
                .Select(p => GetModuleDefinition(p))
                .ToArray()
                );

        private static ModuleDefinition GetModuleDefinition(Type moduleType)
            => new(moduleType.Name, moduleType.AssemblyQualifiedName
                    ?? throw new TypeInitializationException(moduleType.FullName, null)
                );
    }
}