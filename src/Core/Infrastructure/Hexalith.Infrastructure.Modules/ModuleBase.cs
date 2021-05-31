namespace Hexalith.Infrastructure.Modules
{
    using System;

    using Hexalith.Infrastructure.Modules.Definitions;
    using Hexalith.Infrastructure.Modules.Exceptions;

    public abstract class ModuleBase : IModule
    {
        private ModuleDefinition? _moduleDefinition;

        protected ModuleBase(ModuleType moduleType)
        {
            ModuleType = moduleType;
        }

        public ModuleDefinition ModuleDefinition => _moduleDefinition ??= InitializeModuleDefinition();
        public ModuleType ModuleType { get; }

        protected virtual ModuleDefinition InitializeModuleDefinition()
        {
            var type = GetType();
            var name = type.Name;
            var moduleTypeName = $"{ModuleType}Module";
            if (!name.EndsWith(moduleTypeName, StringComparison.InvariantCulture))
            {
                throw new InvalidModuleDefinitionException($"The module name should end with '{moduleTypeName}'.");
            }
            return new ModuleDefinition
            (
                name.Substring(0, name.Length - moduleTypeName.Length), // Remove the word "Server" from the name
                type.FullName ?? throw new InvalidModuleDefinitionException($"The Full name of '{name}' is undefined.")
            );
        }
    }
}