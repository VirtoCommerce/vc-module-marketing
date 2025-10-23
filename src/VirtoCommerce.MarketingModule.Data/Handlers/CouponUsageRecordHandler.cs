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

namespace VirtoCommerce.MarketingModule.Data.Handlers;

/// <summary>
/// Represents the logic of recording the use of coupons on both levels (Cart and Order).
/// </summary>
public class CouponUsageRecordHandler(
    IPromotionUsageService usageService,
    IPromotionUsageSearchService promotionUsageSearchService,
    IPromotionService promotionService)
    : IEventHandler<OrderChangedEvent>
{
    private readonly IEqualityComparer<PromotionUsage> _promotionUsageComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));

    public virtual Task Handle(OrderChangedEvent message)
    {
        var couponUsageJobArguments = message.ChangedEntries
            .Where(x => x.EntryState == EntryState.Added)
            .Select(x => GetJobArgumentsForCouponUsageRecord(x.NewEntry))
            .ToArray();

        if (couponUsageJobArguments.Length > 0)
        {
            BackgroundJob.Enqueue(() => HandleCouponUsages(couponUsageJobArguments));
        }

        return Task.CompletedTask;
    }

    [DisableConcurrentExecution(10)]
    // "DisableConcurrentExecutionAttribute" prevents to start simultaneous job payloads.
    // Should have short timeout, because this attribute implemented by following manner: newly started job falls into "processing" state immediately.
    // Then it tries to receive job lock during timeout. If the lock received, the job starts payload.
    // When the job is awaiting desired timeout for lock release, it is stuck in "processing" anyway. (Therefore, you should not to set long timeouts (like 24*60*60), this will cause a lot of stuck jobs and performance degradation.)
    // Then, if timeout is over and the lock NOT acquired, the job falls into "scheduled" state (this is default fail-retry scenario).
    // Failed job goes to "Failed" state (by default) after retries exhausted.
    public virtual async Task HandleCouponUsages(CouponUsageRecordJobArgument[] jobArguments)
    {
        foreach (var jobArgument in jobArguments)
        {
            await RecordUsages(jobArgument.OrderId, oldUsages: [], jobArgument.PromotionUsages);
        }
    }

    private async Task RecordUsages(string objectId, PromotionUsage[] oldUsages, PromotionUsage[] newUsages)
    {
        var usagesToAdd = newUsages.Except(oldUsages, _promotionUsageComparer).ToArray();
        var usagesToRemove = oldUsages.Except(newUsages, _promotionUsageComparer).ToArray();

        if (usagesToAdd.Length > 0)
        {
            await ValidateUsagesAndThrow(usagesToAdd);
            await usageService.SaveChangesAsync(usagesToAdd);
        }

        if (usagesToRemove.Length > 0)
        {
            var searchCriteria = AbstractTypeFactory<PromotionUsageSearchCriteria>.TryCreateInstance();
            searchCriteria.ObjectId = objectId;

            var existingUsages = await promotionUsageSearchService.SearchAllNoCloneAsync(searchCriteria);

            var idsToDelete = existingUsages.Intersect(usagesToRemove, _promotionUsageComparer)
                .Select(x => x.Id)
                .ToArray();

            await usageService.DeleteAsync(idsToDelete);
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
        var promotions = await promotionService.GetAsync(promotionIds);

        invalidUsage = usages.FirstOrDefault(usage => promotions.All(promotion => promotion.Id != usage.PromotionId));
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
            .Where(x => !x.Coupon.IsNullOrEmpty())
            .Select(x =>
            {
                var usage = AbstractTypeFactory<PromotionUsage>.TryCreateInstance();
                usage.CouponCode = x.Coupon;
                usage.PromotionId = x.PromotionId;
                usage.ObjectId = objectId;
                usage.ObjectType = hasDiscounts.GetType().Name;
                usage.UserId = customerId;
                usage.UserName = customerName;
                return usage;
            })
            .Distinct(usageComparer);

        return new CouponUsageRecordJobArgument { OrderId = objectId, PromotionUsages = result.ToArray() };
    }
}

public class CouponUsageRecordJobArgument
{
    public string OrderId { get; set; }

    public PromotionUsage[] PromotionUsages { get; set; }
}
