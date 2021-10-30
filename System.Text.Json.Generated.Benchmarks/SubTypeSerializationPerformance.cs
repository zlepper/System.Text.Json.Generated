using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace System.Text.Json.Generated.Benchmarks;

public class SubTypeSerializationPerformance
{
    
    private Utf8JsonWriter _jsonWriter = null!;

    private MemoryStream _memoryStream = null!;

    public List<Animal> TheWholeFarm = null!;

    [GlobalSetup]
    public void Setup()
    {
        _memoryStream = new MemoryStream();
        _jsonWriter = new Utf8JsonWriter(_memoryStream);

        TheWholeFarm = new List<Animal>
        {
            new Dog()
            {
                Name = "The dog",
                BonesChewed = 4
            },
            new Cat()
            {
                Name = "The cat",
                MiceHunted = 10
            },
            new Dog()
            {
                Name = "The old dog",
                BonesChewed = 15
            },
            new Cat()
            {
                Name = "The old cat",
                MiceHunted = 666
            }
        };
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
        JsonSerializer.Serialize(_jsonWriter, TheWholeFarm);
        _memoryStream.SetLength(0);
        _jsonWriter.Reset();
    }
    
    [Benchmark]
    public void UsingThisGenerator()
    {
        ForeignTypeSerializer.SerializeToJson(TheWholeFarm, _jsonWriter);
        _jsonWriter.Flush();
        _memoryStream.SetLength(0);
        _jsonWriter.Reset();
    }
}

[GenerateJsonSerializer]
[JsonConverter(typeof(AnimalSerializer))]
public abstract partial class Animal
{
    public string Name { get; set; }
}

[GenerateJsonSerializer]
public partial class Dog : Animal
{
    public int BonesChewed { get; set; }
}

[GenerateJsonSerializer]
public partial class Cat : Animal
{
    public int MiceHunted { get; set; }
}

public class AnimalSerializer : JsonConverter<Animal>
{
    public override Animal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Animal value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
