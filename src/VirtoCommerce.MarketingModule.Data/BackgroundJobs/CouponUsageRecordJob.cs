using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Data.Handlers;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.MarketingModule.Data.BackgroundJobs;

/// <summary>
/// Records the coupon usages of newly placed orders, off the request thread. The logic stays on
/// <see cref="CouponUsageRecordHandler.HandleCouponUsages"/>, which existing customizations may override.
/// </summary>
public class CouponUsageRecordJob(CouponUsageRecordHandler couponUsageRecordHandler, IDistributedLock distributedLock)
    : IBackgroundJobHandler<CouponUsageRecordJobPayload>
{
    // Replaces Hangfire's [DisableConcurrentExecution(10)] with a distributed lock and the same 10-second wait. As with
    // Hangfire, a job that cannot get the lock in time fails and the engine retries it: skipping instead would lose a
    // coupon usage record.
    private const string LockResource = "marketing:job:record-coupon-usages";
    private static readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(10);

    public virtual Task Execute(CouponUsageRecordJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return distributedLock.ExecuteAsync(LockResource, _ => couponUsageRecordHandler.HandleCouponUsages(payload.JobArguments), _lockTimeout, cancellationToken);
    }
}
