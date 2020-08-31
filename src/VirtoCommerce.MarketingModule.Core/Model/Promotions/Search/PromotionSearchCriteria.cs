using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions.Search
{
    public class PromotionSearchCriteria : SearchCriteriaBase
    {
        public bool OnlyActive { get; set; }
        public string Store { get; set; }

        private string[] _storeIds;
        public string[] StoreIds
        {
            get
            {
                if (_storeIds == null && !string.IsNullOrEmpty(Store))
                {
                    _storeIds = new[] { Store };
                }
                return _storeIds;
            }
            set
            {
                _storeIds = value;
            }
        }

        public virtual void PopulateFromEvalContext(PromotionEvaluationContext context)
        {
            //the Extension point for future extensions
            //Nothing todo here.
        }
    }
}
