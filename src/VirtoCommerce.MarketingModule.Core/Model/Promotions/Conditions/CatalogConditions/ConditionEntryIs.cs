using System;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions
{
    //Product is []
    public class ConditionEntryIs : ConditionTree
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string[] ProductIds { get; set; }
        public string[] ProductNames { get; set; }
        public bool ApplyToAllVariants { get; set; }

        /// <summary>
        /// ((PromotionEvaluationContext)x).IsItemInProduct(ProductId)
        /// </summary>
        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            if (ProductId == null && ProductIds == null)
            {
                throw new ArgumentException($"{nameof(ProductId)} and {nameof(ProductIds)} cannot be null.");
            }

            var result = false;
            if (context is PromotionEvaluationContext promotionEvaluationContext)
            {
                result = ProductIds != null
                    ? promotionEvaluationContext.IsItemInProducts(ProductIds)
                    : promotionEvaluationContext.IsItemInProduct(ProductId);

                if (!result && ApplyToAllVariants)
                {
                    // check variations
                    result = ProductIds != null
                        ? promotionEvaluationContext.IsParentItemInProducts(ProductIds)
                        : promotionEvaluationContext.IsParentItemInProduct(ProductId);
                }
            }

            return result;
        }
    }
}
