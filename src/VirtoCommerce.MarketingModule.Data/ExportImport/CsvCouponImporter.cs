using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.MarketingModule.Web.ExportImport;

public sealed class CsvCouponImporter(ICouponService couponService)
{
    private const int _chunkSize = 500;

    public async Task DoImportAsync(Stream inputStream, string delimiter, string promotionId, DateTime? expirationDate, Action<ExportImportProgressInfo> progressCallback)
    {
        var coupons = new List<Coupon>();

        var progressInfo = new ExportImportProgressInfo
        {
            Description = "Reading coupons from CSV...",
        };
        progressCallback(progressInfo);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter,
            HasHeaderRecord = false,
        };

        using (var reader = new CsvReader(new StreamReader(inputStream), config))
        {
            while (await reader.ReadAsync())
            {
                var coupon = AbstractTypeFactory<Coupon>.TryCreateInstance();
                coupon.Code = reader.GetField<string>(0);
                coupon.MaxUsesNumber = reader.GetField<int>(1);
                coupon.MemberId = reader.GetField<string>(2);
                coupon.PromotionId = promotionId;
                coupon.ExpirationDate = expirationDate;

                coupons.Add(coupon);
            }
        }

        var importedCount = 0;

        foreach (var chunk in coupons.Paginate(_chunkSize))
        {
            importedCount += chunk.Count;
            progressInfo.Description = $"Importing {importedCount} of {coupons.Count} coupons...";
            progressCallback(progressInfo);
            await couponService.SaveChangesAsync(chunk);
        }

        progressInfo.Description = "Coupons import is finished.";
        progressCallback(progressInfo);
    }
}
