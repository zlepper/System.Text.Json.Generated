using System.Collections.Generic;

namespace System.Text.Json.Generated.Generator.Models
{
    public record SerializationType(string Name, string Namespace, DeclarationType DeclarationType,
        List<SerializerProperty> Properties);


    public record SerializerProperty(string Name, PropertyJsonType JsonType);
    public record SerializerDictionaryProperty(string Name, DictionaryPropertyType DictionaryPropertyType) : SerializerProperty(Name, PropertyJsonType.Dictionary);

    public record DictionaryPropertyType(JsonKeyType KeyType, PropertyJsonType ValueType);

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
        Object,
        Dictionary,
    }

    public enum JsonKeyType
    {
        String,
        Number,
    }
}
