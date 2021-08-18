namespace System.Text.Json.Generated.UnitTests
{
    public abstract class BaseTests
    {
        protected string GetCode(string propertyType, string propertyName, string defaultValue)
        {
            return $@"
using System.Text.Json.Generated;
using System.Collections.Generic;

namespace MyCode
{{
    [GenerateJsonSerializer]
    public partial class MyClass
    {{
        public {propertyType} {propertyName} {{ get; set; }} = {defaultValue};
    }}
}}
";
        }

        protected string GetExpected(string propertyName, string writePropertiesBody)
        {
            return $@"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

namespace MyCode
{{
    public partial class MyClass : IJsonSerializable
    {{
        public void SerializeToJson(Utf8JsonWriter writer) 
        {{
            writer.WriteStartObject();
            
            WriteProperties(writer);
            
            writer.WriteEndObject();
        }}
        
        protected void WriteProperties(Utf8JsonWriter writer)
        {{
            {writePropertiesBody}
        }}
    }}
    
    internal class MyClassSerializerConstants 
    {{
        internal static readonly JsonEncodedText {propertyName}PropertyName = JsonEncodedText.Encode(""{propertyName}"");
    }}
}}
";
        }
    }
}
