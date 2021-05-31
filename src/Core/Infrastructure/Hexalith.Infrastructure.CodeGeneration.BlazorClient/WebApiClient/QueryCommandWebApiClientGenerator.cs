namespace Hexalith.Infrastructure.CodeGeneration.WebApiClient
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Hexalith.Infrastructure.CodeGeneration.Messages;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    using Scriban;

    [Generator]
    public sealed class QueryCommandWebApiClientGenerator : SourceGeneratorBase
    {
        public override void Initialize(GeneratorInitializationContext context)
        {
            base.Initialize(context);
            context.RegisterForSyntaxNotifications(() => new QueryCommandSyntaxReceiver());
        }

        protected override void Execute(GeneratorExecutionContext context, string moduleName, string namespaceName)
        {
            if (context.SyntaxReceiver is QueryCommandSyntaxReceiver receiver && receiver.Messages.Count > 0)
            {
                var usings = new List<string>()
                {
                    "Hexalith.Application.Messages",
                    "Hexalith.Application.Queries",
                    "Hexalith.Infrastructure.BlazorClient",
                    "System.Threading.Tasks",
                    "System.Net.Http"
                };
                usings.AddRange(receiver.Messages
                        .Where(p => !string.IsNullOrWhiteSpace(p.Namespace))
                        .Select(p => p.Namespace)
                        .Distinct());
                var className = moduleName + "WebApiClient";
                var template = Template.Parse(QueryCommandWebApiClientTemplate.Value);
                var result = template.Render(new
                {
                    Modulename = moduleName,
                    Namespace = namespaceName,
                    Queries = receiver.Messages.OfType<QueryDefinition>().ToList(),
                    Commands = receiver.Messages.OfType<CommandDefinition>().ToList(),
                    Usings = usings.Distinct().OrderBy(p => p).ToList()
                });
                var sourceText = SourceText.From(result, Encoding.UTF8);
                context.AddSource(className + ".cs", sourceText);
            }
        }
    }
}