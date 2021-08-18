using System.Collections.Immutable;
using System.Text.Json.Generated.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;

namespace System.Text.Json.Generated.UnitTests
{
    public static class CSharpSourceGeneratorVerifier<TSourceGenerator>
        where TSourceGenerator : ISourceGenerator, new()
    {
        public class Test : CSharpSourceGeneratorTest<TSourceGenerator, NUnitVerifier>
        {
            public Test()
            {
                TestState.AdditionalReferences.Add(typeof(GenerateJsonSerializerAttribute).Assembly);
                TestState.ReferenceAssemblies =
                    ReferenceAssemblies.NetStandard.NetStandard20.AddPackages(
                        ImmutableArray.Create(new PackageIdentity("System.Text.Json", "5.0.2")));
            }

            public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Default;

            protected override CompilationOptions CreateCompilationOptions()
            {
                var compilationOptions = base.CreateCompilationOptions();
                return compilationOptions.WithSpecificDiagnosticOptions(
                    compilationOptions.SpecificDiagnosticOptions.SetItems(GetNullableWarningsFromCompiler()));
            }

            private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
            {
                string[] args = { "/warnaserror:nullable" };
                var commandLineArguments = CSharpCommandLineParser.Default.Parse(args, Environment.CurrentDirectory,
                    Environment.CurrentDirectory);
                var nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

                return nullableWarnings;
            }

            protected override ParseOptions CreateParseOptions()
            {
                return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
            }
        }
    }

    public class VerifyMainGenerator 
    {
        public static CSharpSourceGeneratorVerifier<MainGenerator>.Test SimpleTest(string code, string expected, string filename)
        {
            return new CSharpSourceGeneratorVerifier<MainGenerator>.Test
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(MainGenerator), $"{filename}.cs", SourceText.From(expected, Encoding.UTF8))
                    }
                }
            };
        }

        public static void RunSimpleTest(string code, string expected, string filename)
        {
            var test = SimpleTest(code, expected, filename);
            test.RunAsync().GetAwaiter().GetResult();
        }

    }
}
