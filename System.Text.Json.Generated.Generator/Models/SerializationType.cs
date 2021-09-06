using System.Collections.Generic;

namespace System.Text.Json.Generated.Generator.Models
{
    public record SerializationType(string Name, string Namespace, DeclarationType DeclarationType,
        List<SerializerProperty> Properties);


    public record SerializerProperty(string Name, PropertyJsonValueType JsonType);
    public record SerializerDictionaryProperty(string Name, DictionaryPropertyType DictionaryPropertyType) : SerializerProperty(Name, PropertyJsonValueType.Dictionary);

    public record DictionaryPropertyType(JsonKeyType KeyType, PropertyJsonValueType ValueType);

    public record DictionaryDictionaryTypePropertyType(JsonKeyType KeyType, DictionaryPropertyType DictionaryPropertyType) : DictionaryPropertyType(KeyType, PropertyJsonValueType.Dictionary);

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
        Dictionary,
    }

    public enum JsonKeyType
    {
        String,
        Number,
    }
}
