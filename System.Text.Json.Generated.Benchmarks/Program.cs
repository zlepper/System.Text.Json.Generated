using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace System.Text.Json.Generated.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<BaseSerializerPerformance>();
            // BenchmarkRunner.Run<SubTypeSerializationPerformance>();
        }
    }
}
