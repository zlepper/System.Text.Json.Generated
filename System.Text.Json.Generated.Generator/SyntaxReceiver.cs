using System.Text.Json.Generated.Generator.Helpers;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Text.Json.Generated.Generator
{
    public class SyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly CancellationToken _cancellationToken;

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
                    if (!IsGenerateJsonSerializerAttribute(attribute))
                    {
                        return;
                    }

                    Logger.Log($"Found class {typeSymbol.Name} with GenerateJsonSerializerAttribute");

                    
                    
                    return;
                }
            }
            catch (Exception e)
            {
                Logger.ReportDiagnostic(Diagnostic.Create(Diags.UnknownAnalyzerError, context.Node.GetLocation(), e));
            }
        }

        private static bool IsGenerateJsonSerializerAttribute(AttributeData attribute)
        {
            return attribute.AttributeClass?.Name is "GenerateJsonSerializerAttribute" or "GenerateJsonSerializer";
        }
    }
}
