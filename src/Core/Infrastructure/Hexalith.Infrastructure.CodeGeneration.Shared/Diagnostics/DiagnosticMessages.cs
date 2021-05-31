namespace Hexalith.Infrastructure.CodeGeneration.Abstractions.Diagnostics
{
    using Microsoft.CodeAnalysis;

    internal static class DiagnosticMessages
    {
        public static readonly DiagnosticDescriptor CodeGenerationError = new(id: "BCG0001",
                                                                                               title: "Hexalith code generation error",
                                                                                               messageFormat: "Couldn't not generate code with code generator '{0}'.\nError : {1}",
                                                                                               category: "Design",
                                                                                               DiagnosticSeverity.Error,
                                                                                               isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor CodeGenerationInfo = new(id: "BCG0002",
                                                                                               title: "Hexalith code generation",
                                                                                               messageFormat: "Generating code with '{0}'.",
                                                                                               category: "Design",
                                                                                               DiagnosticSeverity.Info,
                                                                                               isEnabledByDefault: true);
    }
}