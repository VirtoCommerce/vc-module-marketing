using System;
using System.Data.Entity;
using System.Linq;
using Common.Logging;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DefaultDynamicContentEvaluatorImpl : IMarketingDynamicContentEvaluator
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IExpressionSerializer _expressionSerializer;
        private readonly ILog _logger;

        public DefaultDynamicContentEvaluatorImpl(
            Func<IMarketingRepository> repositoryFactory,
            IDynamicContentService dynamicContentService,
            IExpressionSerializer expressionSerializer,
            ILog logger
            )
        {
            _repositoryFactory = repositoryFactory;
            _dynamicContentService = dynamicContentService;
            _expressionSerializer = expressionSerializer;
            _logger = logger;
        }

        #region IMarketingDynamicContentEvaluator Members

        public DynamicContentItem[] EvaluateItems(IEvaluationContext context)
        {
            var dynamicContext = context as DynamicContentEvaluationContext;

            if (dynamicContext == null)
            {
                throw new ArgumentException("The context must be a DynamicContentEvaluationContext.");
            }

            if(dynamicContext.ToDate == default(DateTime))
            {
                dynamicContext.ToDate = DateTime.UtcNow;
            }

            using (var repository = _repositoryFactory())
            {
                var publishingGroups = repository.PublishingGroups.Include(x => x.ContentItems)
                                                            .Where(x => x.IsActive)
                                                            .Where(x => x.StoreId == dynamicContext.StoreId)
                                                            .Where(x => (x.StartDate == null || dynamicContext.ToDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= dynamicContext.ToDate))
                                                            .Where(x => x.ContentPlaces.Any(y => y.ContentPlace.Name == dynamicContext.PlaceName))
                                                            .ToArray();

                var publications = _dynamicContentService.GetPublicationsByIds(publishingGroups.Select(p => p.Id).ToArray());

                var contentItems = publications.Where(x => x.PredicateSerialized == null)
                    .SelectMany(x => x.ContentItems)
                    .ToList();

                foreach (var dynamicContentPublication in publications.Where(x => x.PredicateSerialized != null))
                {
                    try
                    {
                        var condition = _expressionSerializer.DeserializeExpression<Func<IEvaluationContext, bool>>(dynamicContentPublication.PredicateSerialized);

                        if (condition(context))
                        {
                            contentItems.AddRange(dynamicContentPublication.ContentItems);
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                }

                return contentItems.ToArray();
            }
        }

        #endregion
    }
}
