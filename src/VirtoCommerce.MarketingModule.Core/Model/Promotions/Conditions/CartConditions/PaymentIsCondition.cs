using System;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions
{
    // payment is
    public class PaymentIsCondition : ConditionTree
    {
        public string PaymentMethod { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            if (PaymentMethod == null)
            {
                throw new ArgumentException($"{nameof(PaymentMethod)} cannot be null.");
            }

            var result = context is PromotionEvaluationContext promotionEvaluationContext
                && promotionEvaluationContext.PaymentMethodCode == PaymentMethod;

            return result;
        }
    }
}
