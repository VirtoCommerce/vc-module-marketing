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
    public class PromotionUsageService : IPromotionUsageService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public PromotionUsageService(Func<IMarketingRepository> repositoryFactory, IEventPublisher eventPublisher, IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _eventPublisher = eventPublisher;
            _platformMemoryCache = platformMemoryCache;
        }

        #region IMarketingUsageService Members

        public virtual Task<PromotionUsage[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids));
            return _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                using (var repository = _repositoryFactory())
                {
                    //It is so important to generate change tokens for all ids even for not existing objects to prevent an issue
                    //with caching of empty results for non - existing objects that have the infinitive lifetime in the cache
                    //and future unavailability to create objects with these ids.
                    cacheEntry.AddExpirationToken(PromotionUsageCacheRegion.CreateChangeToken(ids));

                    var promotionUsages = await repository.GetMarketingUsagesByIdsAsync(ids);
                    var usages = promotionUsages.Select(x => x.ToModel(AbstractTypeFactory<PromotionUsage>.TryCreateInstance())).ToArray();

                    return usages;
                }
            });
        }

        public virtual async Task SaveUsagesAsync(PromotionUsage[] usages)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<PromotionUsage>>();
            using (var repository = _repositoryFactory())
            {
                var existUsageEntities = await repository.GetMarketingUsagesByIdsAsync(usages.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var usage in usages)
                {
                    var sourceEntity = AbstractTypeFactory<PromotionUsageEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(usage, pkMap);
                        var targetUsageEntity = existUsageEntities.FirstOrDefault(x => x.Id == usage.Id);
                        if (targetUsageEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<PromotionUsage>(usage, sourceEntity.ToModel(AbstractTypeFactory<PromotionUsage>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetUsageEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<PromotionUsage>(usage, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                ClearCache(usages);

                await _eventPublisher.Publish(new PromotionUsageChangedEvent(changedEntries));
            }
        }

        public virtual async Task DeleteUsagesAsync(string[] ids)
        {
            var usages = await GetByIdsAsync(ids);
            using (var repository = _repositoryFactory())
            {
                await repository.RemoveMarketingUsagesAsync(ids);
                await repository.UnitOfWork.CommitAsync();
            }

            ClearCache(usages);
        }

        #endregion

        private static void ClearCache(PromotionUsage[] usages)
        {
            PromotionUsageSearchCacheRegion.ExpireRegion();
            PromotionUsageCacheRegion.ExpireUsages(usages);
        }
    }
}
