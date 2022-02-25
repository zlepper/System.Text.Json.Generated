namespace System.Text.Json.Generated
{
    public interface IJsonSerializable<out TSelf>
        where TSelf : IJsonSerializable<TSelf>
    {
        void SerializeToJson(Utf8JsonWriter writer);
        // static abstract TSelf Parse(ref Utf8JsonReader reader);
    }
}
