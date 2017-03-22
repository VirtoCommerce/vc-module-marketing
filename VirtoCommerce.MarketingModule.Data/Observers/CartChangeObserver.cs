using System;
using System.Linq;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Marketing.Model.Promotions;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Observers
{
    public class CartChangeObserver : IObserver<CartChangeEvent>
    {
        private readonly ICouponService _couponService;

        public CartChangeObserver(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public void OnNext(CartChangeEvent value)
        {
            if (value.ChangeState == EntryState.Modified)
            {
                var couponDiscount = value.ModifiedCart.Discounts.FirstOrDefault(p => !string.IsNullOrEmpty(p.Coupon));
                if (couponDiscount != null)
                {
                    var applyCouponRequest = new ApplyCouponRequest
                    {
                        CouponCode = value.ModifiedCart.Coupon.Code,
                        MemberId = value.ModifiedCart.CustomerId,
                        PromotionId = couponDiscount.PromotionId
                    };

                    // A coupon was applied to a shopping cart
                    if (value.ModifiedCart.Coupon != null && value.ModifiedCart.Coupon.IsValid)
                    {
                        _couponService.ApplyCouponUsage(applyCouponRequest);
                    }

                    // A coupon was removed from a shopping cart
                    if (value.OrigCart.Coupon != null && value.ModifiedCart.Coupon == null)
                    {
                        _couponService.RemoveCouponUsage(applyCouponRequest);
                    }
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