namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DuplicateModuleDefinitionException : Exception
    {
        public DuplicateModuleDefinitionException() : this(new string[] { string.Empty }, string.Empty, null)
        {
        }

        public DuplicateModuleDefinitionException(string[] duplicates) : this(duplicates, string.Empty, null)
        {
        }

        public DuplicateModuleDefinitionException(string[] duplicateNames, string? message, Exception? innerException)
            : base($"Duplicate module definition(s) : {string.Join(',', duplicateNames)}.\n{message}", innerException)
        {
        }

        public DuplicateModuleDefinitionException(string? message) : base(message)
        {
        }

        public DuplicateModuleDefinitionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateModuleDefinitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}