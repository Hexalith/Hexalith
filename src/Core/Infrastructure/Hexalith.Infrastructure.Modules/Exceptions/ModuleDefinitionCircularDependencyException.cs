namespace Hexalith.Infrastructure.Modules.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class ModuleDefinitionCircularDependencyException : Exception
    {
        public ModuleDefinitionCircularDependencyException() : this(string.Empty, string.Empty, new string[] { string.Empty }, null, null)
        {
        }

        public ModuleDefinitionCircularDependencyException(string moduleName, string dependencyName, IEnumerable<string> tree) : this(moduleName, dependencyName, tree, null, null)
        {
        }

        public ModuleDefinitionCircularDependencyException(string moduleName, string dependencyName, IEnumerable<string> tree, string? message, Exception? innerException)
            : base($"Circular dependency in module {moduleName} with dependency {dependencyName}. Dependency tree : {string.Join(',', tree)}.\n{message}", innerException)
        {
        }

        public ModuleDefinitionCircularDependencyException(string? message) : base(message)
        {
        }

        public ModuleDefinitionCircularDependencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ModuleDefinitionCircularDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}