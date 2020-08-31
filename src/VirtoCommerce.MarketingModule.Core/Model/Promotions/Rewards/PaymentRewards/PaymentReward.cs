namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    /// <summary>
    /// Payment reward
    /// </summary>
    public class PaymentReward : AmountBasedReward
    {
        public PaymentReward() : base(nameof(PaymentReward))
        { }

        public string PaymentMethod { get; set; }
    }
}
