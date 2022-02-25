using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace System.Text.Json.Generated.Benchmarks;

public class DeserializationBenchmark
{
    
    public MySecondClass MyInstance = null!;

    public const string InputString =
        @"{""Int1"":41,""Int2"":42,""Bool1"":false,""Bool2"":true,""String1"":""hej"",""String2"":""Verden""}";

    private byte[] rawBytes = Encoding.UTF8.GetBytes(InputString);

    [GlobalSetup]
    public void Setup()
    {
        MyInstance = new MySecondClass();
    }
    
    

    [GlobalCleanup]
    public void Cleanup()
    {
    }

    // [MethodImpl(MethodImplOptions.NoOptimization)] 
    [Benchmark(Baseline = true)]
    public void BuildinUsingReflection()
    {
        var reader = new Utf8JsonReader(rawBytes);
        var output = JsonSerializer.Deserialize<MyDeserializationClass>(ref reader);
    }

    // [MethodImplAttribute(MethodImplOptions.NoOptimization)] 
    [Benchmark]
    public void UsingDefaultGenerator()
    {
        var reader = new Utf8JsonReader(rawBytes);
        var output = JsonSerializer.Deserialize<MyDeserializationClass>(ref reader, MyDeserializationClassContext.Default.MyDeserializationClass);
    }

    [Benchmark]
    public void UsingHandWrittenParser()
    {
        var reader = new Utf8JsonReader(rawBytes);
        var output = MyDeserializationClass.HandwrittenParseMethod(ref reader);
    }
}

public partial class MyDeserializationClass
{
    
    public int Int1 { get; set; } = 1;
    public int Int2 { get; set; } = 1 << 1;
    public bool Bool1 { get; set; } = true;
    public bool Bool2 { get; set; } = false;
    public string String1 { get; set; } = "hello";
    public string String2 { get; set; } = "World";


    public static MyDeserializationClass HandwrittenParseMethod(ref Utf8JsonReader reader)
    {
        var obj = new MyDeserializationClass();
        var activePropertyName = "";
        
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    activePropertyName = reader.GetString()!.ToLowerInvariant();
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    switch (activePropertyName)
                    {
                        case "string1":
                            obj.String1 = text;
                            break;
                        case "string2":
                            obj.String2 = text;
                            break;
                    }

                    break;
                case JsonTokenType.Number:
                    switch (activePropertyName)
                    {
                        case "int1":
                            obj.Int1 = reader.GetInt32();
                            break;
                        case "int2":
                            obj.Int2 = reader.GetInt32();
                            break;
                    }

                    break;
                case JsonTokenType.True:
                case JsonTokenType.False:
                    switch (activePropertyName)
                    {
                        case "bool1":
                            obj.Bool1 = reader.TokenType == JsonTokenType.True;
                            break;
                        case "bool2":
                            obj.Bool2 = reader.TokenType == JsonTokenType.True;
                            break;
                    }

                    break;
            }
        }

        return obj;
    }
}

[JsonSerializable(typeof(MyDeserializationClass))]
internal partial class MyDeserializationClassContext : JsonSerializerContext {

}
