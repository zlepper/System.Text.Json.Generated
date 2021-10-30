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
        public HashSet<IWellKnownType> WellKnownTypesToSerialize = new();

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
            catch (CannotGenerateReasonableSerializerException)
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

        public static PropertyJsonValueType GetPropertyJsonType(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => PropertyJsonValueType.Boolean,
                SpecialType.System_Int16 or SpecialType.System_Int32 or SpecialType.System_Int64 => PropertyJsonValueType
                    .Number,
                SpecialType.System_String => PropertyJsonValueType.String,
                _ => PropertyJsonValueType.Object
            };
        }

        private SerializerProperty GetSerializationProperty(IPropertySymbol property)
        {
            if (IsExternalType(property.Type))
            {
                AddWellKnownSerializerType(property.Type, property.Locations.FirstOrDefault());
                return new SerializerProperty(property.Name, PropertyJsonValueType.External);
            }
            
            return new SerializerProperty(property.Name, GetPropertyJsonType(property.Type));
        }


        private IWellKnownType AddWellKnownSerializerType(ITypeSymbol type, Location? propertyLocation)
        {
            if (DictionaryInspector.IsDictionary(type))
            {
                var (keyType, valueType) = DictionaryInspector.GetTypeArguments(type);
                var keyJsonType = GetPropertyJsonType(keyType);
                if (keyJsonType is not PropertyJsonValueType.String and not PropertyJsonValueType.Number)
                {
                    Logger.ReportDiagnostic(Diagnostic.Create(Diags.InvalidDictionaryKeyType, propertyLocation, keyType.ToDisplayString()));
                    throw new CannotGenerateReasonableSerializerException();
                }

                var concreteTypeName = type.GetGlobalName();
                var self = new WellKnownDictionary(GetSimpleTypeName(keyType), concreteTypeName, AddWellKnownSerializerType(valueType, propertyLocation));
                WellKnownTypesToSerialize.Add(self);
                return self;
            } 
            else if (ListInspector.IsList(type))
            {
                var concreteTypeName = type.GetGlobalName();
                var self = new WellKnownList(concreteTypeName, AddWellKnownSerializerType(ListInspector.GetTypeArgument(type), propertyLocation));
                WellKnownTypesToSerialize.Add(self);
                return self;
            }
            else if(IsSimpleType(type))
            {
                return new WellKnownValueType(GetSimpleTypeName(type));
            }
            else
            {
                return new SerializableValueType(type.GetGlobalName());
            }
        }

        private static bool IsExternalType(ITypeSymbol type)
        {
            return DictionaryInspector.IsDictionary(type) || ListInspector.IsList(type);
        }
        
        private static bool IsSimpleType(ITypeSymbol type)
        {
            return GetPropertyJsonType(type) is PropertyJsonValueType.Boolean or PropertyJsonValueType.Number
                or PropertyJsonValueType.String;
        }
        
        
        public static string GetSimpleTypeName(ITypeSymbol type)
        {
            return type.SpecialType switch
            {
                SpecialType.System_Boolean => "bool",
                SpecialType.System_Int16 => "short",
                SpecialType.System_Int32 => "int",
                SpecialType.System_Int64 => "long",
                SpecialType.System_UInt32 => "uint",
                SpecialType.System_UInt64 => "ulong",
                SpecialType.System_String => "string",
                _ => throw new ArgumentOutOfRangeException(nameof(type.SpecialType), type.SpecialType, "Unknown simple type")
            };
        }
        
        private static bool IsGenerateJsonSerializerAttribute(AttributeData attribute)
        {
            return attribute.AttributeClass?.Name is "GenerateJsonSerializerAttribute" or "GenerateJsonSerializer";
        }
    }
}
