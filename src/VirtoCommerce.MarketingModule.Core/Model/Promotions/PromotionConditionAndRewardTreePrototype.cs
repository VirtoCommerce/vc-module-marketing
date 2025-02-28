using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    /// <summary>
    /// Represents the prototype for promotion tree <see cref="PromotionConditionAndRewardTree"/> contains the list of available conditions for building a tree in designer
    /// </summary>
    public class PromotionConditionAndRewardTreePrototype : ConditionTree
    {
        public PromotionConditionAndRewardTreePrototype()
        {
            IConditionTree[] children =
            [
                new BlockCustomerCondition()
                    .WithAvailableChildren(
                        new ConditionIsRegisteredUser(),
                        new ConditionIsEveryone(),
                        new ConditionIsFirstTimeBuyer(),
                        new UserGroupsContainsCondition(),
                        new UserGroupIsCondition()
                    ),
                new BlockCatalogCondition()
                    .WithAvailableChildren(
                        new ConditionCategoryIs(),
                        new ConditionCodeContains(),
                        new ConditionCurrencyIs(),
                        new ConditionEntryIs(),
                        new ConditionInStockQuantity(),
                        new ConditionHasNoSalePrice()
                    ),
                new BlockCartCondition()
                    .WithAvailableChildren(
                        new ConditionAtNumItemsInCart(),
                        new ConditionAtNumItemsInCategoryAreInCart(),
                        new ConditionAtNumItemsOfEntryAreInCart(),
                        new PaymentIsCondition(),
                        new ShippingIsCondition(),
                        new ConditionCartSubtotalLeast()
                    ),
                new BlockReward()
                    .WithAvailableChildren(
                        new RewardCartGetOfAbsSubtotal(),
                        new RewardCartGetOfRelSubtotal(),
                        new RewardItemGetFreeNumItemOfProduct(),
                        new RewardItemGetOfAbs(),
                        new RewardItemGetOfAbsForNum(),
                        new RewardItemGetOfRel(),
                        new RewardItemGetOfRelForNum(),
                        new RewardItemGiftNumItem(),
                        new RewardShippingGetOfAbsShippingMethod(),
                        new RewardShippingGetOfRelShippingMethod(),
                        new RewardPaymentGetOfAbs(),
                        new RewardPaymentGetOfRel(),
                        new RewardItemForEveryNumInGetOfRel(),
                        new RewardItemForEveryNumOtherItemInGetOfRel()
                    ),
            ];

            WithChildren(children);
            WithAvailableChildren(children);
        }
    }
}
