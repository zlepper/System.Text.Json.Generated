using System.Linq;
using System.Text.Json.Generated.Generator.Helpers;
using System.Text.Json.Generated.Generator.Models;
using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator
{
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
