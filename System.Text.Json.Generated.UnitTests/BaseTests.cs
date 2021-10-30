using System.Text.Json.Generated.Generator.Models;

namespace System.Text.Json.Generated.UnitTests
{
    public abstract class BaseTests
    {
        protected string GetCode(string propertyType, string propertyName, string defaultValue,
            string className = "MyClass")
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

        protected string GetCode(string propertyType, string propertyName, string defaultValue, string childClassName,
            string parentClassName)
        {
            return $@"
using System.Text.Json.Generated;
using System.Collections.Generic;

namespace MyCode
{{
    [GenerateJsonSerializer]
    public partial class {childClassName} : {parentClassName}
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
        
        protected virtual void WriteProperties(Utf8JsonWriter writer)
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
                GetPropertyWriteCall(propertyName, method, className), className);
        }

        protected static string GetPropertyWriteCall(string propertyName, string method, string className)
        {
            return $"writer.{method}({className}SerializerConstants.{propertyName}PropertyName, {propertyName});";
        }

        protected string GetNestedObjectSerializationCall(string propertyName, string className)
        {
            return @$"writer.WritePropertyName({className}SerializerConstants.{propertyName}PropertyName);
            {propertyName}.SerializeToJson(writer);";
        }

        protected string GetSerializeToJsonWriteCall(string propertyName, string className = "MyClass")
        {
            return @$"writer.WritePropertyName({className}SerializerConstants.{propertyName}PropertyName);
            ForeignTypeSerializer.SerializeToJson({propertyName}, writer);";
        }

        protected IWellKnownType GetWellKnownType(string typeName = "MyCode.MyClass")
        {
            return new SerializableValueType($"global::{typeName}");
        }
    }
}
