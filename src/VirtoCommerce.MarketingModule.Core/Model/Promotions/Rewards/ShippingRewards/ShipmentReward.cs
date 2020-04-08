namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    /// <summary>
    /// Shipment reward
    /// </summary>
    public class ShipmentReward : AmountBasedReward
    {
        public ShipmentReward() : base(nameof(ShipmentReward))
        { }

        public string ShippingMethod { get; set; }
        public string ShippingMethodOption { get; set; }
    }
}
