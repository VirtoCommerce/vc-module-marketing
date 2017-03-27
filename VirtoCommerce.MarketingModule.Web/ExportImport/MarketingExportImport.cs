using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public sealed class MarketingExportImport
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

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback);
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject(progressCallback);

            var progressInfo = new ExportImportProgressInfo();

            progressInfo.Description = String.Format("{0} promotions importing...", backupObject.Promotions.Count());
            progressCallback(progressInfo);
            _promotionService.SavePromotions(backupObject.Promotions.ToArray());

            progressInfo.Description = String.Format("{0} folders importing...", backupObject.ContentFolders.Count());
            progressCallback(progressInfo);
            _dynamicContentService.SaveFolders(backupObject.ContentFolders.ToArray());

            progressInfo.Description = String.Format("{0} places importing...", backupObject.ContentPlaces.Count());
            progressCallback(progressInfo);
            _dynamicContentService.SavePlaces(backupObject.ContentPlaces.ToArray());

            progressInfo.Description = String.Format("{0} contents importing...", backupObject.ContentItems.Count());
            progressCallback(progressInfo);
            _dynamicContentService.SaveContentItems(backupObject.ContentItems.ToArray());

            progressInfo.Description = String.Format("{0} publications importing...", backupObject.ContentPublications.Distinct().Count());
            progressCallback(progressInfo);
            _dynamicContentService.SavePublications(backupObject.ContentPublications.Distinct().ToArray());

            progressInfo.Description = String.Format("{0} coupons importing...", backupObject.Coupons.Count());
            progressCallback(progressInfo);
            _couponService.SaveCoupons(backupObject.Coupons.ToArray());

            progressInfo.Description = String.Format("{0} marketing usages importing...", backupObject.Usages.Count());
            progressCallback(progressInfo);
            _usageService.SaveUsages(backupObject.Usages.ToArray());

        }
        #region BackupObject

        private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
        {
            var result = new BackupObject();
            var progressInfo = new ExportImportProgressInfo { Description = "Search promotions..." };
            progressCallback(progressInfo);
            var allPromotions = _promotionSearchService.SearchPromotions(new Domain.Marketing.Model.Promotions.Search.PromotionSearchCriteria
            {
                Take = int.MaxValue
            }).Results;

            progressInfo.Description = String.Format("{0} promotions loading...", allPromotions.Count());
            progressCallback(progressInfo);
            result.Promotions = _promotionService.GetPromotionsByIds(allPromotions.Select(x=>x.Id).ToArray());

            progressInfo.Description = "Search dynamic content objects...";
            progressCallback(progressInfo);

            progressInfo.Description = String.Format("Loading folders...");
            progressCallback(progressInfo);
            result.ContentFolders = _dynamicContentSearchService.SearchFolders(new DynamicContentFolderSearchCriteria { Take = int.MaxValue }).Results.ToList();

            progressInfo.Description = String.Format("Loading places...");
            progressCallback(progressInfo);
            result.ContentPlaces = _dynamicContentSearchService.SearchContentPlaces(new DynamicContentPlaceSearchCriteria { Take = int.MaxValue }).Results.ToList();

            progressInfo.Description = String.Format("Loading contents...");
            progressCallback(progressInfo);
            result.ContentItems = _dynamicContentSearchService.SearchContentItems(new DynamicContentItemSearchCriteria { Take = int.MaxValue }).Results.ToList();

            progressInfo.Description = String.Format("Loading publications...");
            progressCallback(progressInfo);
            result.ContentPublications = _dynamicContentSearchService.SearchContentPublications(new DynamicContentPublicationSearchCriteria { Take = int.MaxValue }).Results.ToList();

            progressInfo.Description = String.Format("Loading coupons...");
            progressCallback(progressInfo);
            result.Coupons =  _couponService.SearchCoupons(new CouponSearchCriteria { Take = int.MaxValue }).Results;

            progressInfo.Description = String.Format("Loading usages...");
            progressCallback(progressInfo);
            result.Usages = _usageService.SearchUsages(new PromotionUsageSearchCriteria { Take = int.MaxValue }).Results;

            return result;
        }
        #endregion     

    }
}