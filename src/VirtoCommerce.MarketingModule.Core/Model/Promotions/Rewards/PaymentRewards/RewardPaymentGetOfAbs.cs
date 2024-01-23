using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    //Get $[] off payment method []
    public class RewardPaymentGetOfAbs : ConditionTree, IReward
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        #region IRewardExpression Members

        public PromotionReward[] GetRewards()
        {
            var retVal = new CartSubtotalReward
            {
                Amount = Amount,
                AmountType = RewardAmountType.Absolute
            };
            return new PromotionReward[] { retVal };
        }

        #endregion
    }
}
