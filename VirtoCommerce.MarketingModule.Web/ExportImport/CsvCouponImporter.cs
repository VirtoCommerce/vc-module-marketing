using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.MarketingModule.Web.ExportImport
{
    public sealed class CsvCouponImporter
    {
        public CsvCouponImporter(
            Func<IMarketingRepository> marketingRepositoryFactory,
            IPromotionService promotionService)
        {
            _marketingRepositoryFactory = marketingRepositoryFactory;
            _promotionService = promotionService;
        }

        private readonly Func<IMarketingRepository> _marketingRepositoryFactory;
        private readonly IPromotionService _promotionService;

        public void DoImport(Stream inputStream, string delimiter, string promotionId, Action<ExportImportProgressInfo> progressCallback)
        {
            var coupons = new List<Coupon>();

            var progressInfo = new ExportImportProgressInfo
            {
                Description = "Reading coupons from CSV..."
            };
            progressCallback(progressInfo);

            using (var reader = new CsvReader(new StreamReader(inputStream)))
            {
                reader.Configuration.Delimiter = delimiter;
                reader.Configuration.HasHeaderRecord = false;
                while (reader.Read())
                {
                    coupons.Add(new Coupon
                    {
                        Code = reader.GetField<string>(0),
                        MaxUsesNumber = reader.GetField<int>(1),
                        PromotionId = promotionId
                    });
                }
            }

            using (var repository = _marketingRepositoryFactory())
            {
                var uniqueCoupons = new List<Coupon>();
                foreach (var coupon in coupons)
                {
                    progressInfo.Description = string.Format("Creating coupons: {0} created", ++progressInfo.ProcessedCount);
                    var existingCoupon = repository.Coupons.FirstOrDefault(c => c.Code == coupon.Code);
                    if (existingCoupon == null)
                    {
                        uniqueCoupons.Add(coupon);
                    }
                    else
                    {
                        progressInfo.Errors.Add(string.Format("Coupon with code \"{0}\" is already exists", coupon.Code));
                        progressCallback(progressInfo);
                    }
                }

                _promotionService.SaveCoupons(uniqueCoupons.ToArray());
            }
        }
    }
}