using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions;

namespace PromoConditionsBench
{
    public class ConditionsBenchmark
    {
        private readonly PromotionEvaluationContext pCtx;

        private readonly ConditionTree _conditionIsRegisteredUser;
        private readonly ConditionTree _conditionIsEveryone;
        private readonly ConditionTree _conditionIsFirstTimeBuyer;
        private readonly ConditionTree _userGroupsContainsCondition;

        private readonly ConditionTree _conditionCategoryIs;
        private readonly ConditionTree _conditionCodeContains;
        private readonly ConditionTree _conditionCurrencyIs;
        private readonly ConditionTree _conditionEntryIs;
        private readonly ConditionTree _conditionInStockQuantity;
        private readonly ConditionTree _conditionHasNoSalePrice;

        private readonly ConditionTree _conditionAtNumItemsInCart;
        private readonly ConditionTree _conditionAtNumItemsInCategoryAreInCart;
        private readonly ConditionTree _conditionAtNumItemsOfEntryAreInCart;
        private readonly ConditionTree _conditionCartSubtotalLeast;

        public ConditionsBenchmark()
        {
            pCtx = MockPromotionEvaluationContext();

            // Customer conditions
            _conditionIsRegisteredUser = new ConditionIsRegisteredUser();
            _conditionIsEveryone = new ConditionIsEveryone();
            _conditionIsFirstTimeBuyer = new ConditionIsFirstTimeBuyer();
            _userGroupsContainsCondition = new UserGroupsContainsCondition() { Group = "Group7" };

            // Catalog conditions
            _conditionCategoryIs = new ConditionCategoryIs() { CategoryId = "8B77CD0F-5C4E-4BBA-9AEF-CD3022D0D2C1" };
            _conditionCodeContains = new ConditionCodeContains() { Keyword = "16-29" };
            _conditionCurrencyIs = new ConditionCurrencyIs() { Currency = "USD" };
            _conditionEntryIs = new ConditionEntryIs() { ProductIds = new string[] { "4B70F12A-25F8-4225-9A50-68C7E6DA25B3", "4B70F12A-25F8-4225-9A50-68C7E5DA25B2", "4B70F12B-25F8-4225-9A50-68C7E6DA25B2", "4B70F12A-2EF8-4225-9A50-68C7E6DA25B2", "4B70F12A-25F8-4225-9A50-68C7E6DA25B2" } };
            _conditionInStockQuantity = new ConditionInStockQuantity() { CompareCondition = ConditionOperation.Between, Quantity = 1, QuantitySecond = 100 };
            _conditionHasNoSalePrice = new ConditionHasNoSalePrice();

            // Cart conditions
            _conditionAtNumItemsInCart = new ConditionAtNumItemsInCart() { CompareCondition = ConditionOperation.Between, NumItem = 1, NumItemSecond = 100 };
            _conditionAtNumItemsInCategoryAreInCart = new ConditionAtNumItemsInCategoryAreInCart() { CompareCondition = ConditionOperation.Between, NumItem = 1, NumItemSecond = 100, CategoryId = "8B77CD0F-5C4E-4BBA-9AEF-CD3022D0D2C1" };
            _conditionAtNumItemsOfEntryAreInCart = new ConditionAtNumItemsOfEntryAreInCart() { CompareCondition = ConditionOperation.Between, NumItem = 1, NumItemSecond = 100, ProductId = "8B77CD0F-5C4E-4BBA-9AEF-CD3022D0D2C1" };
            _conditionCartSubtotalLeast = new ConditionCartSubtotalLeast() { CompareCondition = ConditionOperation.Between, SubTotal = 1, SubTotalSecond = 100 };

        }

        private PromotionEvaluationContext MockPromotionEvaluationContext()
        {
            return JsonConvert.DeserializeObject<PromotionEvaluationContext>(File.OpenText("pctx_mock.json").ReadToEnd());
        }


        [Benchmark]
        public bool ConditionIsRegisteredUser() => _conditionIsRegisteredUser.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionIsEveryone() => _conditionIsEveryone.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionIsFirstTimeBuyer() => _conditionIsFirstTimeBuyer.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool UserGroupsContainsCondition() => _userGroupsContainsCondition.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionCategoryIs() => _conditionCategoryIs.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionCodeContains() => _conditionCodeContains.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionCurrencyIs() => _conditionCurrencyIs.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionEntryIs() => _conditionEntryIs.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionInStockQuantity() => _conditionInStockQuantity.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionHasNoSalePrice() => _conditionHasNoSalePrice.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionAtNumItemsInCart() => _conditionAtNumItemsInCart.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionAtNumItemsInCategoryAreInCart() => _conditionAtNumItemsInCategoryAreInCart.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionAtNumItemsOfEntryAreInCart() => _conditionAtNumItemsOfEntryAreInCart.IsSatisfiedBy(pCtx);

        [Benchmark]
        public bool ConditionCartSubtotalLeast() => _conditionCartSubtotalLeast.IsSatisfiedBy(pCtx);

    }

    internal static class Program
    {
        static void Main()
        {
            //var result  = new ConditionsBenchmark().ConditionCartSubtotalLeast(); // Debug
            BenchmarkRunner.Run<ConditionsBenchmark>(); // Test
        }
    }
}
