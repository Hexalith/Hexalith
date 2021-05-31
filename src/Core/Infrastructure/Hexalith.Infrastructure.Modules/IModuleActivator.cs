namespace Hexalith.Infrastructure.Modules
{
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules.Definitions;

    public interface IModuleActivator
    {
        Task<IModule?> FindModule(ModuleDefinition definition);

        Task<IModule> GetRequiredModule(ModuleDefinition definition);
    }
}