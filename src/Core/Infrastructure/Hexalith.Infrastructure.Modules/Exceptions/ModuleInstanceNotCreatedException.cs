namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Hexalith.Infrastructure.Modules.Definitions;

    [Serializable]
    public class ModuleInstanceNotCreatedException : Exception
    {
        public ModuleInstanceNotCreatedException()
        {
        }

        public ModuleInstanceNotCreatedException(ModuleDefinition definition) : this(definition, string.Empty)
        {
        }

        public ModuleInstanceNotCreatedException(ModuleDefinition definition, string? message) : this(definition, message, null)
        {
        }

        public ModuleInstanceNotCreatedException(ModuleDefinition definition, string? message, Exception? innerException)
            : base($"Cannont create module '{definition.NormalizedName}' instance. {message}\nDefinition : {JsonSerializer.Serialize(definition)}", innerException)
        {
        }

        public ModuleInstanceNotCreatedException(string? message) : base(message)
        {
        }

        public ModuleInstanceNotCreatedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ModuleInstanceNotCreatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}