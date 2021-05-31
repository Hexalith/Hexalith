namespace Hexalith.Infrastructure.Modules.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public record ModuleDefinition : IModuleDefinition
    {
        public ModuleDefinition(string name, string typeName, string? normalizedName = null, string[]? dependencies = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name not defined.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("Module type name not defined.", nameof(typeName));
            }
            Name = name;
            TypeName = typeName;
            NormalizedName = NormalizeName(string.IsNullOrWhiteSpace(normalizedName) ? Name : normalizedName);
            if (dependencies?.Any() != true)
            {
                Dependencies = Array.Empty<string>();
            }
            else
            {
                Dependencies = dependencies.Select(p => NormalizeName(p)).ToArray();
            }
        }

        public string Name { get; }
        public string TypeName { get; }
        public string NormalizedName { get; }

        public static string NormalizeName(string name)
            => name.Trim().ToLowerInvariant().Replace(' ', '_');

        public string Description { get; init; } = string.Empty;
        public string Version { get; init; } = "1.0.0";
        public IEnumerable<string> Dependencies { get; }
        public int Priority { get; init; }
        public bool Enabled { get; init; } = true;
    }
}