using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent.Search;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Converters;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class MarketingSearchServiceImpl : IDynamicContentSearchService, IPromotionSearchService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IMarketingExtensionManager _customPromotionManager;
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IExpressionSerializer _expressionSerializer;

        public MarketingSearchServiceImpl(Func<IMarketingRepository> repositoryFactory, IMarketingExtensionManager customPromotionManager, IDynamicContentService dynamicContentService, IExpressionSerializer expressionSerializer)
        {
            _repositoryFactory = repositoryFactory;
            _customPromotionManager = customPromotionManager;
            _dynamicContentService = dynamicContentService;
            _expressionSerializer = expressionSerializer;
        }

        #region IPromotionSearchService Members
        public GenericSearchResult<coreModel.Promotion> SearchPromotions(PromotionSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<coreModel.Promotion>();
            using (var repository = _repositoryFactory())
            {      
                var query = repository.Promotions;
                var query2 = _customPromotionManager.Promotions;
                if (!string.IsNullOrEmpty(criteria.Store))
                {
                    query = query.Where(x => x.StoreId == criteria.Store);
                    query2 = query2.Where(x => x.Store == criteria.Store);
                }
                if(criteria.OnlyActive)
                {
                    query = query.Where(x => x.IsActive == true);
                }
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Description.Contains(criteria.Keyword));
                    query2 = query2.Where(x => x.Name.Contains(criteria.Keyword) || x.Description.Contains(criteria.Keyword));
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<coreModel.Promotion>(x => x.ModifiedDate), SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();

                retVal.Results = query.Skip(criteria.Skip)
                                      .Take(criteria.Take)
                                      .ToArray()
                                      .Select(x => x.ToCoreModel(_expressionSerializer))
                                      .ToList();
                criteria.Skip = Math.Max(0, criteria.Skip - retVal.TotalCount);
                criteria.Take = Math.Max(0, criteria.Take - retVal.Results.Count());

                retVal.TotalCount += query2.Count();

                retVal.Results.AddRange(_customPromotionManager.Promotions.Skip(criteria.Skip).Take(criteria.Take));
      
            }
            return retVal;
        } 
        #endregion

        #region IDynamicContentSearchService Members
        public GenericSearchResult<coreModel.DynamicContentItem> SearchContentItems(DynamicContentItemSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<coreModel.DynamicContentItem>();
            using (var repository = _repositoryFactory())
            {             
                var query = repository.Items;
                if (!string.IsNullOrEmpty(criteria.FolderId))
                {
                    query = query.Where(x => x.FolderId == criteria.FolderId);
                }
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<coreModel.DynamicContentItem>(x => x.Name), SortDirection = SortDirection.Ascending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();

                var ids = query.Select(x => x.Id)
                               .Skip(criteria.Skip)
                               .Take(criteria.Take).ToArray();
                retVal.Results = ids.Select(x => _dynamicContentService.GetContentItemById(x)).ToList();
            }
            return retVal;
        }

        public GenericSearchResult<coreModel.DynamicContentPlace> SearchContentPlaces(DynamicContentPlaceSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<coreModel.DynamicContentPlace>();
            using (var repository = _repositoryFactory())
            {
                var query = repository.Places;
                if(!string.IsNullOrEmpty(criteria.FolderId))
                {
                    query = query.Where(x => x.FolderId == criteria.FolderId);
                }
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }
                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<coreModel.DynamicContentPlace>(x => x.Name), SortDirection = SortDirection.Ascending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();
                var ids = query.Select(x => x.Id)
                               .Skip(criteria.Skip)
                               .Take(criteria.Take).ToArray();
                retVal.Results = ids.Select(x => _dynamicContentService.GetPlaceById(x)).ToList();
            }
            return retVal;
        }

        public GenericSearchResult<coreModel.DynamicContentPublication> SearchContentPublications(DynamicContentPublicationSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<coreModel.DynamicContentPublication>();
            using (var repository = _repositoryFactory())
            {             
                var query = repository.PublishingGroups;
                if(string.IsNullOrEmpty(criteria.Store))
                {
                    query = query.Where(x => x.StoreId == criteria.Store);
                }

                if(criteria.OnlyActive)
                {
                    query = query.Where(x => x.IsActive == true);
                }
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }                
                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<coreModel.DynamicContentPublication>(x => x.Name), SortDirection = SortDirection.Ascending } };
                }

                retVal.TotalCount = query.Count();

                var ids = query.Select(x => x.Id)
                           .Skip(criteria.Skip)
                           .Take(criteria.Take).ToArray();
                retVal.Results = ids.Select(x => _dynamicContentService.GetPublicationById(x)).ToList();
            }
            return retVal;
        }

        public GenericSearchResult<coreModel.DynamicContentFolder> SearchFolders(DynamicContentFolderSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<coreModel.DynamicContentFolder>();
            using (var repository = _repositoryFactory())
            {        
                var query = repository.Folders.Where(x => x.ParentFolderId == criteria.FolderId);
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }
                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<coreModel.DynamicContentFolder>(x => x.Name), SortDirection = SortDirection.Ascending } };
                }

                retVal.TotalCount = query.Count();

                var folderIds = query.Select(x => x.Id).ToArray();
                retVal.Results = new List<coreModel.DynamicContentFolder>();                
                foreach (var folderId in folderIds)
                {
                    var folder = repository.GetContentFolderById(folderId);
                    retVal.Results.Add(folder.ToCoreModel());
                }
            }
            return retVal;
        } 
        #endregion        
     
    }
}