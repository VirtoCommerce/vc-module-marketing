using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.PushNotifications;

namespace VirtoCommerce.MarketingModule.Web.BackgroundJobs;

/// <summary>
/// Payload of <see cref="ImportCouponsJob"/>: the import request and the push notification to report progress through.
/// </summary>
public class ImportCouponsJobPayload
{
    public ImportRequest Request { get; set; }

    public ImportNotification Notification { get; set; }
}
