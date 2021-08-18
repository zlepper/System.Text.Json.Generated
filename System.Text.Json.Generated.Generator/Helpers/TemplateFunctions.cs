using System.Text.Json.Generated.Generator.Models;

namespace System.Text.Json.Generated.Generator.Helpers
{
    public static class TemplateFunctions
    {
        public static string GetWriteMethodName(PropertyJsonType propertyJsonType)
        {
            return propertyJsonType switch
            {
                PropertyJsonType.Boolean => "WriteBoolean",
                PropertyJsonType.Number => "WriteNumber",
                PropertyJsonType.String => "WriteString",
                _ => throw new ArgumentOutOfRangeException(nameof(propertyJsonType), propertyJsonType,
                    $"No method exists for writing {propertyJsonType} directly to output")
            };
        }
    }
}
