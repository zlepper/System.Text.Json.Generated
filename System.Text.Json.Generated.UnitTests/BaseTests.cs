namespace System.Text.Json.Generated.UnitTests
{
    public abstract class BaseTests
    {
        protected string GetCode(string propertyType, string propertyName, string defaultValue, string className = "MyClass")
        {
            return $@"
using System.Text.Json.Generated;
using System.Collections.Generic;

namespace MyCode
{{
    [GenerateJsonSerializer]
    public partial class {className}
    {{
        public {propertyType} {propertyName} {{ get; set; }} = {defaultValue};
    }}
}}
";
        }

        protected string GetExpected(string propertyName, string writePropertiesBody, string className = "MyClass")
        {
            return $@"using System.Text.Json.Generated;
using System.Text.Json;
using System.Globalization;

namespace MyCode
{{
    public partial class {className} : IJsonSerializable
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
    
    internal class {className}SerializerConstants 
    {{
        internal static readonly JsonEncodedText {propertyName}PropertyName = JsonEncodedText.Encode(""{propertyName}"");
    }}
}}
";
        }

        
        protected string SimpleWriteCall(string propertyName, string method, string className = "MyClass")
        {
            return GetExpected(propertyName,
                $"writer.{method}({className}SerializerConstants.{propertyName}PropertyName, {propertyName});", className);
        }

        
        protected string GetNestedObjectSerializationCall(string propertyName, string className)
        {
            return @$"writer.WritePropertyName({className}SerializerConstants.{propertyName}PropertyName);
            {propertyName}.SerializeToJson(writer);";
        }
    }
}
