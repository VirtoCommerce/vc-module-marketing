using System;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Test.CustomPromotionExpressions
{
    public static class CustomPromotionEvaluationContextExtension
    {
        public static bool CheckItemTags(this PromotionEvaluationContext context, string[] tags)
        {
            var retVal = tags.Any(x => context.PromoEntry.Attributes.ContainsKey("tag") && string.Equals(context.PromoEntry.Attributes["tag"], x, StringComparison.InvariantCultureIgnoreCase));
            return retVal;
        }
    }
}
