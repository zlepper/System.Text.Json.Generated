using BenchmarkDotNet.Running;

namespace System.Text.Json.Generated.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Tests>();
        }
    }
}