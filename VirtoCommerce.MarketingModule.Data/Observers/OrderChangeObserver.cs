using System;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model.Promotions;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Observers
{
    public class OrderChangeObserver : IObserver<OrderChangeEvent>
    {
        private readonly ICouponService _couponService;

        public OrderChangeObserver(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public void OnNext(OrderChangeEvent value)
        {
            if (value.ChangeState == EntryState.Added)
            {
                var couponDiscount = value.ModifiedOrder.Discounts.FirstOrDefault(d => d.Coupon != null);
                if (couponDiscount != null)
                {
                    _couponService.ApplyCouponUsage(new ApplyCouponRequest
                    {
                        CouponCode = couponDiscount.Coupon.Code,
                        MemberId = value.ModifiedOrder.CustomerId,
                        OrderId = value.ModifiedOrder.Id,
                        OrderNumber = value.ModifiedOrder.Number,
                        PromotionId = couponDiscount.PromotionId
                    });
                }
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}