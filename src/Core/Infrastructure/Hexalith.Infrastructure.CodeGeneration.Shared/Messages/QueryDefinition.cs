namespace Hexalith.Infrastructure.CodeGeneration.Messages
{
    using System.Collections.Generic;
    internal sealed class QueryDefinition : MessageDefinition
    {
        public QueryDefinition(
            string name,
            string namespaceName,
            IEnumerable<PropertyDefinition> properties)
            : base(name, namespaceName, properties)
        {
        }
    }
}