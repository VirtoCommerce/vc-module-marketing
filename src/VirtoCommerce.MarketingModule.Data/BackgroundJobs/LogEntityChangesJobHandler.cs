using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Data.Handlers;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.MarketingModule.Data.BackgroundJobs;

/// <summary>
/// Persists the promotion and coupon change-log entries collected by <see cref="LogChangesChangedEventHandler"/>,
/// off the request thread.
/// </summary>
public class LogEntityChangesJobHandler(IChangeLogService changeLogService, IDistributedLock distributedLock)
    : IBackgroundJobHandler<LogEntityChangesJobPayload>
{
    // Replaces Hangfire's [DisableConcurrentExecution(10)] with a distributed lock and the same 10-second wait. As with
    // Hangfire, a job that cannot get the lock in time fails and the engine retries it: skipping instead would lose a
    // change-log entry.
    private const string LockResource = "marketing:job:log-entity-changes";
    private static readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(10);

    public virtual Task Execute(LogEntityChangesJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return distributedLock.ExecuteAsync(LockResource, _ => changeLogService.SaveChangesAsync(payload.OperationLogs), _lockTimeout, cancellationToken);
    }
}
