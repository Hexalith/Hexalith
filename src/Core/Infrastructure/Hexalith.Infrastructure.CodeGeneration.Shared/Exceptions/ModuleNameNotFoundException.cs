namespace Hexalith.Infrastructure.CodeGeneration.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ModuleNameNotFoundException : Exception
    {
        public ModuleNameNotFoundException()
        {
        }

        public ModuleNameNotFoundException(string? message) : this(message, null)
        {
        }

        public ModuleNameNotFoundException(string? message, Exception? innerException) : base("The module name was not found. Add <ModuleName>MyModule</ModuleName> tag in your CSPROJ file." + message, innerException)
        {
        }

        protected ModuleNameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}