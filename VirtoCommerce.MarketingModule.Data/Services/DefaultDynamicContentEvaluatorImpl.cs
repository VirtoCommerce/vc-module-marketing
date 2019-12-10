using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Logging;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DefaultDynamicContentEvaluatorImpl : IMarketingDynamicContentEvaluator
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IExpressionSerializer _expressionSerializer;
        private readonly ILog _logger;
        private readonly IDynamicPropertyService _dynamicPropertyService;

        public DefaultDynamicContentEvaluatorImpl(
            Func<IMarketingRepository> repositoryFactory,
            IDynamicContentService dynamicContentService,
            IExpressionSerializer expressionSerializer,
            ILog logger,
            IDynamicPropertyService dynamicPropertyService
            )
        {
            _repositoryFactory = repositoryFactory;
            _dynamicContentService = dynamicContentService;
            _expressionSerializer = expressionSerializer;
            _logger = logger;
            _dynamicPropertyService = dynamicPropertyService;
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

            var contentItems = new List<DynamicContentItem>();
            using (var repository = _repositoryFactory())
            {
                var publishings = repository.PublishingGroups
                                                        .Include(x => x.ContentItems)
                                                        .Include(x => x.ContentItems.Select(c => c.ContentItem))
                                                        .Where(x => x.IsActive)
                                                        .Where(x => x.StoreId == dynamicContext.StoreId)
                                                        .Where(x => (x.StartDate == null || dynamicContext.ToDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= dynamicContext.ToDate))
                                                        .Where(x => x.ContentPlaces.Any(y => y.ContentPlace.Name == dynamicContext.PlaceName))
                                                        .OrderBy(x => x.Priority)
                                                        .ToArray();

                //Get content items for publishings without ConditionExpression
                contentItems = publishings.Where(x => x.ConditionExpression == null)
                                              .SelectMany(x =>x.ContentItems.Select(c => c.ToModel(AbstractTypeFactory<DynamicContentItem>.TryCreateInstance())))
                                              .ToList();

                foreach (var publishing in publishings.Where(x => x.ConditionExpression != null))
                {
                    try
                    {
                        //Next step need filter assignments contains dynamicexpression
                        var condition = _expressionSerializer.DeserializeExpression<Func<IEvaluationContext, bool>>(publishing.ConditionExpression);
                        if (condition(context))
                        {
                            contentItems.AddRange(publishing.ContentItems.Select(c =>c.ToModel(AbstractTypeFactory<DynamicContentItem>.TryCreateInstance())));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                }

                _dynamicPropertyService.LoadDynamicPropertyValues(contentItems.ToArray<IHasDynamicProperties>());
            }

            return contentItems.ToArray();
        }

        #endregion
    }
}
