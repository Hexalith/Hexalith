namespace Hexalith.Infrastructure.CodeGeneration.Messages
{
    internal sealed class PropertyDefinition
    {
        public PropertyDefinition(string name, string typeName)
        {
            Name = name;
            TypeName = typeName;
        }

        public string Name { get; }
        public string TypeName { get; }
    }
}