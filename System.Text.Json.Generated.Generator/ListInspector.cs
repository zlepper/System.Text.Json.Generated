using System.Linq;
using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator;

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