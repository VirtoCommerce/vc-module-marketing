using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Caching;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class CouponService : ICouponService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public CouponService(Func<IMarketingRepository> repositoryFactory, IEventPublisher eventPublisher, IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _eventPublisher = eventPublisher;
            _platformMemoryCache = platformMemoryCache;
        }

        #region ICouponService members

        public Task<Coupon[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetByIdsAsync", string.Join("-", ids));
            return _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(CouponCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var coupons = await repository.GetCouponsByIdsAsync(ids);
                    return coupons.Select(x => x.ToModel(AbstractTypeFactory<Coupon>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task SaveCouponsAsync(Coupon[] coupons)
        {
            if (coupons.Any(x => x.Code.IsNullOrEmpty()))
            {
                throw new InvalidOperationException($"Coupon can't have empty code!");
            }

            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<Coupon>>();
            
            using (var repository = _repositoryFactory())
            {
                var existCouponEntities = await repository.GetCouponsByIdsAsync(coupons.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());

                var nonUniqueCouponErrors = await repository.CheckCouponsForUniquenessAsync(coupons.Where(x => x.IsTransient()).ToArray());
                if (!nonUniqueCouponErrors.IsNullOrEmpty())
                {
                    throw new InvalidOperationException(string.Join(Environment.NewLine, nonUniqueCouponErrors));
                }

                foreach (var coupon in coupons)
                {
                    var sourceEntity = AbstractTypeFactory<CouponEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(coupon, pkMap);
                        var targetCouponEntity = existCouponEntities.FirstOrDefault(x => x.Id == coupon.Id);
                        if (targetCouponEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<Coupon>(coupon, sourceEntity.ToModel(AbstractTypeFactory<Coupon>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetCouponEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<Coupon>(coupon, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                ClearCache(coupons.Select(x => x.PromotionId).ToArray());

                await _eventPublisher.Publish(new CouponChangedEvent(changedEntries));
            }
        }

        public async Task DeleteCouponsAsync(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                await repository.RemoveCouponsAsync(ids);
                await repository.UnitOfWork.CommitAsync();
            }

            ClearCache();
        }

        private void ClearCache(string[] promotionIds = null)
        {
            CouponCacheRegion.ExpireRegion();
            PromotionSearchCacheRegion.ExpireRegion();

            if (promotionIds != null)
            {
                PromotionCacheRegion.ExpirePromotions(promotionIds);
            }
        }
        #endregion
    }
}
