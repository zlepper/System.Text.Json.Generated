using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator.Helpers
{
    public static class TypeExtensions
    {
        public static string GetFullName(this INamespaceSymbol ns)
        {
            var output = new List<string>();

            do
            {
                output.Add(ns.Name);
                ns = ns.ContainingNamespace;
            } while (!ns.IsGlobalNamespace);

            output.Reverse();
            return string.Join(".", output);
        }
    }
}
