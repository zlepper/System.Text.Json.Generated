using System.Collections.Generic;
using System.Text.Json.Generated.Generator.Helpers;
using System.Text.Json.Generated.Generator.Models;
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
            if (context.SyntaxContextReceiver is not SyntaxReceiver sr)
            {
                return;
            }
            
            GenerateSerializers(sr.Types, context);
            
            DumpDiagnostics(context);
            // DumpLogs(context);

        }

        private void GenerateSerializers(List<SerializationType> types, GeneratorExecutionContext context)
        {
            var template = new TemplateExecutor("serializer");

            foreach (var type in types)
            {
                var source = template.Render(new
                {
                    Type = type
                });
                
                context.AddSource($"{type.Namespace}.{type.Name}", source);
            }
        }

        private static void DumpDiagnostics(GeneratorExecutionContext context)
        {
            foreach (var diagnostic in Logger.Diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void DumpLogs(GeneratorExecutionContext context)
        {
            context.AddSource("Logs", Logger.GetAsCommentedSource());
        }
    }
}
