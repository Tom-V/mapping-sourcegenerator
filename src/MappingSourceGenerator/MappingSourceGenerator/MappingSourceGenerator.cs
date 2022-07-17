﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MappingSourceGenerator
{

    [Generator]
    public class MappingSourceGenerator : ISourceGenerator
    {
        private const string MapToAttribute = @"using System;

namespace MappingSourceGenerator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class MapToAttribute : Attribute
    {
        public MapToAttribute(Type toType, string functionName)
        {
            ToType = toType;
            FunctionName = functionName;
        }

        public Type ToType { get; set; }
        public string FunctionName{ get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class IgnorePropertyAttribute : Attribute
    {
        public Type ToType { get; set; }
    }
}";



        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            INamedTypeSymbol attributeSymbol = context.Compilation.GetTypeByMetadataName("MappingSourceGenerator.MapToAttribute");


            foreach (var classSymbol in receiver.Classes)
            {
                context.AddSource($"{classSymbol.ToDisplayString()}.Mapper.g.cs", SourceText.From(CreateMapper(classSymbol, attributeSymbol), Encoding.UTF8));
            }
        }

        private string CreateMapper(ITypeSymbol symbol, INamedTypeSymbol attributeSymbol)
        {

            var mapToAttributes = symbol.GetAttributes()
                                        .Where(a => a.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            var mapToAttribute = mapToAttributes.First();
            var toType = mapToAttribute.ConstructorArguments[0].Value as ITypeSymbol;
            var functionName = mapToAttribute.ConstructorArguments[1].Value as string;
            var partialFunctionName = $"_{char.ToLower(functionName[0])}{functionName.Substring(1)}";

            var toTypeMembers = toType.GetMembers()
                                      .OfType<IPropertySymbol>()
                                      .Where(p => !p.IsReadOnly);
            var fromTypeMembers = symbol.GetMembers()
                                        .OfType<IPropertySymbol>()
                                        .Where(p => !p.IsWriteOnly &&
                                                    !p.GetAttributes()
                                                      .Any(at => at.AttributeClass.ToDisplayString() == "MappingSourceGenerator.IgnorePropertyAttribute" &&
                                                                 (at.NamedArguments.FirstOrDefault(f => f.Key == "ToType").Value.IsNull ||
                                                                 (at.NamedArguments.First(f => f.Key == "ToType").Value.Value as ITypeSymbol).Equals(toType, SymbolEqualityComparer.Default))));

            var propsToMap = fromTypeMembers.Join(toTypeMembers, from => new { from.Type, from.Name }, to => new { to.Type, to.Name }, (from, to) => from.Name);

            var propMapping = new StringBuilder();
            foreach (var item in propsToMap)
            {
                propMapping.Append("            output.").Append(item).Append(" = input.").Append(item).AppendLine(";");
            }

            var mapper = $@"
namespace {symbol.ContainingNamespace.ToDisplayString()}
{{
    public static partial class Mapper 
    {{
        public static {toType.ToDisplayString()} {functionName}({symbol.ToDisplayString()} input)
        {{
            var output = new {toType.ToDisplayString()}();
{propMapping}
            {functionName}(input, output);

            return output;
        }}

        public static void {functionName}({symbol.ToDisplayString()} input, {toType.ToDisplayString()} output)
        {{
{propMapping}
            {partialFunctionName}(input, output);
        }}

        static partial void {partialFunctionName}({symbol.ToDisplayString()} input, {toType.ToDisplayString()} output);

    }}
}}
";

            return mapper;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 

            context.RegisterForPostInitialization((i) => i.AddSource("MapToAttribute.g.cs", MapToAttribute));
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<ITypeSymbol> Classes { get; } = new List<ITypeSymbol>();

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                // any class with at least one attribute is a candidate for mapping generation
                if (context.Node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.AttributeLists.Count > 0)
                {
                    var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
                    if (symbol.Constructors.Any(c => c.Parameters.Length == 0) && // only with parameterless constructors
                        symbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "MappingSourceGenerator.MapToAttribute"))
                    {
                        Classes.Add(symbol);
                    }

                }
            }
        }
    }
}