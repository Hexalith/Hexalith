namespace Hexalith.Infrastructure.CodeGeneration.Generators.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Hexalith.Infrastructure.CodeGeneration.Messages;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    using Scriban;

    [Generator]
    public sealed class QueryCommandControllerGenerator : SourceGeneratorBase
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
                    "Hexalith.Infrastructure.WebServer.Controllers",
                    "Microsoft.AspNetCore.Authorization",
                    "Microsoft.AspNetCore.Mvc",
                    "Microsoft.Extensions.Logging",
                    "System",
                    "System.Threading.Tasks"
                };
                usings.AddRange(receiver.Messages
                        .Where(p => !string.IsNullOrWhiteSpace(p.Namespace))
                        .Select(p => p.Namespace)
                        .Distinct());
                var className = moduleName + "ApiController";
                var template = Template.Parse(QueryCommandControllerTemplate.Value);
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