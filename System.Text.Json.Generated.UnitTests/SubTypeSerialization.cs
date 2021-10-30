using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests;

[TestFixture]
public class SubTypeSerialization : BaseTests
{
    [Test]
    public void HandlesSubTypes()
    {
        var parent = GetCode("int", "MyInt", "42", "MyParent");
        var child = GetCode("string", "MyString", "null!", "MyChild", "MyParent");

        var expectedParent = SimpleWriteCall("MyInt", "WriteNumber", "MyParent");
        var expectedChildBody = @$"{GetPropertyWriteCall("MyString", "WriteString", "MyChild")}
            base.WriteProperties(writer);";
        var expectedChild = ChildExpected;
        
        VerifyMainGenerator.RunSimpleTest(new []{parent, child}, new []{expectedParent, expectedChild}, new []{"MyCode.MyParent", "MyCode.MyChild"}, GetWellKnownType("MyCode.MyParent"), GetWellKnownType("MyCode.MyChild"));
    }
    
    private static readonly string ChildExpected = $@"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

namespace MyCode
{{
    public partial class MyChild : IJsonSerializable
    {{

        protected override void WriteProperties(Utf8JsonWriter writer)
        {{
            {GetPropertyWriteCall("MyString", "WriteString", "MyChild")}
            base.WriteProperties(writer);
        }}
    }}
    
    internal class MyChildSerializerConstants 
    {{
        internal static readonly JsonEncodedText MyStringPropertyName = JsonEncodedText.Encode(""MyString"");
    }}
}}
";
}
