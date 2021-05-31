namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Hexalith.Infrastructure.Modules.Definitions;

    [Serializable]
    public class InvalidModuleDefinitionException : Exception
    {
        public InvalidModuleDefinitionException()
        {
        }

        public InvalidModuleDefinitionException(ModuleDefinition definition) : this(definition, string.Empty)
        {
        }

        public InvalidModuleDefinitionException(ModuleDefinition definition, string? message) : this(definition, message, null)
        {
        }

        public InvalidModuleDefinitionException(ModuleDefinition definition, string? message, Exception? innerException)
            : base($"Invalid module definition : {message}\nDefinition : {JsonSerializer.Serialize(definition)}", innerException)
        {
        }

        public InvalidModuleDefinitionException(string? message) : base(message)
        {
        }

        public InvalidModuleDefinitionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidModuleDefinitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}