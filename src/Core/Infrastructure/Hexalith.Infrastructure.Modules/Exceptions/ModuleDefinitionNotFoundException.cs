namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ModuleDefinitionNotFoundException : Exception
    {
        public ModuleDefinitionNotFoundException()
        {
        }

        public ModuleDefinitionNotFoundException(string name, string? message, Exception? innerException = null)
             : base($"Module definition with normalized name {name}, was not found. {message}", innerException)
        {
            NotFoundName = name;
        }

        public ModuleDefinitionNotFoundException(string? message) : base(message)
        {
        }

        public ModuleDefinitionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ModuleDefinitionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string NotFoundName { get; } = string.Empty;
    }
}