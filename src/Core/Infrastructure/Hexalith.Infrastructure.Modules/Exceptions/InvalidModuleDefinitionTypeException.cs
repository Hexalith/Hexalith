namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Hexalith.Infrastructure.Modules.Definitions;

    [Serializable]
    public class InvalidModuleDefinitionTypeException : InvalidModuleDefinitionException
    {
        public InvalidModuleDefinitionTypeException(ModuleDefinition definition) : this(definition, string.Empty)
        {
        }

        public InvalidModuleDefinitionTypeException(ModuleDefinition definition, string? message) : this(definition, message, null)
        {
        }

        public InvalidModuleDefinitionTypeException(ModuleDefinition definition, string? message, Exception? innerException)
            : base(definition, $"{nameof(ModuleDefinition.TypeName)} is mandatory. {message}\n", innerException)
        {
        }

        public InvalidModuleDefinitionTypeException() : base()
        {
        }

        public InvalidModuleDefinitionTypeException(string? message) : base(message)
        {
        }

        public InvalidModuleDefinitionTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidModuleDefinitionTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}