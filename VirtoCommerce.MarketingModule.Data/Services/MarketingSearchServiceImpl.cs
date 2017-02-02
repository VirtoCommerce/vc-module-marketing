using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Converters;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class MarketingSearchServiceImpl : IMarketingSearchService
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

        #region IMarketingSearchService Members

        public coreModel.MarketingSearchResult SearchResources(coreModel.MarketingSearchCriteria criteria)
        {
            var retVal = new coreModel.MarketingSearchResult();

            if (string.IsNullOrEmpty(criteria.Sort))
            {
                criteria.Sort = "name:asc";
            }

            if (!string.IsNullOrEmpty(criteria.ResponseGroup))
            {
                var responseGroup = EnumUtility.SafeParse(criteria.ResponseGroup, coreModel.SearchResponseGroup.Full);

                if ((responseGroup & coreModel.SearchResponseGroup.WithPromotions) == coreModel.SearchResponseGroup.WithPromotions)
                {
                    SearchPromotions(criteria, retVal);
                    criteria.Take -= retVal.Promotions.Count;
                }
                if ((responseGroup & coreModel.SearchResponseGroup.WithContentItems) == coreModel.SearchResponseGroup.WithContentItems)
                {
                    SearchContentItems(criteria, retVal);
                    criteria.Take -= retVal.ContentItems.Count;
                }
                if ((responseGroup & coreModel.SearchResponseGroup.WithContentPlaces) == coreModel.SearchResponseGroup.WithContentPlaces)
                {
                    SearchContentPlaces(criteria, retVal);
                    criteria.Take -= retVal.ContentPlaces.Count;
                }
                if ((responseGroup & coreModel.SearchResponseGroup.WithContentPublications) == coreModel.SearchResponseGroup.WithContentPublications)
                {
                    SearchContentPublications(criteria, retVal);
                    criteria.Take -= retVal.ContentPublications.Count;
                }
                if ((responseGroup & coreModel.SearchResponseGroup.WithFolders) == coreModel.SearchResponseGroup.WithFolders)
                {
                    SearchFolders(criteria, retVal);
                    criteria.Take -= retVal.ContentFolders.Count;
                }
            }

            return retVal;
        }

        #endregion

        private void SearchFolders(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var sortingAliases = new Dictionary<string, string>();
                sortingAliases["contentFolders"] = ReflectionUtility.GetPropertyName<coreModel.DynamicContentFolder>(x => x.Name);

                var query = repository.Folders.Where(x => x.ParentFolderId == criteria.FolderId);
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }

                TryTransformSortingInfoColumnNames(sortingAliases, criteria.SortInfos);
                query = query.OrderBySortInfos(criteria.SortInfos);

                result.TotalCount += query.Count();

                var folderIds = query.Select(x => x.Id).ToArray();

                result.ContentFolders = new List<coreModel.DynamicContentFolder>();
                foreach (var folderId in folderIds)
                {
                    var folder = repository.GetContentFolderById(folderId);
                    result.ContentFolders.Add(folder.ToCoreModel());
                }

                // Populate folder for all found places and items
                if (criteria.FolderId != null)
                {
                    var searchedFolder = repository.GetContentFolderById(criteria.FolderId);
                    if (searchedFolder != null)
                    {
                        var hasfolderItems = result.ContentPlaces.OfType<coreModel.IsHasFolder>().Concat(result.ContentItems);
                        foreach (var hasfolderItem in hasfolderItems)
                        {
                            hasfolderItem.Folder = searchedFolder.ToCoreModel();
                        }
                    }
                }

            }
        }

        private void SearchContentItems(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var sortingAliases = new Dictionary<string, string>();
                sortingAliases["contentItems"] = ReflectionUtility.GetPropertyName<coreModel.DynamicContentItem>(x => x.Name);

                var query = repository.Items.Where(x => x.FolderId == criteria.FolderId);
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }

                TryTransformSortingInfoColumnNames(sortingAliases, criteria.SortInfos);
                query = query.OrderBySortInfos(criteria.SortInfos);

                result.TotalCount += query.Count();
                var ids = query.Select(x => x.Id)
                               .Skip(criteria.Skip)
                               .Take(criteria.Take).ToArray();
                result.ContentItems = ids.Select(x => _dynamicContentService.GetContentItemById(x)).ToList();
            }

        }

        private void SearchContentPlaces(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var sortingAliases = new Dictionary<string, string>();
                sortingAliases["contentPlaces"] = ReflectionUtility.GetPropertyName<coreModel.DynamicContentPlace>(x => x.Name);

                var query = repository.Places.Where(x => x.FolderId == criteria.FolderId);

                TryTransformSortingInfoColumnNames(sortingAliases, criteria.SortInfos);
                query = query.OrderBySortInfos(criteria.SortInfos);

                result.TotalCount += query.Count();
                var ids = query.Select(x => x.Id)
                               .Skip(criteria.Skip)
                               .Take(criteria.Take).ToArray();
                result.ContentPlaces = ids.Select(x => _dynamicContentService.GetPlaceById(x)).ToList();
            }

        }

        private void SearchContentPublications(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var sortingAliases = new Dictionary<string, string>();
                sortingAliases["contentPublications"] = ReflectionUtility.GetPropertyName<coreModel.DynamicContentPublication>(x => x.Name);

                var query = repository.PublishingGroups;
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(q => q.Name.Contains(criteria.Keyword));
                }

                TryTransformSortingInfoColumnNames(sortingAliases, criteria.SortInfos);
                query = query.OrderBySortInfos(criteria.SortInfos);

                result.TotalCount += query.Count();

                var ids = query.Select(x => x.Id)
                           .Skip(criteria.Skip)
                           .Take(criteria.Take).ToArray();
                result.ContentPublications = ids.Select(x => _dynamicContentService.GetPublicationById(x)).ToList();
            }

        }

        private void SearchPromotions(coreModel.MarketingSearchCriteria criteria, coreModel.MarketingSearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var sortingAliases = new Dictionary<string, string>();
                sortingAliases["promotions"] = ReflectionUtility.GetPropertyName<coreModel.Promotion>(x => x.Name);

                var query = repository.Promotions;
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Description.Contains(criteria.Keyword));
                }

                TryTransformSortingInfoColumnNames(sortingAliases, criteria.SortInfos);
                query = query.OrderBySortInfos(criteria.SortInfos);

                var promotions = query.Skip(criteria.Skip)
                                      .Take(criteria.Take)
                                      .ToArray()
                                      .Select(x => x.ToCoreModel(_expressionSerializer))
                                      .ToList();
                var totalCount = query.Count();

                promotions.AddRange(_customPromotionManager.Promotions.Skip(criteria.Skip).Take(criteria.Take));
                totalCount += _customPromotionManager.Promotions.Count();

                result.Promotions = promotions;
                result.TotalCount += totalCount;
            }
        }

        private static void TryTransformSortingInfoColumnNames(IDictionary<string, string> transformationMap, SortInfo[] sortingInfos)
        {
            foreach (var sortInfo in sortingInfos)
            {
                string newColumnName;
                if (transformationMap.TryGetValue(sortInfo.SortColumn.ToLowerInvariant(), out newColumnName))
                {
                    sortInfo.SortColumn = newColumnName;
                }
            }
        }
    }
}