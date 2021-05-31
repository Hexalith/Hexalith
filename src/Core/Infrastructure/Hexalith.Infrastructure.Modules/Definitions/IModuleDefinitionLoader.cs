namespace Hexalith.Infrastructure.Modules.Definitions
{
    using System.Threading.Tasks;

    public interface IModuleDefinitionLoader
    {
        Task<ModuleDefinition[]> GetDefinitions();
    }
}