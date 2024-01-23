using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    //Get [] % off payment method [] not to exceed $ [ 500 ]
    public class RewardPaymentGetOfRel : ConditionTree, IReward
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal MaxLimit { get; set; }

        #region IRewardExpression Members

        public PromotionReward[] GetRewards()
        {
            var retVal = new CartSubtotalReward
            {
                Amount = Amount,
                AmountType = RewardAmountType.Relative,
                MaxLimit = MaxLimit
            };
            return new PromotionReward[] { retVal };
        }

        #endregion
    }
}
