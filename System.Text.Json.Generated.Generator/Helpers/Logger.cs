using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace System.Text.Json.Generated.Generator.Helpers
{
    public static class Logger
    {
        private static readonly List<string> Lines = new();
        public static readonly List<Diagnostic> Diagnostics = new();

        public static string GetAsCommentedSource()
        {
            return "/*\n" + string.Join("\n", Lines) + "\n*/";
        }

        public static List<string> GetLines()
        {
            return Lines;
        }

        public static void Log(string message)
        {
            Lines.Add(message);
        }

        public static void ReportDiagnostic(Diagnostic diagnostic)
        {
            Diagnostics.Add(diagnostic);
        }
    }
}