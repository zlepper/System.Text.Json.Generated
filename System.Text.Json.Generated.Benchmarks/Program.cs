using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace System.Text.Json.Generated.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // BenchmarkRunner.Run<BaseSerializerPerformance>();
            // BenchmarkRunner.Run<SubTypeSerializationPerformance>();
            var cfg = ManualConfig.CreateMinimumViable()
                .AddJob(Job.InProcess);
            BenchmarkRunner.Run<DeserializationBenchmark>(cfg);

            //


            // var c = new MyDeserializationClass();
            // Console.WriteLine(JsonSerializer.Serialize(c));
        }
    }
}
