using System;

namespace VirtoCommerce.MarketingModule.Core.Model
{
    public class DynamicContentPublicationSearchCriteria : DynamicContentSearchCriteriaBase
    {
        public bool OnlyActive { get; set; }
        public string Store { get; set; }
        public string PlaceName { get; set; }
        public DateTime? ToDate { get; set; }

        public virtual DynamicContentPublicationSearchCriteria FromEvalContext(DynamicContentEvaluationContext context)
        {
            Store = context.StoreId;
            PlaceName = context.PlaceName;
            ToDate = context.ToDate;
            OnlyActive = true;
            return this;
        }
    }
}
