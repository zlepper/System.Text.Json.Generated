using System.Collections.Generic;
using System.Linq;

namespace System.Text.Json.Generated.Generator.Models;

public interface IWellKnownType : IComparable
{
    string CreateMethod();
    bool SerializesSerializableType();
    string GetTypeName();
    IEnumerable<IWellKnownType> GetNestedTypes();
}

public record WellKnowDictionary(string KeyType, string ConcreteDictionaryType, IWellKnownType ValueType) : IWellKnownType, IComparable<WellKnowDictionary>
{
    public string CreateMethod()
    {
        return $@"
        public static void SerializeToJson({GetTypeName()} dict, Utf8JsonWriter writer)
        {{
            writer.WriteStartObject();
            foreach (var keyValuePair in dict)
            {{
{CreateWriteValueMethod()}
            }}
            writer.WriteEndObject();
        }}
";
    }

    private static readonly string[] NumberTypes = {"double" ,"float", "int", "long", "decimal" , "ulong" , "uint" }; 
    
    private bool IsNumber => NumberTypes.Contains(KeyType);


    public bool SerializesSerializableType()
    {
        return ValueType.SerializesSerializableType();
    }

    public string GetTypeName()
    {
        return $"{ConcreteDictionaryType}<{KeyType}, {ValueType.GetTypeName()}>";
    }

    public IEnumerable<IWellKnownType> GetNestedTypes()
    {
        return new[] { ValueType }.Concat(ValueType.GetNestedTypes());
    }

    private string CreateWriteValueMethod()
    {
        var keyString = IsNumber ? "keyValuePair.Key.ToString(CultureInfo.InvariantCulture)" : "keyValuePair.Key";
        if (ValueType is WellKnownValueType wellKnownValueType)
        {
            return $"                writer.{wellKnownValueType.MethodName}({keyString}, keyValuePair.Value);";
        }
        else
        {
            return $@"                writer.WritePropertyName({keyString});
                SerializeToJson(keyValuePair.Value, writer);";
        }
    }

    public int CompareTo(WellKnowDictionary? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var keyTypeComparison = string.Compare(KeyType, other.KeyType, StringComparison.Ordinal);
        if (keyTypeComparison != 0) return keyTypeComparison;
        var concreteDictionaryTypeComparison = string.Compare(ConcreteDictionaryType, other.ConcreteDictionaryType, StringComparison.Ordinal);
        if (concreteDictionaryTypeComparison != 0) return concreteDictionaryTypeComparison;
        return ValueType.CompareTo(other.ValueType);
    }

    public int CompareTo(object obj)
    {
        
        return obj switch
        {
            WellKnowDictionary same => CompareTo(same),
            WellKnownValueType or SerializableValueType or WellKnownList => 1,
            _ => -1,
        };
    }
}

public record WellKnownList(string ConcreteCollectionType, IWellKnownType ValueType) : IWellKnownType, IComparable<WellKnownList>
{
    public string CreateMethod()
    {
        return $@"
        public static void SerializeToJson({GetTypeName()} enumerable, Utf8JsonWriter writer)
        {{
            writer.WriteStartArray();
            foreach (var item in enumerable)
            {{
                {CreateWriteValueMethod()}
            }}
            writer.WriteEndArray();
        }}
";
    }

    public bool SerializesSerializableType()
    {
        return ValueType.SerializesSerializableType();
    }

    public string GetTypeName()
    {
        return $"{ConcreteCollectionType}<{ValueType.GetTypeName()}>";
    }

    public IEnumerable<IWellKnownType> GetNestedTypes()
    {
        return new[] { ValueType }.Concat(ValueType.GetNestedTypes());
    }

    private string CreateWriteValueMethod()
    {
        if (ValueType is WellKnownValueType wellKnownValueType)
        {
            return $"writer.{wellKnownValueType.MethodName}Value(item);";
        }
        else
        {
            return "SerializeToJson(item, writer);";
        }
    }

    public int CompareTo(WellKnownList? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var concreteCollectionTypeComparison = string.Compare(ConcreteCollectionType, other.ConcreteCollectionType, StringComparison.Ordinal);
        if (concreteCollectionTypeComparison != 0) return concreteCollectionTypeComparison;
        return ValueType.CompareTo(other.ValueType);
    }

    public int CompareTo(object obj)
    {
        return obj switch
        {
            WellKnownList same => CompareTo(same),
            WellKnownValueType or SerializableValueType => 1,
            _ => -1,
        };
    }
}

public record WellKnownValueType(string TypeName) : IWellKnownType, IComparable<WellKnownValueType>
{
    public string CreateMethod()
    {
        throw new NotImplementedException("Should not be called directly");
    }

    public bool SerializesSerializableType()
    {
        return false;
    }

    public string GetTypeName()
    {
        return TypeName;
    }

    public IEnumerable<IWellKnownType> GetNestedTypes()
    {
        return Array.Empty<IWellKnownType>();
    }

    public string MethodName => TypeName switch
    {
        "double" or "float" or "int" or "long" or "decimal" or "ulong" or "uint" => "WriteNumber",
        "string" => "WriteString",
        "bool" => "WriteBoolean",
        _ => throw new ArgumentOutOfRangeException(nameof(TypeName), TypeName, "Unknown simple type name")
    };

    public int CompareTo(WellKnownValueType? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(TypeName, other.TypeName, StringComparison.Ordinal);
    }

    public int CompareTo(object obj)
    {

        return obj switch
        {
            WellKnownValueType same => CompareTo(same),
            SerializableValueType => 1,
            _ => -1
        };
    }
}

public record SerializableValueType(string ConcreteTypeName) : IWellKnownType, IComparable<SerializableValueType>
{
    public string CreateMethod()
    {
        return $@"
        public static void SerializeToJson({GetTypeName()} item, Utf8JsonWriter writer)
        {{
            item.SerializeToJson(writer);
        }}
";
    }

    public bool SerializesSerializableType()
    {
        return true;
    }

    public string GetTypeName()
    {
        return ConcreteTypeName;
    }

    public IEnumerable<IWellKnownType> GetNestedTypes()
    {
        return Array.Empty<IWellKnownType>();
    }

    public int CompareTo(SerializableValueType? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(ConcreteTypeName, other.ConcreteTypeName, StringComparison.Ordinal);
    }


    public int CompareTo(object obj)
    {
        if (obj is SerializableValueType same)
        {
            return CompareTo(same);
        }

        return -1;
    }
}
