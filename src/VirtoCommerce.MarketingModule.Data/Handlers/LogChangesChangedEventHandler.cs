using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Data.BackgroundJobs;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.MarketingModule.Data.Handlers
{
    public class LogChangesChangedEventHandler : IEventHandler<PromotionChangedEvent>, IEventHandler<CouponChangedEvent>
    {
        private readonly IChangeLogService _changeLogService;

        public LogChangesChangedEventHandler(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        public virtual Task Handle(PromotionChangedEvent @event)
        {
            return InnerHandle(@event);
        }

        public virtual Task Handle(CouponChangedEvent @event)
        {
            return InnerHandle(@event);
        }

        // Returns Task instead of void: enqueuing is asynchronous now. Breaking for an already-compiled override,
        // which stops overriding the signature Handle calls and would be silently skipped.
        protected virtual Task InnerHandle<T>(GenericChangedEntryEvent<T> @event) where T : IEntity
        {
            var logOperations = @event.ChangedEntries.Select(x => AbstractTypeFactory<OperationLog>.TryCreateInstance().FromChangedEntry(x)).ToArray();

            var payload = AbstractTypeFactory<LogEntityChangesJobPayload>.TryCreateInstance();
            payload.OperationLogs = logOperations;

            // Background task is used here for performance reasons.
            // The static facade, not an injected IBackgroundJob: RegisterEventHandler resolves this handler once from
            // the root provider and holds it for the process lifetime, so it must not capture a Scoped dependency.
            return BackgroundJob.Enqueue<LogEntityChangesJobHandler>(payload);
        }

        /// <summary>
        /// Kept for background jobs enqueued by an earlier version, which reference this method by name.
        /// New work goes through <see cref="LogEntityChangesJobHandler"/>; remove this once no such job
        /// can still be pending.
        /// </summary>
        // Signature is byte-identical on purpose: Hangfire persists a queued job as type name + method name +
        // parameter types + serialized args, so changing any of them would strand already-queued entries as Failed.
        [Obsolete("Enqueued indirectly by legacy Hangfire jobs only; new work uses LogEntityChangesJobHandler.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public void LogEntityChangesInBackground(OperationLog[] operationLogs)
        {
            _changeLogService.SaveChangesAsync(operationLogs).GetAwaiter().GetResult();
        }
    }
}
