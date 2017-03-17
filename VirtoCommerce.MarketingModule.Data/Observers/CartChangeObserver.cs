using System;
using System.Linq;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Observers
{
    public class CartChangeObserver : IObserver<CartChangeEvent>
    {
        public CartChangeObserver(Func<IMarketingRepository> marketingRepositoryFactory)
        {
            _marketingRepositoryFactory = marketingRepositoryFactory;
        }

        private readonly Func<IMarketingRepository> _marketingRepositoryFactory;

        public void OnNext(CartChangeEvent value)
        {
            using (var repository = _marketingRepositoryFactory())
            {
                if (value.ChangeState == EntryState.Modified && value.OrigCart != null && value.ModifiedCart != null)
                {
                    // A coupon for a cart was applied
                    if (value.OrigCart.Coupon == null && value.ModifiedCart.Coupon != null)
                    {
                        var discount = value.ModifiedCart.Discounts.FirstOrDefault();
                        if (discount != null)
                        {
                            repository.Add(new PromotionUsage
                            {
                                CouponCode = value.ModifiedCart.Coupon.Code,
                                MemberId = value.ModifiedCart.CustomerId,
                                MemberName = value.ModifiedCart.CustomerName,
                                PromotionId = discount.PromotionId,
                                UsageDate = DateTime.UtcNow
                            });
                            repository.UnitOfWork.Commit();
                        }
                    }

                    // A coupon from a cart was removed
                    if (value.OrigCart.Coupon != null && value.ModifiedCart.Coupon == null)
                    {
                        var discount = value.OrigCart.Discounts.FirstOrDefault();
                        if (discount != null)
                        {
                            var promotionUsage = repository.PromotionUsages.FirstOrDefault(pu =>
                                pu.CouponCode == value.OrigCart.Coupon.Code &&
                                pu.MemberId == value.OrigCart.CustomerId &&
                                pu.PromotionId == discount.PromotionId);
                            if (promotionUsage != null)
                            {
                                repository.Remove(promotionUsage);
                                repository.UnitOfWork.Commit();
                            }
                        }
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