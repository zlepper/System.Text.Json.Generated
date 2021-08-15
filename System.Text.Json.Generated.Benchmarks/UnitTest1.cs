using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace System.Text.Json.Generated.Benchmarks
{
    public class Tests
    {
        private Utf8JsonWriter _jsonWriter = null!;

        private MemoryStream _memoryStream = null!;
        public MyFirstClass MyFirstInstance = null!;
        public MySecondClass MySecondInstance = null!;
        public MyThirdClass MyThirdInstance = null!;

        [GlobalSetup]
        public void Setup()
        {
            _memoryStream = new MemoryStream();
            _jsonWriter = new Utf8JsonWriter(_memoryStream);
            MyFirstInstance = new MyFirstClass();
            MySecondInstance = new MySecondClass();
            MyThirdInstance = new MyThirdClass();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _memoryStream.Dispose();
            _jsonWriter.Dispose();
        }


        [Benchmark(Baseline = true)]
        public void BuildinUsingReflection()
        {
            JsonSerializer.Serialize(_jsonWriter, MyFirstInstance);
            _memoryStream.SetLength(0);
            _jsonWriter.Reset();
        }


        [Benchmark]
        public void StandardSourceGenerator()
        {
            JsonSerializer.Serialize(_jsonWriter, MySecondInstance, MySecondClassJsonContext.Default.MySecondClass);
            _memoryStream.SetLength(0);
            _jsonWriter.Reset();
        }

        [Benchmark]
        public void Handwritten()
        {
            MyThirdInstance.Serialize(_jsonWriter);
            _memoryStream.SetLength(0);
            _jsonWriter.Reset();
        }
    }

    public class MyInnerClass : IJsonSerializable
    {
        private static readonly JsonEncodedText InnerInt1PropertyName = JsonEncodedText.Encode(nameof(InnerInt1));
        private static readonly JsonEncodedText InnerInt2PropertyName = JsonEncodedText.Encode(nameof(InnerInt2));
        private static readonly JsonEncodedText InnerInt3PropertyName = JsonEncodedText.Encode(nameof(InnerInt3));
        private static readonly JsonEncodedText InnerInt4PropertyName = JsonEncodedText.Encode(nameof(InnerInt4));

        public int InnerInt1 { get; set; } = 1;
        public int InnerInt2 { get; set; } = 1 << 1;
        public int InnerInt3 { get; set; } = 2 << 2;
        public int InnerInt4 { get; set; } = 3 << 3;


        public void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            WriteProperties(writer);

            writer.WriteEndObject();
        }

        protected virtual void WriteProperties(Utf8JsonWriter writer)
        {
            writer.WriteNumber(InnerInt1PropertyName, InnerInt1);
            writer.WriteNumber(InnerInt2PropertyName, InnerInt2);
            writer.WriteNumber(InnerInt3PropertyName, InnerInt3);
            writer.WriteNumber(InnerInt4PropertyName, InnerInt4);
        }
    }

    public class MyFirstClass
    {
        public int Int1 { get; set; } = 1;
        public int Int2 { get; set; } = 1 << 1;
        public int Int3 { get; set; } = 2 << 2;
        public int Int4 { get; set; } = 3 << 3;

        public bool Bool1 { get; set; } = true;
        public bool Bool2 { get; set; } = false;
        public bool Bool3 { get; set; } = true;
        public bool Bool4 { get; set; } = false;

        public string String1 { get; set; } = "hello";
        public string String2 { get; set; } = "World";
        public string String3 { get; set; } = "HEj";
        public string String4 { get; set; } = "Verden";
        public object InnerObj2 { get; set; } = new MyInnerClass();
    }


    public class MySecondClass
    {
        public int Int1 { get; set; } = 1;
        public int Int2 { get; set; } = 1 << 1;
        public int Int3 { get; set; } = 2 << 2;
        public int Int4 { get; set; } = 3 << 3;

        public bool Bool1 { get; set; } = true;
        public bool Bool2 { get; set; } = false;
        public bool Bool3 { get; set; } = true;
        public bool Bool4 { get; set; } = false;

        public string String1 { get; set; } = "hello";
        public string String2 { get; set; } = "World";
        public string String3 { get; set; } = "HEj";
        public string String4 { get; set; } = "Verden";

        public Dictionary<string, MyInnerClass> InnerObj { get; set; } = new();
        public object InnerObj2 { get; set; } = new MyInnerClass();
    }

    [JsonSerializable(typeof(MySecondClass))]
    [JsonSerializable(typeof(MyInnerClass))]
    internal partial class MySecondClassJsonContext : JsonSerializerContext
    {
    }

    public interface IJsonSerializable
    {
        void Serialize(Utf8JsonWriter writer);
    }

    public class MyThirdClass : IJsonSerializable
    {
        private static readonly JsonEncodedText Int1PropertyName = JsonEncodedText.Encode(nameof(Int1));
        private static readonly JsonEncodedText Int2PropertyName = JsonEncodedText.Encode(nameof(Int2));
        private static readonly JsonEncodedText Int3PropertyName = JsonEncodedText.Encode(nameof(Int3));
        private static readonly JsonEncodedText Int4PropertyName = JsonEncodedText.Encode(nameof(Int4));
        private static readonly JsonEncodedText Bool1PropertyName = JsonEncodedText.Encode(nameof(Bool1));
        private static readonly JsonEncodedText Bool2PropertyName = JsonEncodedText.Encode(nameof(Bool2));
        private static readonly JsonEncodedText Bool3PropertyName = JsonEncodedText.Encode(nameof(Bool3));
        private static readonly JsonEncodedText Bool4PropertyName = JsonEncodedText.Encode(nameof(Bool4));
        private static readonly JsonEncodedText String1PropertyName = JsonEncodedText.Encode(nameof(String1));
        private static readonly JsonEncodedText String2PropertyName = JsonEncodedText.Encode(nameof(String2));
        private static readonly JsonEncodedText String3PropertyName = JsonEncodedText.Encode(nameof(String3));
        private static readonly JsonEncodedText String4PropertyName = JsonEncodedText.Encode(nameof(String4));
        private static readonly JsonEncodedText InnerObj2PropertyName = JsonEncodedText.Encode(nameof(InnerObj2));
        public int Int1 { get; set; } = 1;
        public int Int2 { get; set; } = 1 << 1;
        public int Int3 { get; set; } = 2 << 2;
        public int Int4 { get; set; } = 3 << 3;

        public bool Bool1 { get; set; } = true;
        public bool Bool2 { get; set; } = false;
        public bool Bool3 { get; set; } = true;
        public bool Bool4 { get; set; } = false;

        public string String1 { get; set; } = "hello";
        public string String2 { get; set; } = "World";
        public string String3 { get; set; } = "HEj";
        public string String4 { get; set; } = "Verden";


        public object InnerObj2 { get; set; } = new MyInnerClass();

        public virtual void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            WriteProperties(writer);

            writer.WriteEndObject();
        }

        protected virtual void WriteProperties(Utf8JsonWriter writer)
        {
            writer.WriteNumber(Int1PropertyName, Int1);
            writer.WriteNumber(Int2PropertyName, Int2);
            writer.WriteNumber(Int3PropertyName, Int3);
            writer.WriteNumber(Int4PropertyName, Int4);

            writer.WriteBoolean(Bool1PropertyName, Bool1);
            writer.WriteBoolean(Bool2PropertyName, Bool2);
            writer.WriteBoolean(Bool3PropertyName, Bool3);
            writer.WriteBoolean(Bool4PropertyName, Bool4);

            writer.WriteString(String1PropertyName, String1);
            writer.WriteString(String2PropertyName, String2);
            writer.WriteString(String3PropertyName, String3);
            writer.WriteString(String4PropertyName, String4);

            writer.WriteStartObject(InnerObj2PropertyName);

            if (InnerObj2 is IJsonSerializable serializable)
            {
                writer.WritePropertyName(InnerObj2PropertyName);
                serializable.Serialize(writer);
            }
            else
            {
                throw new Exception($"Object with type {InnerObj2?.GetType()} is not `IJsonSerializable`");
            }


            writer.WriteEndObject();
        }
    }
}
