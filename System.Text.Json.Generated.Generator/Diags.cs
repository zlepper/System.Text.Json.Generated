using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator
{
    public static class Diags
    {
        public static readonly DiagnosticDescriptor UnknownAnalyzerError = new("ZL0666", "Unknown analyzer error",
            "Analyzer error occurred: {0}", "ERROR", DiagnosticSeverity.Warning, true);

        public static readonly DiagnosticDescriptor PropertyIsNotSerializable = new(
            "ZL0001",
            "Property is not serializable",
            "The property {0} is not serializable. Please add the [GenerateJsonSerializer] attribute to the {1} class.",
            "ERROR",
            DiagnosticSeverity.Error,
            true);

        public static readonly DiagnosticDescriptor InnerClassesAreNotSupported = new("ZL0002",
            "Inner classes are not supported", "The class {0} is an inner class of {1}, this is not supported", "ERROR",
            DiagnosticSeverity.Error, true);
    }
}
