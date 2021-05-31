using System.Collections.Generic;

namespace Hexalith.Infrastructure.Modules.Definitions
{
    public interface IModuleDefinition
    {
        IEnumerable<string> Dependencies { get; }
        string Description { get; }
        bool Enabled { get; }
        string Name { get; }
        string NormalizedName { get; }
        int Priority { get; }
        string TypeName { get; }
        string Version { get; }
    }
}