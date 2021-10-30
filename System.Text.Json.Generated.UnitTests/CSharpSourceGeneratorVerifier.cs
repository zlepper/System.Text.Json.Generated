using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Generated.Generator;
using System.Text.Json.Generated.Generator.Models;
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
        private static CSharpSourceGeneratorVerifier<MainGenerator>.Test SimpleTest(string[] code, string[] expectedCode, string[] filenames, IEnumerable<IWellKnownType> wellKnownTypes)
        {
            var test = new CSharpSourceGeneratorVerifier<MainGenerator>.Test();
            foreach (var s in code)
            {
                test.TestState.Sources.Add(s);
            }
            
            foreach (var (expected, filename) in expectedCode.Zip(filenames))
            {
                test.TestState.GeneratedSources.Add((typeof(MainGenerator), $"{filename}.cs", SourceText.From(expected, Encoding.UTF8)));
            }
            var wellKnownTypesSerializerCode = MainGenerator.GetWellKnownTypeSerializerCode(wellKnownTypes);
            test.TestState.GeneratedSources.Add((typeof(MainGenerator), $"{MainGenerator.ForeignTypeSerializerFileName}.cs", SourceText.From(wellKnownTypesSerializerCode, Encoding.UTF8)));

            return test;
        }

        public static void RunSimpleTest(string code, string expected, string filename, params IWellKnownType[] wellKnownTypes)
        {
            RunSimpleTest(code, expected, filename, wellKnownTypes.AsEnumerable());
        }

        public static void RunSimpleTest(string code, string expected, string filename, IEnumerable<IWellKnownType> wellKnownTypes)
        {
            RunSimpleTest(new []{code}, new []{expected}, new []{filename}, wellKnownTypes);
        }

        public static void RunSimpleTest(string[] code, string[] expected, string[] filename,
            params IWellKnownType[] wellKnownTypes)
        {
            RunSimpleTest(code, expected, filename, wellKnownTypes.AsEnumerable());
        }

        public static void RunSimpleTest(string[] code, string[] expected, string[] filename, IEnumerable<IWellKnownType> wellKnownTypes)
        {
            var test = SimpleTest(code, expected, filename, wellKnownTypes);
            test.RunAsync().GetAwaiter().GetResult();
        }

    }
}
