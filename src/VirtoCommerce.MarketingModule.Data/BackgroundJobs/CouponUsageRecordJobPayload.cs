using VirtoCommerce.MarketingModule.Data.Handlers;

namespace VirtoCommerce.MarketingModule.Data.BackgroundJobs;

/// <summary>
/// Payload of <see cref="CouponUsageRecordJob"/>: the coupon usages of newly placed orders.
/// </summary>
public class CouponUsageRecordJobPayload
{
    public CouponUsageRecordJobArgument[] JobArguments { get; set; } = [];
}
