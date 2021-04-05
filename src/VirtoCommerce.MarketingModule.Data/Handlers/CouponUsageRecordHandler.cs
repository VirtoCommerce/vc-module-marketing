using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.OrdersModule.Core.Events;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Data.Handlers
{


    /// <summary>
    /// Represents the logic of recording the use of coupons on both levels (Cart and Order).
    /// </summary>
    public class CouponUsageRecordHandler : IEventHandler<OrderChangedEvent>
    {
        private readonly IPromotionUsageService _usageService;
        private readonly IPromotionUsageSearchService _promotionUsageSearchService;
        private readonly IPromotionService _promotionService;

        public CouponUsageRecordHandler(IPromotionUsageService usageService, IPromotionUsageSearchService promotionUsageSearchService, IPromotionService promotionService)
        {
            _usageService = usageService;
            _promotionUsageSearchService = promotionUsageSearchService;
            EqualityComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));
            _promotionService = promotionService;
        }

        private IEqualityComparer<PromotionUsage> EqualityComparer { get; set; }

        #region Implementation of IHandler<in OrderChangedEvent>

        public virtual Task Handle(OrderChangedEvent message)
        {
            var couponUsageJobArguments = message.ChangedEntries.Where(x => x.EntryState == EntryState.Added).Select(x=> GetJobArgumentsForCouponUsageRecord(x.NewEntry));

            if (couponUsageJobArguments.Any())
            {
                BackgroundJob.Enqueue(() => HandleCouponUsages(couponUsageJobArguments.ToArray()));
            }

            return Task.CompletedTask;
        }

        #endregion

        [DisableConcurrentExecution(10)]
        // "DisableConcurrentExecutionAttribute" prevents to start simultaneous job payloads.
        // Should have short timeout, because this attribute implemented by following manner: newly started job falls into "processing" state immediately.
        // Then it tries to receive job lock during timeout. If the lock received, the job starts payload.
        // When the job is awaiting desired timeout for lock release, it stucks in "processing" anyway. (Therefore, you should not to set long timeouts (like 24*60*60), this will cause a lot of stucked jobs and performance degradation.)
        // Then, if timeout is over and the lock NOT acquired, the job falls into "scheduled" state (this is default fail-retry scenario).
        // Failed job goes to "Failed" state (by default) after retries exhausted.
        public virtual async Task HandleCouponUsages(CouponUsageRecordJobArgument[] jobArguments)
        {
            foreach (var jobArgument in jobArguments)
            {
                var oldUsages = new List<PromotionUsage>();
                await RecordUsages(jobArgument.OrderId, oldUsages, jobArgument.PromotionUsages);
            }
        }

        private async Task RecordUsages(string objectId, IEnumerable<PromotionUsage> oldUsages, IEnumerable<PromotionUsage> newUsages)
        {
            var toAddUsages = newUsages.Except(oldUsages, EqualityComparer).ToArray();
            var toRemoveUsages = oldUsages.Except(newUsages, EqualityComparer);
            if (!toAddUsages.IsNullOrEmpty())
            {
                await ValidateUsagesAndThrow(toAddUsages);
                await _usageService.SaveUsagesAsync(toAddUsages);
            }
            if (!toRemoveUsages.IsNullOrEmpty())
            {
                var alreadyExistUsages = (await _promotionUsageSearchService.SearchUsagesAsync(new PromotionUsageSearchCriteria { ObjectId = objectId })).Results;
                await _usageService.DeleteUsagesAsync(alreadyExistUsages.Intersect(toRemoveUsages, EqualityComparer).Select(x => x.Id).ToArray());
            }
        }

        private async Task ValidateUsagesAndThrow(IReadOnlyCollection<PromotionUsage> usages)
        {
            var invalidUsage = usages.FirstOrDefault(x => x.PromotionId.IsNullOrEmpty());
            if (invalidUsage != null)
            {
                throw new ValidationException($"Invalid PromotionUsage: CouponCode '{invalidUsage.CouponCode}' for objectId '{invalidUsage.ObjectId}' of type {invalidUsage.ObjectType}. PromotionId is missing!");
            }

            var promotionIds = usages.Select(x => x.PromotionId).ToArray();
            var promotions = await _promotionService.GetPromotionsByIdsAsync(promotionIds);

            invalidUsage = usages.FirstOrDefault(x => !promotions.Any(p => p.Id == x.PromotionId));
            if (invalidUsage != null)
            {
                throw new ValidationException($"Invalid PromotionUsage: CouponCode '{invalidUsage.CouponCode}' for objectId '{invalidUsage.ObjectId}' of type {invalidUsage.ObjectType}. Promotion with Id '{invalidUsage.PromotionId}' not found!");
            }
        }

        protected virtual CouponUsageRecordJobArgument GetJobArgumentsForCouponUsageRecord(CustomerOrder order)
        {
            var objectId = order.Id;
            IHasDiscounts hasDiscounts = order;
            var customerId = order.CustomerId;
            var customerName = order.CustomerName;
            var usageComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));
            var result = hasDiscounts.GetFlatObjectsListWithInterface<IHasDiscounts>()
                .Where(x => x.Discounts != null)
                .SelectMany(x => x.Discounts)
                .Where(x => !string.IsNullOrEmpty(x.Coupon))
                .Select(x => new PromotionUsage
                {
                    CouponCode = x.Coupon,
                    PromotionId = x.PromotionId,
                    ObjectId = objectId,
                    ObjectType = hasDiscounts.GetType().Name,
                    UserId = customerId,
                    UserName = customerName
                })
                .Distinct(usageComparer);

            return new CouponUsageRecordJobArgument() { OrderId = objectId, PromotionUsages = result.ToArray()};
        }
    }

    public class CouponUsageRecordJobArgument
    {
        public string OrderId { get; set; }

        public PromotionUsage[] PromotionUsages { get; set; }
    }
}
