using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.AssetsModule.Core.Assets;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.MarketingModule.Web.BackgroundJobs;

/// <summary>
/// Imports promotion coupons from a CSV file and reports progress through the push notification returned to the
/// admin UI when the import was started.
/// </summary>
public class ImportCouponsJob(
    IBlobStorageProvider blobStorageProvider,
    CsvCouponImporter csvCouponImporter,
    IPushNotificationManager notifier)
    : IBackgroundJobHandler<ImportCouponsJobPayload>
{
    public virtual async Task Execute(ImportCouponsJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        var request = payload.Request;
        var notification = payload.Notification;

        await using var stream = await blobStorageProvider.OpenReadAsync(request.FileUrl);

        try
        {
            await csvCouponImporter.DoImportAsync(stream, request.Delimiter, request.PromotionId, request.ExpirationDate, ProgressCallback);
        }
        catch (Exception exception)
        {
            notification.Description = "Import error";
            notification.ErrorCount++;
            notification.Errors.Add(exception.ToString());
        }
        finally
        {
            notification.Finished = DateTime.UtcNow;
            notification.Description = "Import finished" + (notification.Errors.Any() ? " with errors" : " successfully");
            await notifier.SendAsync(notification);
        }

        return;

        void ProgressCallback(ExportImportProgressInfo c)
        {
            notification.Description = c.Description;
            notification.Errors = c.Errors;
            notification.ErrorCount = c.ErrorCount;

            notifier.Send(notification);
        }
    }
}
