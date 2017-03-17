using System;
using System.Linq;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Observers
{
    public class OrderChangeObserver : IObserver<OrderChangeEvent>
    {
        public OrderChangeObserver(Func<IMarketingRepository> marketingRepositoryFactory)
        {
            _marketingRepositoryFactory = marketingRepositoryFactory;
        }

        private readonly Func<IMarketingRepository> _marketingRepositoryFactory;

        public void OnNext(OrderChangeEvent value)
        {
            using (var repository = _marketingRepositoryFactory())
            {
                var order = value.ModifiedOrder;
                if (value.ChangeState == EntryState.Added && order != null)
                {
                    var couponPromotion = order.Discounts.FirstOrDefault();
                    if (couponPromotion != null)
                    {
                        repository.Add(new PromotionUsage
                        {
                            CouponCode = couponPromotion.Coupon.Code,
                            MemberId = order.CustomerId,
                            MemberName = order.CustomerName,
                            OrderId = order.Id,
                            OrderNumber = order.Number,
                            PromotionId = couponPromotion.PromotionId,
                            UsageDate = DateTime.UtcNow
                        });

                        var reservedUsage = repository.PromotionUsages.FirstOrDefault(pu =>
                            pu.CouponCode == couponPromotion.Coupon.Code &&
                            pu.PromotionId == couponPromotion.PromotionId &&
                            pu.MemberId == order.CustomerId &&
                            string.IsNullOrEmpty(pu.OrderId));
                        if (reservedUsage != null)
                        {
                            repository.Remove(reservedUsage);
                        }

                        repository.UnitOfWork.Commit();
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