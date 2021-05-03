using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingSampleModule.Web.Models
{
    public class SampleCondition : ConditionTree
    {
        public bool IsSatisfied { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            return IsSatisfied;
        }
    }
}
