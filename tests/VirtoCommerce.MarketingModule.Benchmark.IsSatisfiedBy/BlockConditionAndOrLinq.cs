using System.Linq;
using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.Platform.Core.Common;

namespace IsSatisfiedByBench
{
    internal class BlockConditionAndOrLinq
    {

        public string Id { get; protected set; }

        public virtual IList<BlockConditionAndOrLinq> AvailableChildren { get; set; } = new List<BlockConditionAndOrLinq>();
        public virtual IList<BlockConditionAndOrLinq> Children { get; set; } = new List<BlockConditionAndOrLinq>();


        public bool All { get; set; }

        // Logical inverse of expression
        public bool Not { get; set; } = false;


        public bool IsSatisfiedBy(IEvaluationContext context)
        {
            var result = false;

            if (Children.IsNullOrEmpty())
            {
                return true;
            }

            if (Children != null && Children.Any())
            {
                if (!Not)
                {
                    result = All ? Children.All(ch => ch.IsSatisfiedBy(context)) : Children.Any(ch => ch.IsSatisfiedBy(context));
                }
                else
                {
                    result = All ? !Children.All(ch => ch.IsSatisfiedBy(context)) : !Children.Any(ch => ch.IsSatisfiedBy(context));
                }

            }

            return result;
        }
    }
}
