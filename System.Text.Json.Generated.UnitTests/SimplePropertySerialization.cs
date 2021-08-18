using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class SimplePropertySerialization
    {
        [Test]
        public void TestStringProperty()
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

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        
        [Test]
        public void TestIntProperty()
        {
            var code = @"
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public int Int1 { get; set; } = 42;
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
            
            writer.WriteNumber(MyClassSerializerConstants.Int1PropertyName, Int1);
            
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText Int1PropertyName = JsonEncodedText.Encode(""Int1"");
    }
}
";

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        
        [Test]
        public void TestBoolProperty()
        {
            var code = @"
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public bool Bool1 { get; set; } = true;
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
            
            writer.WriteBoolean(MyClassSerializerConstants.Bool1PropertyName, Bool1);
            
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText Bool1PropertyName = JsonEncodedText.Encode(""Bool1"");
    }
}
";

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
    }
}
