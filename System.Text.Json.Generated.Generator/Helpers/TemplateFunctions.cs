using System.Text.Json.Generated.Generator.Models;

namespace System.Text.Json.Generated.Generator.Helpers
{
    public static class TemplateFunctions
    {
        public static string GetWriteMethodName(PropertyJsonValueType propertyJsonType)
        {
            return propertyJsonType switch
            {
                PropertyJsonValueType.Boolean => "WriteBoolean",
                PropertyJsonValueType.Number => "WriteNumber",
                PropertyJsonValueType.String => "WriteString",
                _ => throw new ArgumentOutOfRangeException(nameof(propertyJsonType), propertyJsonType,
                    $"No method exists for writing {propertyJsonType} directly to output")
            };
        }

        public static string GetDictionaryKeyOutput(JsonKeyType keyType, string valueName)
        {
            return keyType switch
            {
                JsonKeyType.Number => $"{valueName}.ToString(CultureInfo.InvariantCulture)",
                JsonKeyType.String => valueName,
                _ => throw new ArgumentOutOfRangeException(nameof(keyType), keyType, "Unknown key type")
            };
        }
    }
}
