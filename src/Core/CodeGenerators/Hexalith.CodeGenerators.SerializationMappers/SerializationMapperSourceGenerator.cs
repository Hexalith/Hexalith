// <copyright file="SerializationMapperSourceGenerator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.CodeGenerators.SerializationMappers;

using System.Collections.Immutable;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

/// <summary>
/// The source generator that generates the source code for the DI registration of the serialization mappers.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class SerializationMapperSourceGenerator : IIncrementalGenerator
{
    private const string _serializationMapperAttributeFullName = "Hexalith.PolymorphicSerialization.PolymorphicSerializationAttribute";
    private const string _serializationMapperAttributeName = "PolymorphicSerializationAttribute";

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(TypeDeclarationSyntax?, AttributeData?)> classOrRecordDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
            _serializationMapperAttributeFullName,
            predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
            transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m.Type is not null && m.Data is not null);

        IncrementalValueProvider<(Compilation, ImmutableArray<(TypeDeclarationSyntax?, AttributeData?)>)> compilationAndClasses
            = context.CompilationProvider.Combine(classOrRecordDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, static (spc, source)
            => Execute(source.Item1, source.Item2, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<(TypeDeclarationSyntax? Type, AttributeData? Data)> classesOrRecords, SourceProductionContext context)
    {
        IEnumerable<(TypeDeclarationSyntax, AttributeData)> types = classesOrRecords
            .Where(p => p.Type != null && p.Data != null)
            .Select(p => (p.Type!, p.Data!)!);
        List<INamedTypeSymbol> symbols = [];
        foreach ((TypeDeclarationSyntax Type, AttributeData Data) classOrRecord in types)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(classOrRecord.Type.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classOrRecord.Type) is not INamedTypeSymbol symbol)
            {
                continue;
            }

            symbols.Add(symbol);
            string? namespaceName = symbol.ContainingNamespace.ToDisplayString();
            string domainName = classOrRecord.Type.Identifier.Text;

            context.AddSource(
                $"{domainName}Mapper.g.cs",
                SourceText.From(
                    GenerateMapperClass(classOrRecord, symbol, namespaceName),
                    Encoding.UTF8));
        }

        string code = GenerateAddServicesExtension(symbols);

        context.AddSource($"SerializationMapperExtension.g.cs", SourceText.From(code, Encoding.UTF8));
    }

    private static string GenerateAddServicesExtension(List<INamedTypeSymbol> symbols)
    {
        string namespaceName = symbols.First().ContainingAssembly.MetadataName;
        string usings = symbols
            .Select(s => s.ContainingNamespace.ToDisplayString())
            .Distinct()
            .Select(n => $"using {n}.PolymorphicMappers;")
            .Aggregate((a, b) => $"{a}\n{b}");
        string addSingletonMappers = symbols
            .Select(s => $"        services.TryAddSingleton<IPolymorphicSerializationMapper, {s.MetadataName}Mapper>();")
            .Aggregate((a, b) => $"{a}\n{b}");
        string project = namespaceName.Replace(".", string.Empty);
        return $$"""
                namespace {{namespaceName}}.Extensions;

                {{usings}}
                using Microsoft.Extensions.DependencyInjection;
                using Microsoft.Extensions.DependencyInjection.Extensions;
                using System;
                using Hexalith.PolymorphicSerialization;

                public static class {{project}}MapperExtension
                {
                    public static IServiceCollection Add{{project}}Mappers(this IServiceCollection services)
                    {
                        services.TryAddSingleton<PolymorphicSerializationResolver>();
                {{addSingletonMappers}}
                        return services;
                    }
                }
                """;
    }

    private static string GenerateMapperClass((TypeDeclarationSyntax Type, AttributeData Data) syntax, INamedTypeSymbol classSymbol, string? namespaceName)
    {
        // get the name and the version from the attribute
        TypedConstant nameParam = syntax.Data.ConstructorArguments[0];
        TypedConstant versionParam = syntax.Data.ConstructorArguments[1];
        TypedConstant typeParam = syntax.Data.ConstructorArguments[2];
        string? name = (string?)nameParam.Value;
        int? version = (int?)versionParam.Value;
        Type? baseType = (Type?)typeParam.Value;
        string typeDiscriminator = name + "V" + version;
        string baseTypeName = baseType == null
            ? classSymbol.IsRecord
                ? "PolymorphicRecordBase"
                : "PolymorphicClassBase"
            : baseType.FullName;

        return $$"""
                namespace {{namespaceName}}.PolymorphicMappers;

                using Microsoft.Extensions.DependencyInjection;
                using Hexalith.PolymorphicSerialization;
                using System.Runtime.Serialization;

                [DataContract]
                public sealed partial {{(classSymbol.IsRecord ? "record" : "class")}} {{classSymbol.MetadataName}} : {{baseTypeName}} {}

                public sealed record {{classSymbol.MetadataName}}Mapper() : PolymorphicSerializationMapper<{{classSymbol.MetadataName}},{{baseTypeName}}>("{{typeDiscriminator}}")
                {
                }
                """;
    }

    private static (TypeDeclarationSyntax? Type, AttributeData? Data) GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetNode is not TypeDeclarationSyntax classDeclaration)
        {
            return (null, null);
        }

        AttributeData? attribute = context.Attributes
            .FirstOrDefault(a => a.AttributeClass?.Name == _serializationMapperAttributeName);

        return (classDeclaration, attribute);
    }
}