namespace Hexalith.Infrastructure.WebServer.Modules
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules.Definitions;

    public class ReflectionServerModuleDefinitionLoader : IModuleDefinitionLoader
    {
        public Task<ModuleDefinition[]> GetDefinitions()
            => Task.FromResult(
                AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && typeof(IServerModule).IsAssignableFrom(p))
                .Select(p => GetModuleDefinition(p))
                .ToArray()
            );

        private static ModuleDefinition GetModuleDefinition(Type moduleType)
        {
            var module = new ModuleDefinition(
                moduleType.Name,
                moduleType.AssemblyQualifiedName
                    ?? throw new TypeInitializationException(moduleType.FullName, null)
                );
            Console.WriteLine("Reflection loader found module : " + moduleType.Name);
            return module;
        }
    }
}