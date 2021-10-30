using System.Collections.Generic;

namespace System.Text.Json.Generated.Generator.Models
{
    public record SerializationType(string Name, string Namespace, DeclarationType DeclarationType,
        List<SerializerProperty> Properties, bool HasParent);


    public record SerializerProperty(string Name, PropertyJsonValueType JsonType);

    public enum DeclarationType
    {
        Class,
        Record,
        Struct
    }

    public enum PropertyJsonValueType
    {
        Boolean,
        Number,
        String,
        Object,
        External
    }
}
