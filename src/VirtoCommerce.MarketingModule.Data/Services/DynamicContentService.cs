using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Caching;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DynamicContentService : IDynamicContentService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public DynamicContentService(Func<IMarketingRepository> repositoryFactory, IEventPublisher eventPublisher, IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _eventPublisher = eventPublisher;
            _platformMemoryCache = platformMemoryCache;
        }

        #region IDynamicContentService Members

        #region DynamicContentItem methods

        public async Task<DynamicContentItem[]> GetContentItemsByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetContentItemsByIdsAsync", string.Join("-", ids));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicContentItemCacheRegion.CreateChangeToken());
                DynamicContentItem[] retVal = null;
                using (var repository = _repositoryFactory())
                {
                    retVal = (await repository.GetContentItemsByIdsAsync(ids)).Select(x => x.ToModel(AbstractTypeFactory<DynamicContentItem>.TryCreateInstance())).ToArray();
                }
                return retVal;
            });
        }

        public async Task SaveContentItemsAsync(DynamicContentItem[] items)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DynamicContentItem>>();
            using (var repository = _repositoryFactory())
            {
                var existEntities = await repository.GetContentItemsByIdsAsync(items.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var item in items)
                {
                    var sourceEntity = AbstractTypeFactory<DynamicContentItemEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(item, pkMap);
                        var targetEntity = existEntities.FirstOrDefault(x => x.Id == item.Id);
                        if (targetEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentItem>(item, targetEntity.ToModel(AbstractTypeFactory<DynamicContentItem>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentItem>(item, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }

                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                DynamicContentItemCacheRegion.ExpireRegion();

                await _eventPublisher.Publish(new DynamicContentItemChangedEvent(changedEntries));
            }
        }

        public async Task DeleteContentItemsAsync(string[] ids)
        {
            var items = await GetContentItemsByIdsAsync(ids);
            var changedEntries = items.Select(x => new GenericChangedEntry<DynamicContentItem>(x, EntryState.Deleted));
            using (var repository = _repositoryFactory())
            {
                await repository.RemoveContentItemsAsync(ids);
                await repository.UnitOfWork.CommitAsync();
            }

            DynamicContentItemCacheRegion.ExpireRegion();

            await _eventPublisher.Publish(new DynamicContentItemChangedEvent(changedEntries));
        }

        #endregion

        #region DynamicContentPlace methods

        public async Task<DynamicContentPlace[]> GetPlacesByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetPlacesByIdsAsync", string.Join("-", ids));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicContentPlaceCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var contentPlaces = await repository.GetContentPlacesByIdsAsync(ids);
                    return contentPlaces
                        .Select(x => x.ToModel(AbstractTypeFactory<DynamicContentPlace>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task SavePlacesAsync(DynamicContentPlace[] places)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DynamicContentPlace>>();
            using (var repository = _repositoryFactory())
            {
                var existEntities = await repository.GetContentPlacesByIdsAsync(places.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var place in places)
                {
                    var sourceEntity = AbstractTypeFactory<DynamicContentPlaceEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(place, pkMap);
                        var targetEntity = existEntities.FirstOrDefault(x => x.Id == place.Id);
                        if (targetEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentPlace>(place, targetEntity.ToModel(AbstractTypeFactory<DynamicContentPlace>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentPlace>(place, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }

                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                DynamicContentPlaceCacheRegion.ExpireRegion();

                await _eventPublisher.Publish(new DynamicContentPlaceChangedEvent(changedEntries));
            }
        }

        public async Task DeletePlacesAsync(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                await repository.RemovePlacesAsync(ids);
                await repository.UnitOfWork.CommitAsync();
            }

            DynamicContentPlaceCacheRegion.ExpireRegion();
        }

        #endregion

        #region DynamicContentPublication methods

        public async Task<DynamicContentPublication[]> GetPublicationsByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetPublicationsByIdsAsync", string.Join("-", ids));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicContentPublicationCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var publications = await repository.GetContentPublicationsByIdsAsync(ids);
                    return publications.Select(x => x.ToModel(AbstractTypeFactory<DynamicContentPublication>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task SavePublicationsAsync(DynamicContentPublication[] publications)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DynamicContentPublication>>();
            using (var repository = _repositoryFactory())
            {
                var existEntities = await repository.GetContentPublicationsByIdsAsync(publications.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var publication in publications)
                {
                    var sourceEntity = AbstractTypeFactory<DynamicContentPublishingGroupEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(publication, pkMap);
                        var targetEntity = existEntities.FirstOrDefault(x => x.Id == publication.Id);
                        if (targetEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentPublication>(publication, targetEntity.ToModel(AbstractTypeFactory<DynamicContentPublication>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentPublication>(publication, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                DynamicContentPublicationCacheRegion.ExpireRegion();

                await _eventPublisher.Publish(new DynamicContentPublicationChangedEvent(changedEntries));
            }
        }

        public async Task DeletePublicationsAsync(string[] ids)
        {
            using (var repository = _repositoryFactory())
            { 
                await repository.RemoveContentPublicationsAsync(ids);
                await repository.UnitOfWork.CommitAsync();
            }

            DynamicContentPublicationCacheRegion.ExpireRegion();
        }
      
        #endregion

        #region DynamicContentFolder methods

        public async Task<DynamicContentFolder[]> GetFoldersByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetFoldersByIdsAsync", string.Join("-", ids));
            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.AddExpirationToken(DynamicContentFolderCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    var folders = await repository.GetContentFoldersByIdsAsync(ids);
                    return folders.Select(x => x.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance())).ToArray();
                }
            });
        }

        public async Task SaveFoldersAsync(DynamicContentFolder[] folders)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DynamicContentFolder>>();
            using (var repository = _repositoryFactory())
            {
                var existEntities = await repository.GetContentFoldersByIdsAsync(folders.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var folder in folders)
                {
                    var sourceEntity = AbstractTypeFactory<DynamicContentFolderEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(folder, pkMap);
                        var targetEntity = existEntities.FirstOrDefault(x => x.Id == folder.Id);
                        if (targetEntity != null)
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentFolder>(folder, targetEntity.ToModel(AbstractTypeFactory<DynamicContentFolder>.TryCreateInstance()), EntryState.Modified));
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            changedEntries.Add(new GenericChangedEntry<DynamicContentFolder>(folder, EntryState.Added));
                            repository.Add(sourceEntity);
                        }
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();

                DynamicContentFolderCacheRegion.ExpireRegion();

                await _eventPublisher.Publish(new DynamicContentFolderChangedEvent(changedEntries));
            }
        }

        public async Task DeleteFoldersAsync(string[] ids)
        {

            await RemoveChildren(ids);

            using (var repository = _repositoryFactory())
            {
                await repository.RemoveFoldersAsync(ids);
            }

            DynamicContentFolderCacheRegion.ExpireRegion();
        }

        private async Task RemoveChildren(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var children  = repository.Folders.Where(x => ids.Contains(x.ParentFolderId)).ToArray();

                if (!children.IsNullOrEmpty())
                {
                    await RemoveChildren(children.Select(x => x.Id).ToArray());
                }

                await repository.RemoveFoldersAsync(ids);
            }
        }

        #endregion

        #endregion
    }
}
