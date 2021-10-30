using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace System.Text.Json.Generated.Benchmarks
{
    public class BaseSerializerPerformance
    {
        private Utf8JsonWriter _jsonWriter = null!;

        private MemoryStream _memoryStream = null!;
        public MySecondClass MyInstance = null!;

        [GlobalSetup]
        public void Setup()
        {
            _memoryStream = new MemoryStream();
            _jsonWriter = new Utf8JsonWriter(_memoryStream);
            MyInstance = new MySecondClass();
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
            JsonSerializer.Serialize(_jsonWriter, MyInstance);
            _memoryStream.SetLength(0);
            _jsonWriter.Reset();
        }


        [Benchmark]
        public void StandardSourceGenerator()
        {
            JsonSerializer.Serialize(_jsonWriter, MyInstance, MySecondClassJsonContext.Default.MySecondClass);
            _memoryStream.SetLength(0);
            _jsonWriter.Reset();
        }

        [Benchmark]
        public void UsingThisGenerator()
        {
            MyInstance.SerializeToJson(_jsonWriter);
            _jsonWriter.Flush();
            _memoryStream.SetLength(0);
            _jsonWriter.Reset();
        }
    }

    [GenerateJsonSerializer]
    public partial class MyInnerClass
    {
        public int InnerInt1 { get; set; } = 1;
        public int InnerInt2 { get; set; } = 1 << 1;
        public int InnerInt3 { get; set; } = 2 << 2;
        public int InnerInt4 { get; set; } = 3 << 3;
    }

    [GenerateJsonSerializer]
    public partial class MySecondClass
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

        public Dictionary<string, MyInnerClass> InnerObj { get; set; } = new()
        {
            {"foo", new MyInnerClass()},
            {"bar", new MyInnerClass()},
        };
        public MyInnerClass InnerObj2 { get; set; } = new MyInnerClass();
        public List<MyInnerClass> InnerObj3 { get; set; } = Enumerable.Range(0, 5).Select(_ => new MyInnerClass()).ToList();
    }

    [JsonSerializable(typeof(MySecondClass))]
    [JsonSerializable(typeof(MyInnerClass))]
    internal partial class MySecondClassJsonContext : JsonSerializerContext
    {
    }
}
