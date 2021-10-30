using System.Collections.Generic;
using System.Linq;
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
            if (context.SyntaxContextReceiver is not SyntaxReceiver sr) return;

            GenerateSerializers(sr.Types, sr.WellKnownTypesToSerialize, context);

            DumpDiagnostics(context);
            // DumpLogs(context);
        }

        private void GenerateSerializers(List<SerializationType> types,
            HashSet<IWellKnownType> wellKnownTypesToSerialize, GeneratorExecutionContext context)
        {
            var template = new TemplateExecutor("serializer");

            foreach (var type in types)
            {
                try
                {
                    var source = template.Render(new
                    {
                        Type = type
                    });

                    context.AddSource($"{type.Namespace}.{type.Name}", source);
                }
                catch (Exception e)
                {
                    Logger.ReportDiagnostic(Diagnostic.Create(Diags.UnknownAnalyzerError, null, e));
                }
            }

            try
            {
                var source = GetWellKnownTypeSerializerCode(wellKnownTypesToSerialize);
                context.AddSource(ForeignTypeSerializerFileName, source);
            }
            catch (Exception e)
            {
                Logger.ReportDiagnostic(Diagnostic.Create(Diags.UnknownAnalyzerError, null, e));
            }
        }

        private static void DumpDiagnostics(GeneratorExecutionContext context)
        {
            foreach (var diagnostic in Logger.Diagnostics) context.ReportDiagnostic(diagnostic);
        }

        private static void DumpLogs(GeneratorExecutionContext context)
        {
            context.AddSource("Logs", Logger.GetAsCommentedSource());
        }

        public static void ValidateTemplates()
        {
            var _ = new TemplateExecutor("serializer");
        }

        public static string GetWellKnownTypeSerializerCode(IReadOnlyCollection<IWellKnownType> wellKnownTypes)
        {
            var additionalTypes = wellKnownTypes.OfType<SerializableValueType>()
                .Select(type => new WellKnownList("global::System.Collections.Generic.IEnumerable", type));
            
            var fullSet = wellKnownTypes
                .SelectMany(self => new[] { self }.Concat(self.GetNestedTypes()))
                .Where(type => type is not WellKnownValueType)
                .Concat(additionalTypes)
                .Distinct()
                .ToList();
            
            fullSet.Sort();
            
            var builder = new StringBuilder(@"using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Generated
{
    internal static class ForeignTypeSerializer
    {");

            foreach (var type in fullSet)
            {
                builder.AppendLine(type.CreateMethod());
            }

            builder.AppendLine(@"
    }
}");

            return builder.ToString();
        }
        
        public const string ForeignTypeSerializerFileName = "System.Text.Json.Generated.ForeignTypeSerializer";
    }
}
