using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions;

namespace VirtoCommerce.MarketingModule.Benchmark.PromoConditions;

public class ConditionsBenchmark
{
    private readonly PromotionEvaluationContext _context = MockPromotionEvaluationContext();

    // Customer conditions
    private readonly ConditionIsRegisteredUser _conditionIsRegisteredUser = new();
    private readonly ConditionIsEveryone _conditionIsEveryone = new();
    private readonly ConditionIsFirstTimeBuyer _conditionIsFirstTimeBuyer = new();
    private readonly UserGroupsContainsCondition _userGroupsContainsCondition = new() { Group = "Group7" };

    // Catalog conditions
    private readonly ConditionCategoryIs _conditionCategoryIs = new() { CategoryId = "8B77CD0F-5C4E-4BBA-9AEF-CD3022D0D2C1" };
    private readonly ConditionCodeContains _conditionCodeContains = new() { Keyword = "16-29" };
    private readonly ConditionCurrencyIs _conditionCurrencyIs = new() { Currency = "USD" };
    private readonly ConditionEntryIs _conditionEntryIs = new() { ProductIds = ["4B70F12A-25F8-4225-9A50-68C7E6DA25B3", "4B70F12A-25F8-4225-9A50-68C7E5DA25B2", "4B70F12B-25F8-4225-9A50-68C7E6DA25B2", "4B70F12A-2EF8-4225-9A50-68C7E6DA25B2", "4B70F12A-25F8-4225-9A50-68C7E6DA25B2"] };
    private readonly ConditionInStockQuantity _conditionInStockQuantity = new() { CompareCondition = ConditionOperation.Between, Quantity = 1, QuantitySecond = 100 };
    private readonly ConditionHasNoSalePrice _conditionHasNoSalePrice = new();

    // Cart conditions
    private readonly ConditionAtNumItemsInCart _conditionAtNumItemsInCart = new() { CompareCondition = ConditionOperation.Between, NumItem = 1, NumItemSecond = 100 };
    private readonly ConditionAtNumItemsInCategoryAreInCart _conditionAtNumItemsInCategoryAreInCart = new() { CompareCondition = ConditionOperation.Between, NumItem = 1, NumItemSecond = 100, CategoryId = "8B77CD0F-5C4E-4BBA-9AEF-CD3022D0D2C1" };
    private readonly ConditionAtNumItemsOfEntryAreInCart _conditionAtNumItemsOfEntryAreInCart = new() { CompareCondition = ConditionOperation.Between, NumItem = 1, NumItemSecond = 100, ProductId = "8B77CD0F-5C4E-4BBA-9AEF-CD3022D0D2C1" };
    private readonly ConditionCartSubtotalLeast _conditionCartSubtotalLeast = new() { CompareCondition = ConditionOperation.Between, SubTotal = 1, SubTotalSecond = 100 };

    private static PromotionEvaluationContext MockPromotionEvaluationContext()
    {
        return JsonConvert.DeserializeObject<PromotionEvaluationContext>(File.ReadAllText("promotion_evaluation_context_mock.json"));
    }


    [Benchmark]
    public bool ConditionIsRegisteredUser() => _conditionIsRegisteredUser.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionIsEveryone() => _conditionIsEveryone.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionIsFirstTimeBuyer() => _conditionIsFirstTimeBuyer.IsSatisfiedBy(_context);

    [Benchmark]
    public bool UserGroupsContainsCondition() => _userGroupsContainsCondition.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionCategoryIs() => _conditionCategoryIs.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionCodeContains() => _conditionCodeContains.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionCurrencyIs() => _conditionCurrencyIs.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionEntryIs() => _conditionEntryIs.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionInStockQuantity() => _conditionInStockQuantity.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionHasNoSalePrice() => _conditionHasNoSalePrice.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionAtNumItemsInCart() => _conditionAtNumItemsInCart.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionAtNumItemsInCategoryAreInCart() => _conditionAtNumItemsInCategoryAreInCart.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionAtNumItemsOfEntryAreInCart() => _conditionAtNumItemsOfEntryAreInCart.IsSatisfiedBy(_context);

    [Benchmark]
    public bool ConditionCartSubtotalLeast() => _conditionCartSubtotalLeast.IsSatisfiedBy(_context);

}

internal static class Program
{
    private static void Main()
    {
        //var result  = new ConditionsBenchmark().ConditionCartSubtotalLeast(); // Debug
        BenchmarkRunner.Run<ConditionsBenchmark>(); // Test
    }
}
