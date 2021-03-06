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

        public static readonly DiagnosticDescriptor CannotGenerateReasonableSerializerMethod = new("ZL0003",
            "Cannot generate reasonable serializer method",
            "Cannot generate reasonable serializer method. Please see other diagnostic errors for specific reasons.",
            "ERROR", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor InvalidDictionaryKeyType = new DiagnosticDescriptor("ZL0101",
            "Invalid dictionary key type",
            "The dictionary key type '{0}' cannot be converted to a proper json key. Please use either a string or a number type.", "ERROR", DiagnosticSeverity.Error, true);
    }
}
