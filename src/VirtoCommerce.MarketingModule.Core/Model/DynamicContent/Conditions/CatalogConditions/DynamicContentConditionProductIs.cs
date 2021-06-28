using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Conditions.CatalogConditions
{
    public class DynamicContentConditionProductIs : ConditionTree
    {
        public string[] ProductIds { get; set; }
        public string[] ProductNames { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            return context is DynamicContentEvaluationContext dynamicContentContext
                && (ProductIds?.Contains(dynamicContentContext.ProductId) ?? false);
        }
    }
}
