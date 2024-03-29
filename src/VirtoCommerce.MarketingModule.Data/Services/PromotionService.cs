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
    public class PromotionService : IPromotionService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IEventPublisher _eventPublisher;

        public PromotionService(Func<IMarketingRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
        }

        #region IMarketingService Members       

        public virtual async Task<Promotion[]> GetPromotionsByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetPromotionsByIdsAsync), string.Join("-", ids));
            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                //It is so important to generate change tokens for all ids even for not existing objects to prevent an issue
                //with caching of empty results for non - existing objects that have the infinitive lifetime in the cache
                //and future unavailability to create objects with these ids.
                cacheEntry.AddExpirationToken(PromotionCacheRegion.CreateChangeToken(ids));

                using (var repository = _repositoryFactory())
                {
                    var promotionEntities = await repository.GetPromotionsByIdsAsync(ids);
                    return promotionEntities.Select(x => x.ToModel(AbstractTypeFactory<Promotion>.TryCreateInstance())).ToArray();
                }
            });

            return result.Select(x => x.Clone() as Promotion).ToArray();
        }

        public virtual async Task SavePromotionsAsync(Promotion[] promotions)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<Promotion>>();
            using (var repository = _repositoryFactory())
            {
                var existEntities = await repository.GetPromotionsByIdsAsync(promotions.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var promotion in promotions)
                {
                    var sourceEntity = AbstractTypeFactory<PromotionEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(promotion, pkMap);
                        var targetEntity = existEntities.FirstOrDefault(x => x.Id == promotion.Id);
                        if (targetEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<Promotion>(promotion, targetEntity.ToModel(AbstractTypeFactory<Promotion>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<Promotion>(promotion, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                ClearCache(promotions.Select(x => x.Id).ToArray());

                await _eventPublisher.Publish(new PromotionChangedEvent(changedEntries));
            }
        }

        public virtual async Task DeletePromotionsAsync(string[] ids)
        {
            var models = await GetPromotionsByIdsAsync(ids);

            using (var repository = _repositoryFactory())
            {

                await repository.RemovePromotionsAsync(ids);
                await repository.UnitOfWork.CommitAsync();
                var changedEntries = new List<GenericChangedEntry<Promotion>>();

                foreach (var model in models)
                {
                    changedEntries.Add(new GenericChangedEntry<Promotion>(model, EntryState.Deleted));
                }

                ClearCache(ids);

                //Raise domain events after deletion
                await _eventPublisher.Publish(new PromotionChangedEvent(changedEntries));
            }
        }

        #endregion

        protected virtual void ClearCache(string[] promotionIds)
        {
            PromotionSearchCacheRegion.ExpireRegion();
            PromotionCacheRegion.ExpirePromotions(promotionIds);
        }
    }
}
