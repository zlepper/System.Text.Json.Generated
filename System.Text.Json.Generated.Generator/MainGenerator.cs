using System.Text.Json.Generated.Generator.Helpers;
using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator
{
    [Generator]
    public class MainGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver(context.CancellationToken));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            DumpDiagnostics(context);
            DumpLogs(context);
        }

        private static void DumpDiagnostics(GeneratorExecutionContext context)
        {
            foreach (var diagnostic in Logger.Diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void DumpLogs(GeneratorExecutionContext context)
        {
            context.AddSource("Logs", Logger.GetAsCommentedSource());
        }
    }
}
