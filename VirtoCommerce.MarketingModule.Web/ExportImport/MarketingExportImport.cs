using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent.Search;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.MarketingModule.Web.ExportImport
{

    public sealed class BackupObject
    {
        public BackupObject()
        {
            Promotions = new List<Promotion>();
            Coupons = new List<Coupon>();
            ContentPlaces = new List<DynamicContentPlace>();
            ContentItems = new List<DynamicContentItem>();
            ContentPublications = new List<DynamicContentPublication>();
            ContentFolders = new List<DynamicContentFolder>();
            Usages = new List<PromotionUsage>();
        }
        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Coupon> Coupons { get; set; }
        public ICollection<DynamicContentPlace> ContentPlaces { get; set; }
        public ICollection<DynamicContentItem> ContentItems { get; set; }
        public ICollection<DynamicContentPublication> ContentPublications { get; set; }
        public ICollection<DynamicContentFolder> ContentFolders { get; set; }
        public ICollection<PromotionUsage> Usages { get; set; }
    }

    public class MarketingExportImport
    {
        private readonly IPromotionSearchService _promotionSearchService;
        private readonly IDynamicContentSearchService _dynamicContentSearchService;
        private readonly IPromotionService _promotionService;
        private readonly ICouponService _couponService;
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IPromotionUsageService _usageService;

        public MarketingExportImport(IPromotionSearchService promotionSearchService, IPromotionService promotionService, IDynamicContentService dynamicContentService, ICouponService couponService, IDynamicContentSearchService dynamicContentSearchService, IPromotionUsageService marketingUsageService)
        {
            _promotionSearchService = promotionSearchService;
            _promotionService = promotionService;
            _dynamicContentService = dynamicContentService;
            _couponService = couponService;
            _dynamicContentSearchService = dynamicContentSearchService;
            _usageService = marketingUsageService;
        }

        public virtual void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
        }

        public virtual void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var progressInfo = new ExportImportProgressInfo();

            progressInfo.Description = $"{backupObject.Promotions.Count} promotions importing...";
            progressCallback(progressInfo);

            //legacy promotion compatability
            foreach (var promotion in backupObject.Promotions)
            {
                if (promotion.StoreIds.IsNullOrEmpty())
                {
                    promotion.StoreIds = new List<string>();
                }

                if (!promotion.Store.IsNullOrEmpty() && !promotion.StoreIds.Contains(promotion.Store))
                {
                    promotion.StoreIds.Add(promotion.Store);
                }
            }

            SavePromotions(backupObject.Promotions.ToArray());

            progressInfo.Description = $"{backupObject.ContentFolders.Count} folders importing...";
            progressCallback(progressInfo);
            SaveFolders(backupObject.ContentFolders.ToArray());

            progressInfo.Description = $"{backupObject.ContentPlaces.Count} places importing...";
            progressCallback(progressInfo);
            SavePlaces(backupObject.ContentPlaces.ToArray());

            progressInfo.Description = $"{backupObject.ContentItems.Count} contents importing...";
            progressCallback(progressInfo);
            SaveContentItems(backupObject.ContentItems.ToArray());

            progressInfo.Description = $"{backupObject.ContentPublications.Distinct().Count()} publications importing...";
            progressCallback(progressInfo);
            SavePublications(backupObject.ContentPublications.Distinct().ToArray());

            var pageSize = 500;
            var couponsTotal = backupObject.Coupons.Count;
            Paginate(couponsTotal, pageSize, (x) =>
            {
                progressInfo.Description = $"{Math.Min(x * pageSize, couponsTotal)} of {couponsTotal} coupons imported";
                progressCallback(progressInfo);
                SaveCoupons(backupObject.Coupons.Skip((x - 1) * pageSize).Take(pageSize).ToArray());
            });

            var usagesTotal = backupObject.Usages.Count;
            Paginate(usagesTotal, pageSize, (x) =>
            {
                progressInfo.Description = $"{Math.Min(x * pageSize, usagesTotal)} of {usagesTotal} usages imported";
                progressCallback(progressInfo);
                SaveUsages(backupObject.Usages.Skip((x - 1) * pageSize).Take(pageSize).ToArray());
            });
        }

        #region BackupObject

        protected virtual BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            var result = new BackupObject();

            var progressInfo = new ExportImportProgressInfo { Description = "Search promotions..." };
            progressCallback(progressInfo);
            var allPromotions = SearchPromotions(new PromotionSearchCriteria { Take = int.MaxValue }).Results;

            progressInfo.Description = $"{allPromotions.Count} promotions loading...";
            progressCallback(progressInfo);
            result.Promotions = LoadPromotions(allPromotions.Select(x => x.Id).ToArray());

            progressInfo.Description = "Search dynamic content objects...";
            progressCallback(progressInfo);

            progressInfo.Description = "Loading folders...";
            progressCallback(progressInfo);
            result.ContentFolders = LoadFoldersRecursive(null);

            progressInfo.Description = "Loading places...";
            progressCallback(progressInfo);
            result.ContentPlaces = LoadContentPlaces(new DynamicContentPlaceSearchCriteria { Take = int.MaxValue }).Results;

            progressInfo.Description = "Loading contents...";
            progressCallback(progressInfo);
            result.ContentItems = LoadContentItems(new DynamicContentItemSearchCriteria { Take = int.MaxValue }).Results;

            progressInfo.Description = "Loading publications...";
            progressCallback(progressInfo);
            result.ContentPublications = LoadContentPublications(new DynamicContentPublicationSearchCriteria { Take = int.MaxValue }).Results;

            progressInfo.Description = "Loading coupons...";
            progressCallback(progressInfo);
            var couponsTotal = LoadCoupons(new CouponSearchCriteria { Take = 0 }).TotalCount;
            var pageSize = 500;
            Paginate(couponsTotal, pageSize, (x) =>
            {
                progressInfo.Description = $"Loading coupons: {Math.Min(x * pageSize, couponsTotal)} of {couponsTotal} loaded";
                progressCallback(progressInfo);
                result.Coupons.AddRange(LoadCoupons(new CouponSearchCriteria { Skip = (x - 1) * pageSize, Take = pageSize }).Results);
            });

            progressInfo.Description = "Loading usages...";
            progressCallback(progressInfo);
            var usagesTotal = LoadPromotionUsages(new PromotionUsageSearchCriteria { Take = 0 }).TotalCount;
            Paginate(usagesTotal, pageSize, (x) =>
            {
                progressInfo.Description = $"Loading usages: {Math.Min(x * pageSize, usagesTotal)} of {usagesTotal} loaded";
                progressCallback(progressInfo);
                result.Usages.AddRange(LoadPromotionUsages(new PromotionUsageSearchCriteria { Skip = (x - 1) * pageSize, Take = pageSize }).Results);
            });

            return result;
        }

        protected List<DynamicContentFolder> LoadFoldersRecursive(DynamicContentFolder folder)
        {
            var retVal = new List<DynamicContentFolder>();
            var childrenFolders = LoadFolders(new DynamicContentFolderSearchCriteria { FolderId = folder?.Id, Take = int.MaxValue }).Results.ToList();
            foreach (var childFolder in childrenFolders)
            {
                retVal.Add(childFolder);
                retVal.AddRange(LoadFoldersRecursive(childFolder));
            }
            return retVal;
        }

        protected static void Paginate(int totalCount, int batchSize, Action<int> callback = null)
        {
            var pagesCount = totalCount > 0 ? (int)Math.Ceiling(totalCount / (double)batchSize) : 0;
            for (var i = 1; i <= pagesCount; i++)
            {
                callback(i);
            }
        }

        #endregion

        #region Load methods

        protected virtual GenericSearchResult<Promotion> SearchPromotions(PromotionSearchCriteria criteria)
        {
            return _promotionSearchService.SearchPromotions(criteria);
        }

        protected virtual Promotion[] LoadPromotions(string[] ids)
        {
            return _promotionService.GetPromotionsByIds(ids);
        }

        protected virtual GenericSearchResult<DynamicContentFolder> LoadFolders(DynamicContentFolderSearchCriteria criteria)
        {
            return _dynamicContentSearchService.SearchFolders(criteria);
        }

        protected virtual GenericSearchResult<DynamicContentPlace> LoadContentPlaces(DynamicContentPlaceSearchCriteria criteria)
        {
            return _dynamicContentSearchService.SearchContentPlaces(criteria);
        }

        protected virtual GenericSearchResult<DynamicContentItem> LoadContentItems(DynamicContentItemSearchCriteria criteria)
        {
            return _dynamicContentSearchService.SearchContentItems(criteria);
        }

        protected virtual GenericSearchResult<DynamicContentPublication> LoadContentPublications(DynamicContentPublicationSearchCriteria criteria)
        {
            return _dynamicContentSearchService.SearchContentPublications(criteria);
        }

        protected virtual GenericSearchResult<Coupon> LoadCoupons(CouponSearchCriteria criteria)
        {
            return _couponService.SearchCoupons(criteria);
        }

        protected virtual GenericSearchResult<PromotionUsage> LoadPromotionUsages(PromotionUsageSearchCriteria criteria)
        {
            return _usageService.SearchUsages(criteria);
        }

        #endregion Load methods

        #region Save methods

        protected virtual void SavePromotions(Promotion[] promotions)
        {
            _promotionService.SavePromotions(promotions);
        }

        protected virtual void SaveFolders(DynamicContentFolder[] folders)
        {
            _dynamicContentService.SaveFolders(folders);
        }

        protected virtual void SavePlaces(DynamicContentPlace[] dynamicContentPlaces)
        {
            _dynamicContentService.SavePlaces(dynamicContentPlaces);
        }

        protected virtual void SaveContentItems(DynamicContentItem[] dynamicContentItems)
        {
            _dynamicContentService.SaveContentItems(dynamicContentItems);
        }

        protected virtual void SavePublications(DynamicContentPublication[] dynamicContentPublications)
        {
            _dynamicContentService.SavePublications(dynamicContentPublications);
        }

        protected virtual void SaveCoupons(Coupon[] coupons)
        {
            _couponService.SaveCoupons(coupons);
        }

        protected virtual void SaveUsages(PromotionUsage[] promotionUsages)
        {
            _usageService.SaveUsages(promotionUsages);
        }

        #endregion Save methods
    }
}
