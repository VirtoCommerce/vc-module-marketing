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

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DefaultDynamicContentEvaluator : IMarketingDynamicContentEvaluator
    {
        private readonly IContentPublicationsSearchService _contentPublicationsSearchService;
        private readonly IDynamicContentService _dynamicContentService;
        private readonly ILogger _logger;

        public DefaultDynamicContentEvaluator(IContentPublicationsSearchService contentPublicationsSearchService, IDynamicContentService dynamicContentService, ILogger<DefaultDynamicContentEvaluator> logger)
        {
            _contentPublicationsSearchService = contentPublicationsSearchService;
            _dynamicContentService = dynamicContentService;
            _logger = logger;
        }

        #region IMarketingDynamicContentEvaluator Members

        public async Task<DynamicContentItem[]> EvaluateItemsAsync(IEvaluationContext context)
        {
            if (context is not DynamicContentEvaluationContext dynamicContext)
            {
                throw new ArgumentException("The context must be a DynamicContentEvaluationContext.");
            }

            if (dynamicContext.ToDate == default)
            {
                dynamicContext.ToDate = DateTime.UtcNow;
            }

            var result = new List<DynamicContentItem>();
            var criteria = AbstractTypeFactory<DynamicContentPublicationSearchCriteria>.TryCreateInstance();
            criteria = criteria.FromEvalContext(dynamicContext);

            var publications = await _contentPublicationsSearchService.SearchContentPublicationsAsync(criteria);

            foreach (var publication in publications.Results)
            {
                try
                {
                    // Filter assignments containing dynamic expression
                    if (publication.DynamicExpression != null && publication.DynamicExpression.IsSatisfiedBy(context))
                    {
                        result.AddRange(publication.ContentItems);
                    }

                    if (result.Count >= 20)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            }

            return await _dynamicContentService.GetContentItemsByIdsAsync(result.Select(x => x.Id).ToArray());
        }

        #endregion
    }
}
