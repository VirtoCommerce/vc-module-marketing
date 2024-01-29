using System;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions
{
    // shipment is
    public class ShippingIsCondition : ConditionTree
    {
        public string ShippingMethod { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            if (ShippingMethod == null)
            {
                throw new ArgumentException($"{nameof(ShippingMethod)} cannot be null.");
            }

            var result = context is PromotionEvaluationContext promotionEvaluationContext
                && promotionEvaluationContext.ShipmentMethodCode == ShippingMethod;

            return result;
        }
    }
}
