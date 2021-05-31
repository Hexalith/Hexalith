namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Hexalith.Infrastructure.Modules.Definitions;

    [Serializable]
    public class ModuleDefinitionDependencyNotFoundException : InvalidModuleDefinitionException
    {
        public ModuleDefinitionDependencyNotFoundException()
        {
        }

        public ModuleDefinitionDependencyNotFoundException(ModuleDefinition definition, string dependency, string? message, Exception? innerException = null) : base(definition, $"Module for dependency {dependency} not found. {message}\n", innerException)
        {
        }

        public ModuleDefinitionDependencyNotFoundException(ModuleDefinition definition) : base(definition)
        {
        }

        public ModuleDefinitionDependencyNotFoundException(ModuleDefinition definition, string? message) : base(definition, message)
        {
        }

        public ModuleDefinitionDependencyNotFoundException(ModuleDefinition definition, string? message, Exception? innerException) : base(definition, message, innerException)
        {
        }

        public ModuleDefinitionDependencyNotFoundException(string? message) : base(message)
        {
        }

        public ModuleDefinitionDependencyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ModuleDefinitionDependencyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}