using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Conditions.CatalogConditions
{
    public class DynamicContentConditionCategoryIs : ConditionTree
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            return context is DynamicContentEvaluationContext dynamicContentContext
                && !dynamicContentContext.CategoryId.IsNullOrEmpty()
                && CategoryId.EqualsInvariant(dynamicContentContext.CategoryId);
        }
    }
}
