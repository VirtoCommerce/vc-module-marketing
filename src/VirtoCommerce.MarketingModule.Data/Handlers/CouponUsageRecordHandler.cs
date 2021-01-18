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
            BackgroundJob.Enqueue(() => HandleCouponUsages(message));

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
        public virtual async Task HandleCouponUsages(OrderChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries)
            {
                if (changedEntry.EntryState == EntryState.Added)
                {
                    var oldUsages = new List<PromotionUsage>();
                    var newUsages = GetCouponUsages(changedEntry.NewEntry.Id, changedEntry.NewEntry, changedEntry.NewEntry.CustomerId, changedEntry.NewEntry.CustomerName);
                    await RecordUsages(changedEntry.NewEntry.Id, oldUsages, newUsages);
                }
            }
        }

        private async Task RecordUsages(string objectId, IEnumerable<PromotionUsage> oldUsages, IEnumerable<PromotionUsage> newUsages)
        {
            var toAddUsages = newUsages.Except(oldUsages, EqualityComparer);
            var toRemoveUsages = oldUsages.Except(newUsages, EqualityComparer);
            if (!toAddUsages.IsNullOrEmpty())
            {
                await ValidateUsagesAndThrow(toAddUsages);
                await _usageService.SaveUsagesAsync(toAddUsages.ToArray());
            }
            if (!toRemoveUsages.IsNullOrEmpty())
            {
                var alreadyExistUsages = (await _promotionUsageSearchService.SearchUsagesAsync(new PromotionUsageSearchCriteria { ObjectId = objectId })).Results;
                await _usageService.DeleteUsagesAsync(alreadyExistUsages.Intersect(toRemoveUsages, EqualityComparer).Select(x => x.Id).ToArray());
            }
        }

        private async Task ValidateUsagesAndThrow(IEnumerable<PromotionUsage> usages)
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

        private List<PromotionUsage> GetCouponUsages(string objectId, IHasDiscounts hasDiscounts, string customerId, string customerName)
        {
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
                .Distinct(usageComparer)
                .ToList();

            return result;
        }
    }
}
