using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.ExperienceApiModule.Core.Infrastructure;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.Core.Services;

namespace VirtoCommerce.MarketingModule.ExperienceApi.Queries
{
    public class EvaluateDynamicContentQueryHandler : IQueryHandler<EvaluateDynamicContentQuery, EvaluateDynamicContentResult>
    {
        private readonly IMarketingDynamicContentEvaluator _marketingDynamicContentEvaluator;

        public EvaluateDynamicContentQueryHandler(IMarketingDynamicContentEvaluator marketingDynamicContentEvaluator)
        {
            _marketingDynamicContentEvaluator = marketingDynamicContentEvaluator;
        }

        public async Task<EvaluateDynamicContentResult> Handle(EvaluateDynamicContentQuery request, CancellationToken cancellationToken)
        {
            var context = new DynamicContentEvaluationContext()
            {
                StoreId = request.StoreId,
                PlaceName = request.PlaceName,
                CategoryId = request.CategoryId,
                ProductId = request.ProductId,
                Language = request.CultureName,
                ToDate = request.ToDate,
                Tags = request.Tags,
                UserGroups = request.UserGroups,
            };

            var items = await _marketingDynamicContentEvaluator.EvaluateItemsAsync(context);

            var result = new EvaluateDynamicContentResult() { Items = items, TotalCount = items.Length };
            return result;
        }
    }
}
