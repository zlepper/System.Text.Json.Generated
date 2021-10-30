using System.Linq;
using System.Text.Json.Generated.Generator.Helpers;
using System.Text.Json.Generated.Generator.Models;
using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator
{
    public class ListInspector
    {
        public static bool IsList(ITypeSymbol type)
        {
            return type.AllInterfaces.Any(IsCollectionInterface);
        }
        
        private static bool IsCollectionInterface(INamedTypeSymbol i)
        {
            return i is
            {
                Name: "ICollection",
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
        
        public static ITypeSymbol GetTypeArgument(ITypeSymbol type)
        {
            var dictInterface = type.AllInterfaces.Single(IsCollectionInterface);
            var typeArguments = dictInterface.TypeArguments;

            return typeArguments[0];
        }

    }
    
    public class DictionaryInspector
    {
        public static bool IsDictionary(ITypeSymbol type)
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

        public static (ITypeSymbol Key, ITypeSymbol Value) GetTypeArguments(ITypeSymbol type)
        {
            var dictInterface = type.AllInterfaces.Single(IsDictionaryInterface);
            var typeArguments = dictInterface.TypeArguments;

            return (typeArguments[0], typeArguments[1]);
        }

    }
}
