using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Data.Handlers;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.MarketingModule.Data.BackgroundJobs;

// The jobs below replace Hangfire's [DisableConcurrentExecution(10)] with a distributed lock and the same 10-second
// wait. As with Hangfire, a job that cannot get the lock in time fails and the engine retries it: skipping instead
// would lose a coupon usage record or a change-log entry.

public class CouponUsageRecordJobPayload
{
    public CouponUsageRecordJobArgument[] JobArguments { get; set; } = [];
}

/// <summary>
/// Records the coupon usages of newly placed orders, off the request thread. The logic stays on
/// <see cref="CouponUsageRecordHandler.HandleCouponUsages"/>, which existing customizations may override.
/// </summary>
public class CouponUsageRecordJob(CouponUsageRecordHandler couponUsageRecordHandler, IDistributedLock distributedLock)
    : IBackgroundJobHandler<CouponUsageRecordJobPayload>
{
    private const string LockResource = "marketing:job:record-coupon-usages";
    private static readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(10);

    public virtual Task Execute(CouponUsageRecordJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return distributedLock.ExecuteAsync(LockResource, _ => couponUsageRecordHandler.HandleCouponUsages(payload.JobArguments), _lockTimeout, cancellationToken);
    }
}

public class LogEntityChangesJobPayload
{
    public OperationLog[] OperationLogs { get; set; }
}

/// <summary>
/// Persists the promotion and coupon change-log entries collected by <see cref="LogChangesChangedEventHandler"/>,
/// off the request thread.
/// </summary>
public class LogEntityChangesJobHandler(IChangeLogService changeLogService, IDistributedLock distributedLock)
    : IBackgroundJobHandler<LogEntityChangesJobPayload>
{
    private const string LockResource = "marketing:job:log-entity-changes";
    private static readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(10);

    public virtual Task Execute(LogEntityChangesJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return distributedLock.ExecuteAsync(LockResource, _ => changeLogService.SaveChangesAsync(payload.OperationLogs), _lockTimeout, cancellationToken);
    }
}
