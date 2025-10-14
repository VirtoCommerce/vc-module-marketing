using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class DefaultDynamicContentEvaluator(
    IDynamicContentItemService dynamicContentItemService,
    IDynamicContentPublicationSearchService dynamicContentPublicationSearchService,
    ILogger<DefaultDynamicContentEvaluator> logger)
    : IMarketingDynamicContentEvaluator
{
    public async Task<IList<DynamicContentItem>> EvaluateItemsAsync(IEvaluationContext context)
    {
        if (context is not DynamicContentEvaluationContext dynamicContext)
        {
            throw new ArgumentException("The context must be a DynamicContentEvaluationContext.");
        }

        if (dynamicContext.ToDate == default)
        {
            dynamicContext.ToDate = DateTime.UtcNow;
        }

        var items = new List<DynamicContentItem>();

        var criteria = AbstractTypeFactory<DynamicContentPublicationSearchCriteria>.TryCreateInstance();
        criteria = criteria.FromEvalContext(dynamicContext);

        var publications = await dynamicContentPublicationSearchService.SearchAsync(criteria);
        const int maxResponseItemsCount = 20;

        foreach (var publication in publications.Results)
        {
            try
            {
                // Filter assignments containing dynamic expression
                if (publication.DynamicExpression != null && publication.DynamicExpression.IsSatisfiedBy(context))
                {
                    items.AddRange(publication.ContentItems);
                }

                if (items.Count >= maxResponseItemsCount)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        return await dynamicContentItemService.GetAsync(items.Select(x => x.Id).ToArray());
    }
}
