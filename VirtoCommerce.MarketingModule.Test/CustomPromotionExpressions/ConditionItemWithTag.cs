using System;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Test.CustomPromotionExpressions
{
    //items with [] tag
    public class ConditionItemWithTag : DynamicExpression, IConditionExpression
    {
        public string[] Tags { get; set; }

        /// <summary>
        /// ((PromotionEvaluationContext)x).CheckItemTags() > NumItem
        /// </summary>
        /// <returns></returns>
        linq.Expression<Func<IEvaluationContext, bool>> IConditionExpression.GetConditionExpression()
        {
            var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
            var castOp = linq.Expression.Convert(paramX, typeof(PromotionEvaluationContext));

            var tagsArray = linq.Expression.NewArrayInit(typeof(string), Tags.Select(linq.Expression.Constant));

            var methodInfo = typeof(CustomPromotionEvaluationContextExtension).GetMethod("CheckItemTags");
            var methodCall = linq.Expression.Call(null, methodInfo, castOp, tagsArray);
            var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);
            return retVal;
        }
    }
}
