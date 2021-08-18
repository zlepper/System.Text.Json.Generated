using NUnit.Framework;

namespace System.Text.Json.Generated.UnitTests
{
    [TestFixture]
    public class DictionarySerialization
    {
        [Test]
        public void HandleStringInt()
        {
            var code = @"
using System.Collections.Generic;
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public Dictionary<string, int> MyDict { get; set; } = new();
    }
}
";
            
            var expected = @"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

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
            writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair in MyDict)
            {
                writer.WriteNumber(keyValuePair.Key, keyValuePair.Value);
            }
            writer.WriteEndObject();
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText MyDictPropertyName = JsonEncodedText.Encode(""MyDict"");
    }
}
";

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        [Test]
        public void HandleIntString()
        {
            var code = @"
using System.Collections.Generic;
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public Dictionary<int, string> MyDict { get; set; } = new();
    }
}
";
            
            var expected = @"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

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
            writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair in MyDict)
            {
                writer.WriteString(keyValuePair.Key.ToString(CultureInfo.InvariantCulture), keyValuePair.Value);
            }
            writer.WriteEndObject();
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText MyDictPropertyName = JsonEncodedText.Encode(""MyDict"");
    }
}
";

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        [Test]
        public void HandleStringString()
        {
            var code = @"
using System.Collections.Generic;
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public Dictionary<string, string> MyDict { get; set; } = new();
    }
}
";
            
            var expected = @"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

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
            writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair in MyDict)
            {
                writer.WriteString(keyValuePair.Key, keyValuePair.Value);
            }
            writer.WriteEndObject();
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText MyDictPropertyName = JsonEncodedText.Encode(""MyDict"");
    }
}
";

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
        [Test]
        public void HandleStringBool()
        {
            var code = @"
using System.Collections.Generic;
using System.Text.Json.Generated;

namespace MyCode
{
    [GenerateJsonSerializer]
    public partial class MyClass
    {
        public Dictionary<string, bool> MyDict { get; set; } = new();
    }
}
";
            
            var expected = @"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

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
            writer.WriteStartObject(MyClassSerializerConstants.MyDictPropertyName);
            foreach(var keyValuePair in MyDict)
            {
                writer.WriteBoolean(keyValuePair.Key, keyValuePair.Value);
            }
            writer.WriteEndObject();
        }
    }
    
    internal class MyClassSerializerConstants 
    {
        internal static readonly JsonEncodedText MyDictPropertyName = JsonEncodedText.Encode(""MyDict"");
    }
}
";

            VerifyMainGenerator.RunSimpleTest(code, expected, "MyCode.MyClass");
        }
    }
}
