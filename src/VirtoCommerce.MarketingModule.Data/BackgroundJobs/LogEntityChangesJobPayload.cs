using VirtoCommerce.Platform.Core.ChangeLog;

namespace VirtoCommerce.MarketingModule.Data.BackgroundJobs;

/// <summary>
/// Payload of <see cref="LogEntityChangesJobHandler"/>: the promotion and coupon change-log entries to persist.
/// </summary>
public class LogEntityChangesJobPayload
{
    public OperationLog[] OperationLogs { get; set; }
}
