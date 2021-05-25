using BenchmarkDotNet.Running;

namespace VirtoCommerce.MarketingModule.Benchmark.PromoPolicies
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            new PolicyBenchmark().EvaluateBestReward(); // Debug
            new PolicyBenchmark().EvaluateStackable(); // Debug
            */

            BenchmarkRunner.Run<PolicyBenchmark>(); // Test
        }
    }
}
