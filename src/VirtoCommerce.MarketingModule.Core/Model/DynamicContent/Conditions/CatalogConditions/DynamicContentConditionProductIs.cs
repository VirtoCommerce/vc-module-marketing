using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Conditions.CatalogConditions
{
    public class DynamicContentConditionProductIs : ConditionTree
    {
        public string[] ProductIds { get; set; }
        public string[] ProductNames { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            return context is DynamicContentEvaluationContext dynamicContentContext
                && !dynamicContentContext.ProductId.IsNullOrEmpty()
                && (ProductIds?.Contains(dynamicContentContext.ProductId) ?? false);
        }
    }
}
