namespace Hexalith.Infrastructure.CodeGeneration
{
    using System;

    using Hexalith.Infrastructure.CodeGeneration.Abstractions.Diagnostics;
    using Hexalith.Infrastructure.CodeGeneration.Helpers;

    using Microsoft.CodeAnalysis;

    public abstract class SourceGeneratorBase : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                string moduleName = context.GetMSBuildProperty("ModuleName");
                if (string.IsNullOrWhiteSpace(moduleName))
                {
                    moduleName = nameof(Hexalith) + "Undefined";
                }
                string namespaceName = context.GetMSBuildProperty("RootNamespace");
                if (string.IsNullOrWhiteSpace(namespaceName))
                {
                    namespaceName = nameof(Hexalith) + "." + moduleName;
                }
                Execute(context, moduleName, namespaceName);
            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticMessages.CodeGenerationError, Location.None, GetType().Name, e.Message));
            }
        }

        public virtual void Initialize(GeneratorInitializationContext context)
        {
        }

        protected abstract void Execute(GeneratorExecutionContext context, string moduleName, string namespaceName);
    }
}