using System.Collections.Immutable;
using System.Text.Json.Generated.Generator;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using VerifyCS = System.Text.Json.Generated.UnitTests.CSharpSourceGeneratorVerifier<System.Text.Json.Generated.Generator.MainGenerator>;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class FuckAroundAndFigureOut
    {
        [Test]
        public async Task Something()
        {
            var code = @"
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public string String1 { get; set; } = ""Hello"";
    }
}
";

            var expected = @"using System.Text.Json.Generated;
using System.Text.Json;

namespace MyCode
{
    public partial class MyClass : IJsonSerializable
    {
        public void SerializeToJson(Utf8JsonWriter writer) 
        {
            writer.WriteStartObject();
            
            WriteProperties(writer);
            
            writer.WriteEndObject();
        }
        
        protected void WriteProperties(Utf8JsonWriter writer)
        {
            
            writer.WriteString(MyClassSerializerConstants.String1PropertyName, String1);
            
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText String1PropertyName = JsonEncodedText.Encode(""String1"");
    }
}
";

            await new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(MainGenerator), "MyCode.MyClass.cs", SourceText.From(expected, Encoding.UTF8))
                    },
                },
            }.RunAsync();
        }
    }
}
