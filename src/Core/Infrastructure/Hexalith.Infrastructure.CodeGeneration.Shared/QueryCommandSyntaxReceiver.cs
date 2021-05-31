namespace Hexalith.Infrastructure.CodeGeneration
{
    using System.Collections.Generic;
    using System.Linq;

    using Hexalith.Infrastructure.CodeGeneration.Messages;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal sealed class QueryCommandSyntaxReceiver : ISyntaxReceiver
    {
        private const string ApiCommand = nameof(ApiCommand);
        private const string ApiQuery = nameof(ApiQuery);
        public List<MessageDefinition> Messages { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TypeDeclarationSyntax typeSyntax)
            {
                if (typeSyntax is ClassDeclarationSyntax || typeSyntax is RecordDeclarationSyntax)
                {
                    var attribute = typeSyntax
                        .AttributeLists
                        .SelectMany(p => p.Attributes)
                        .FirstOrDefault(p => p.Name.ToString() == ApiQuery || p.Name.ToString() == ApiCommand);
                    if (attribute == null)
                    {
                        return;
                    }
                    string namespaceName = string.Empty;
                    SyntaxNode? parent = typeSyntax;
                    while ((parent = parent.Parent) != null)
                    {
                        if (parent is NamespaceDeclarationSyntax namespaceSyntax)
                        {
                            namespaceName = namespaceSyntax.Name.ToString();
                        }
                    }
                    Messages.Add(attribute.Name.ToString() switch
                    {
                        ApiQuery => new QueryDefinition(typeSyntax.Identifier.ValueText, namespaceName, new List<PropertyDefinition>()),
                        _ => new CommandDefinition(typeSyntax.Identifier.ValueText, namespaceName, new List<PropertyDefinition>())
                    });
                }
            }
        }
    }
}