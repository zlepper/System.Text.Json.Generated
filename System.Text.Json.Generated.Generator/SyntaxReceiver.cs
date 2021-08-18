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

                    var properties = new List<SerializerProperty>();

                    foreach (var member in typeSymbol.GetMembers())
                    {
                        if (member is not IPropertySymbol property)
                            continue;

                        Logger.Log($"Found property {property.Name} on type");

                        properties.Add(GetSerializationProperty(property));
                    }

                    var declarationType = GetTypeDeclarationType(context.Node);

                    Types.Add(new SerializationType(typeSymbol.Name, typeSymbol.ContainingNamespace.GetFullName(),
                        declarationType, properties));

                    return;
                }
            }
            catch (CannotGenerateResonableSerializerException)
            {
                Logger.ReportDiagnostic(Diagnostic.Create(Diags.CannotGenerateReasonableSerializerMethod, context.Node.GetLocation()));
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

        private static PropertyJsonType GetPropertyJsonType(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => PropertyJsonType.Boolean,
                SpecialType.System_Int16 or SpecialType.System_Int32 or SpecialType.System_Int64 => PropertyJsonType
                    .Number,
                SpecialType.System_String => PropertyJsonType.String,
                _ => PropertyJsonType.Object
            };
        }

        private static bool IsDictionary(ITypeSymbol type)
        {
            return type.AllInterfaces.Any(IsDictionaryInterface);
        }

        private static bool IsDictionaryInterface(INamedTypeSymbol i)
        {
            return i is
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
            };
        }

        private static SerializerProperty GetSerializationProperty(IPropertySymbol property)
        {
            if (IsDictionary(property.Type))
            {
                return GetDictionaryProperty(property);
            }
            else
            {
                return new SerializerProperty(property.Name, GetPropertyJsonType(property.Type));
            }
        }


        private static SerializerDictionaryProperty GetDictionaryProperty(IPropertySymbol property)
        {
            var dictionaryType = GetDictionaryPropertyType(property);
            
            return new SerializerDictionaryProperty(property.Name, dictionaryType);
        }

        private static DictionaryPropertyType GetDictionaryPropertyType(IPropertySymbol property)
        {
            JsonKeyType keyType;

            var type = property.Type;
            var dictInterface = type.AllInterfaces.Single(IsDictionaryInterface);
            var typeArguments = dictInterface.TypeArguments;

            var keyTypeType = typeArguments[0];
            switch (keyTypeType.SpecialType)
            {
                case SpecialType.System_String:
                    keyType = JsonKeyType.String;
                    break;
                case SpecialType.System_Int16:
                case SpecialType.System_Int32:
                case SpecialType.System_Int64:
                    keyType = JsonKeyType.Number;
                    break;
                default:
                    Logger.Log(
                        $"Dictionary type {type} has a key of type {keyTypeType}, for which we cannot generate a proper key in json notation");
                    Logger.ReportDiagnostic(Diagnostic.Create(Diags.InvalidDictionaryKeyType, property.Locations.First(),
                        keyTypeType.Name));
                    throw new CannotGenerateResonableSerializerException();
            }

            var valueTypeType = typeArguments[1];
            var valueType = GetPropertyJsonType(valueTypeType);

            var dictionaryType = new DictionaryPropertyType(keyType, valueType);
            return dictionaryType;
        }

        private static bool IsGenerateJsonSerializerAttribute(AttributeData attribute)
        {
            return attribute.AttributeClass?.Name is "GenerateJsonSerializerAttribute" or "GenerateJsonSerializer";
        }
    }
}
