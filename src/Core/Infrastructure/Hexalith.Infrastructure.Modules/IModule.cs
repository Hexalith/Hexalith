namespace Hexalith.Infrastructure.Modules
{
    using Hexalith.Infrastructure.Modules.Definitions;

    public interface IModule
    {
        ModuleDefinition ModuleDefinition { get; }
    }
}