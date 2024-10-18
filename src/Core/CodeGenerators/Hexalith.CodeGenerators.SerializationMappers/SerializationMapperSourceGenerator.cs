// <copyright file="SerializationMapperSourceGenerator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
    private const string _classBaseTypeName = "PolymorphicClassBase";
    private const string _recordBaseTypeName = "PolymorphicRecordBase";

    private const string _serializationMapperAttributeFullName = "Hexalith.PolymorphicSerialization.PolymorphicSerializationAttribute";

    private const string _serializationMapperAttributeName = "PolymorphicSerializationAttribute";

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(TypeDeclarationSyntax?, AttributeData?)> classOrRecordDeclarations = context
            .SyntaxProvider
            .ForAttributeWithMetadataName(
                _serializationMapperAttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m.Type is not null && m.Data is not null);

        IncrementalValueProvider<(Compilation, ImmutableArray<(TypeDeclarationSyntax?, AttributeData?)>)>
            compilationAndClasses
                = context.CompilationProvider.Combine(classOrRecordDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, static (spc, source)
            => Execute(source.Item1, source.Item2, spc));
    }

    private static void Execute(
        Compilation compilation,
        ImmutableArray<(TypeDeclarationSyntax? Type, AttributeData? Data)> classesOrRecords,
        SourceProductionContext context)
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
                    GenerateMapperClass(classOrRecord, symbol, namespaceName, context),
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
            .Select(n => $"using {n};")
            .Aggregate((a, b) => $"{a}\n{b}");
        string addSingletonMappers = symbols
            .Where(s => !s.IsAbstract)
            .Select(s =>
                $"        _ = services.AddSingleton<IPolymorphicSerializationMapper, {s.MetadataName}Mapper>();")
            .Aggregate((a, b) => $"{a}\n{b}");
        string addResolverMappers = symbols
            .Where(s => !s.IsAbstract)
            .Select(s => $"        PolymorphicSerializationResolver.TryAddDefaultMapper(new {s.MetadataName}Mapper());")
            .Aggregate((a, b) => $"{a}\n{b}");
        string project = namespaceName.Replace(".", string.Empty);
        return $$"""
                 namespace {{namespaceName}}.Extensions;

                 {{usings}}
                 using Microsoft.Extensions.DependencyInjection;
                 using Microsoft.Extensions.DependencyInjection.Extensions;
                 using System;
                 using Hexalith.PolymorphicSerialization;

                 public static class {{project}}
                 {
                     public static IServiceCollection Add{{project}}PolymorphicMappers(this IServiceCollection services)
                     {
                         services.TryAddSingleton<PolymorphicSerializationResolver>();
                 {{addSingletonMappers}}
                         return services;
                     }
                     public static void RegisterPolymorphicMappers()
                     {
                 {{addResolverMappers}}
                     }
                 }
                 """;
    }

    private static string GenerateMapperClass(
        (TypeDeclarationSyntax Type, AttributeData Data) syntax,
        INamedTypeSymbol classSymbol,
        string? namespaceName,
        SourceProductionContext context)
    {
        string baseTypeName = classSymbol.IsRecord
            ? _recordBaseTypeName
            : _classBaseTypeName;

        INamedTypeSymbol? baseTypeSymbol = classSymbol.BaseType;

        bool hasParent = baseTypeSymbol != null && baseTypeSymbol.SpecialType != SpecialType.System_Object;
        bool validBaseType = false;

        while (baseTypeSymbol != null && baseTypeSymbol.SpecialType != SpecialType.System_Object)
        {
            if (baseTypeSymbol.Name is _recordBaseTypeName or _classBaseTypeName)
            {
                validBaseType = true;
                break;
            }

            // Check if the base type has the PolymorphicSerialization attribute
            if (baseTypeSymbol
                .GetAttributes()
                .Any(a =>
                    a.AttributeClass?.ToDisplayString() == _serializationMapperAttributeFullName))
            {
                validBaseType = true;
                break;
            }

            baseTypeSymbol = baseTypeSymbol.BaseType;
        }

        if (hasParent && !validBaseType)
        {
            // Emit an error
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "HM001",
                    "Invalid Inheritance",
                    (classSymbol.IsRecord ? "Record" : "Class") + "{0} has a parent but does not inherit from " +
                    baseTypeName + " .",
                    "CodeGeneration",
                    DiagnosticSeverity.Error,
                    true),
                syntax.Type.GetLocation(),
                syntax.Type.Identifier.Text));
        }

        // get the name and the version from the attribute
        TypedConstant nameParam = syntax.Data.ConstructorArguments[0];
        TypedConstant versionParam = syntax.Data.ConstructorArguments[1];
        string name = (nameParam.Value == null || string.IsNullOrWhiteSpace((string?)nameParam.Value))
            ? classSymbol.MetadataName
            : (string)nameParam.Value;

        int version = ((int?)versionParam.Value) ?? 1;
        string typeDiscriminator = /*PolymorphicSerializationAttribute.*/GetTypeName(name, version);
        string inheritance = hasParent ? string.Empty : $" : {baseTypeName}";

        return $$"""
                 namespace {{namespaceName}};

                 using Microsoft.Extensions.DependencyInjection;
                 using Hexalith.PolymorphicSerialization;
                 using System.Runtime.Serialization;

                 [DataContract]
                 public partial {{(classSymbol.IsRecord ? "record" : "class")}} {{classSymbol.MetadataName}}{{inheritance}} {}

                 public sealed record {{classSymbol.MetadataName}}Mapper() : PolymorphicSerializationMapper<{{classSymbol.MetadataName}},{{baseTypeName}}>("{{typeDiscriminator}}")
                 {
                 }
                 """;
    }

    private static (TypeDeclarationSyntax? Type, AttributeData? Data) GetSemanticTargetForGeneration(
        GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetNode is not TypeDeclarationSyntax classDeclaration)
        {
            return (null, null);
        }

        AttributeData? attribute = context.Attributes
            .FirstOrDefault(a => a.AttributeClass?.Name == _serializationMapperAttributeName);

        return (classDeclaration, attribute);
    }

    private static string GetTypeName(string name, int version)
            => (version < 2) ? name : $"{name}V{version}";
}