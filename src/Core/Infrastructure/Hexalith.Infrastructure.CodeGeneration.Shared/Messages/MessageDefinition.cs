namespace Hexalith.Infrastructure.CodeGeneration.Messages
{
    using System.Collections.Generic;
    internal abstract class MessageDefinition
    {
        protected MessageDefinition(string name, string namespaceName, IEnumerable<PropertyDefinition> properties)
        {
            Name = name;
            Namespace = namespaceName;
            Properties = properties;
        }

        public string Name { get; }
        public string Namespace { get; set; }
        public IEnumerable<PropertyDefinition> Properties { get; }
    }
}