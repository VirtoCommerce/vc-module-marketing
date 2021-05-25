using BenchmarkDotNet.Running;

namespace VirtoCommerce.MarketingModule.Benchmark.PromoPolicies
{
    internal static class Program
    {
        static void Main()
        {
            /*
            new PolicyBenchmark().EvaluateBestReward(); // Debug
            new PolicyBenchmark().EvaluateStackable(); // Debug
            */

            BenchmarkRunner.Run<PolicyBenchmark>(); // Test
        }
    }
}
