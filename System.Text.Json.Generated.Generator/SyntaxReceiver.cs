using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Generated.Generator.Helpers;
using System.Text.Json.Generated.Generator.Models;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Text.Json.Generated.Generator
{
    public class SyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly CancellationToken _cancellationToken;

        public List<SerializationType> Types = new();

        public SyntaxReceiver(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            try
            {
                if (context.Node is not ClassDeclarationSyntax or RecordDeclarationSyntax or StructDeclarationSyntax)
                    return;

                var typeSymbol =
                    (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node, _cancellationToken)!;

                foreach (var attribute in typeSymbol.GetAttributes())
                {
                    if (!IsGenerateJsonSerializerAttribute(attribute)) return;

                    Logger.Log(
                        $"Found class {typeSymbol.Name} ({context.Node.GetLocation().GetMappedLineSpan()}) with GenerateJsonSerializerAttribute");

                    var properties = new List<SerializerTypeProperty>();

                    foreach (var member in typeSymbol.GetMembers())
                    {
                        if (member is not IPropertySymbol property)
                            continue;

                        Logger.Log($"Found property {property.Name} on type");

                        var propertyType = GetPropertyJsonType(property);

                        properties.Add(new SerializerTypeProperty(property.Name, propertyType));
                    }

                    var declarationType = GetTypeDeclarationType(context.Node);

                    Types.Add(new SerializationType(typeSymbol.Name, typeSymbol.ContainingNamespace.GetFullName(),
                        declarationType, properties));

                    return;
                }
            }
            catch (Exception e)
            {
                Logger.ReportDiagnostic(Diagnostic.Create(Diags.UnknownAnalyzerError, context.Node.GetLocation(), e));
            }
        }

        private static DeclarationType GetTypeDeclarationType(SyntaxNode node)
        {
            return node switch
            {
                ClassDeclarationSyntax => DeclarationType.Class,
                RecordDeclarationSyntax => DeclarationType.Record,
                StructDeclarationSyntax => DeclarationType.Struct,
                _ => throw new Exception($"Unknown declaration type: {node.GetType()}")
            };
        }

        private static PropertyJsonType GetPropertyJsonType(IPropertySymbol property)
        {
            return property.Type.SpecialType switch
            {
                SpecialType.System_Boolean => PropertyJsonType.Boolean,
                SpecialType.System_Int16 or SpecialType.System_Int32 or SpecialType.System_Int64 => PropertyJsonType
                    .Number,
                SpecialType.System_String => PropertyJsonType.String,
                _ when IsDictionary(property.Type) => PropertyJsonType.Dictionary,
                _ => PropertyJsonType.Object
            };
        }

        private static bool IsDictionary(ITypeSymbol type)
        {
            return type.AllInterfaces.Any(i => i is
            {
                Name: "IDictionary",
                ContainingNamespace:
                {
                    Name: "Generic",
                    ContainingNamespace:
                    {
                        Name: "Collections",
                        ContainingNamespace: { Name: "System", ContainingNamespace: { IsGlobalNamespace: true } }
                    }
                }
            });
        }
        
        private static bool IsGenerateJsonSerializerAttribute(AttributeData attribute)
        {
            return attribute.AttributeClass?.Name is "GenerateJsonSerializerAttribute" or "GenerateJsonSerializer";
        }
    }
}
