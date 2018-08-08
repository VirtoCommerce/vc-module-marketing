using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Events;
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
        public CouponUsageRecordHandler(IPromotionUsageService usageService)
        {
            _usageService = usageService;
            EqualityComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));
        }

        private IEqualityComparer<PromotionUsage> EqualityComparer { get; set; }

        #region Implementation of IHandler<in OrderChangedEvent>

        public Task Handle(OrderChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries)
            {
                if (changedEntry.EntryState == EntryState.Added)
                {
                    var oldUsages = new List<PromotionUsage>();
                    var newUsages = GetCouponUsages(changedEntry.NewEntry.Id, changedEntry.NewEntry, changedEntry.NewEntry.CustomerId, changedEntry.NewEntry.CustomerName);
                    RecordUsages(changedEntry.NewEntry.Id, oldUsages, newUsages);
                }
            }
            return Task.CompletedTask;
        }

        #endregion

        private void RecordUsages(string objectId, IEnumerable<PromotionUsage> oldUsages, IEnumerable<PromotionUsage> newUsages)
        {
            var toAddUsages = newUsages.Except(oldUsages, EqualityComparer);
            var toRemoveUsages = oldUsages.Except(newUsages, EqualityComparer);
            if (!toAddUsages.IsNullOrEmpty())
            {
                _usageService.SaveUsages(toAddUsages.ToArray());
            }
            if (!toRemoveUsages.IsNullOrEmpty())
            {
                var alreadyExistUsages = _usageService.SearchUsages(new PromotionUsageSearchCriteria { ObjectId = objectId }).Results;
                _usageService.DeleteUsages(alreadyExistUsages.Intersect(toRemoveUsages, EqualityComparer).Select(x => x.Id).ToArray());
            }
        }

        private List<PromotionUsage> GetCouponUsages(string objectId, IHasDiscounts hasDiscounts, string customerId, string customerName)
        {
            var usageComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));
            var retVal = hasDiscounts.GetFlatObjectsListWithInterface<IHasDiscounts>()
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
            return retVal;
        }
    }
}
