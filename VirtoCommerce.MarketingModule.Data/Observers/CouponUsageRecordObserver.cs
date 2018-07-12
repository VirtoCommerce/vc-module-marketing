using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Observers
{
    /// <summary>
    /// Represents the logic of recording the use of coupons on both levels (Cart and Order).
    /// </summary>
    public class CouponUsageRecordObserver : IObserver<CartChangedEvent>, IObserver<OrderChangedEvent>
    {
        private readonly IPromotionUsageService _usageService;
        public CouponUsageRecordObserver(IPromotionUsageService usageService)
        {
            _usageService = usageService;
            EqualityComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));
        }

        private IEqualityComparer<PromotionUsage> EqualityComparer { get; set; }

        public void OnNext(OrderChangedEvent changedEvent)
        {
            //if (changedEvent.ChangeState == EntryState.Added)
            //{
            //    var oldUsages = new List<PromotionUsage>();
            //    var newUsages = GetCouponUsages(changedEvent.ModifiedOrder.Id, changedEvent.ModifiedOrder);               
            //    RecordUsages(changedEvent.ModifiedOrder.Id, oldUsages, newUsages);
            //}
        }

        public void OnNext(CartChangedEvent changedEvent)
        {
            //var oldUsages = GetCouponUsages(changedEvent.OrigCart.Id, changedEvent.OrigCart);
            //var newUsages = GetCouponUsages(changedEvent.ModifiedCart.Id, changedEvent.ModifiedCart);

            //if (changedEvent.ChangeState == EntryState.Added)
            //{
            //    oldUsages.Clear();
            //}         
            //if (changedEvent.ChangeState == EntryState.Deleted)
            //{
            //    newUsages.Clear();
            //}
            //RecordUsages(changedEvent.OrigCart.Id, oldUsages, newUsages);
        }

        private void RecordUsages(string objectId, IEnumerable<PromotionUsage> oldUsages, IEnumerable<PromotionUsage> newUsages)
        {
            var toAddUsages = newUsages.Except(oldUsages, EqualityComparer);
            var toRemoveUsages = oldUsages.Except(newUsages, EqualityComparer);
            if(!toAddUsages.IsNullOrEmpty())
            {
                _usageService.SaveUsages(toAddUsages.ToArray());
            }
            if (!toRemoveUsages.IsNullOrEmpty())
            {
                var alreadyExistUsages = _usageService.SearchUsages(new PromotionUsageSearchCriteria { ObjectId = objectId }).Results;
                _usageService.DeleteUsages(alreadyExistUsages.Intersect(toRemoveUsages, EqualityComparer).Select(x => x.Id).ToArray());
            }
        }

        private List<PromotionUsage> GetCouponUsages(string objectId, IHasDiscounts hasDiscounts)
        {
            var usageComparer = AnonymousComparer.Create((PromotionUsage x) => string.Join(":", x.PromotionId, x.CouponCode, x.ObjectId));
            var retVal = hasDiscounts.GetFlatObjectsListWithInterface<IHasDiscounts>()
                                                 .Where(x => x.Discounts != null)
                                                 .SelectMany(x => x.Discounts)
                                                 .Where(x => !string.IsNullOrEmpty(x.Coupon))
                                                 .Select(x => new PromotionUsage { CouponCode = x.Coupon, PromotionId = x.PromotionId, ObjectId = objectId, ObjectType = hasDiscounts.GetType().Name })
                                                 .Distinct(usageComparer)
                                                 .ToList();
            return retVal;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

    }

    
}