using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Common;

namespace IsSatisfiedByBench
{
    internal class BlockConditionAndOrPlain
    {

        public string Id { get; protected set; }

        public virtual IList<BlockConditionAndOrPlain> AvailableChildren { get; set; } = new List<BlockConditionAndOrPlain>();
        public virtual IList<BlockConditionAndOrPlain> Children { get; set; } = new List<BlockConditionAndOrPlain>();


        public bool All { get; set; }

        // Logical inverse of expression
        public bool Not { get; set; } = false;


        public bool IsSatisfiedBy(IEvaluationContext context)
        {
            var result = false;

            if (Children != null && Children.Count > 0)
            {
                if (!Not)
                {
                    result = All ? AllSatisfied(Children, context) : AnySatisfied(Children, context);
                }
                else
                {
                    result = All ? !AllSatisfied(Children, context) : !AnySatisfied(Children, context);
                }

            }
            else
            {
                result = true;
            }

            return result;
        }

        private bool AnySatisfied(IList<BlockConditionAndOrPlain> children, IEvaluationContext context)
        {
            foreach (var ch in children)
            {
                if (ch.IsSatisfiedBy(context)) return true;
            }
            return false;
        }

        private bool AllSatisfied(IList<BlockConditionAndOrPlain> children, IEvaluationContext context)
        {
            foreach (var ch in children)
            {
                if (!ch.IsSatisfiedBy(context)) return false;
            }
            return true;
        }
    }
}
