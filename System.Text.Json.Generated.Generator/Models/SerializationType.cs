using System.Collections.Generic;

namespace System.Text.Json.Generated.Generator.Models
{
    public record SerializationType(string Name, string Namespace, DeclarationType DeclarationType, List<SerializerTypeProperty> Properties);

    public record SerializerTypeProperty(string Name, PropertyJsonType JsonType);

    public enum DeclarationType
    {
        Class,
        Record,
        Struct
    }

    public enum PropertyJsonType
    {
        Boolean,
        Number,
        String,
        Object
    }
}
