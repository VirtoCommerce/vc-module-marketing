using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.MarketingModule.Data.Repositories;

public class MarketingRepository(MarketingDbContext dbContext)
    : DbContextRepositoryBase<MarketingDbContext>(dbContext),
        IMarketingRepository
{
    private readonly MarketingDbContext _dbContext = dbContext;
    private const int _defaultPageSize = 50;

    public IQueryable<PromotionEntity> Promotions => DbContext.Set<PromotionEntity>();

    public IQueryable<CouponEntity> Coupons => DbContext.Set<CouponEntity>();

    public IQueryable<PromotionUsageEntity> PromotionUsages => DbContext.Set<PromotionUsageEntity>();

    public IQueryable<DynamicContentFolderEntity> Folders => DbContext.Set<DynamicContentFolderEntity>();

    public IQueryable<DynamicContentItemEntity> Items => DbContext.Set<DynamicContentItemEntity>();

    public IQueryable<DynamicContentPlaceEntity> Places => DbContext.Set<DynamicContentPlaceEntity>();

    public IQueryable<DynamicContentPublishingGroupEntity> PublishingGroups => DbContext.Set<DynamicContentPublishingGroupEntity>();

    public IQueryable<PublishingGroupContentItemEntity> PublishingGroupContentItems => DbContext.Set<PublishingGroupContentItemEntity>();

    public IQueryable<PublishingGroupContentPlaceEntity> PublishingGroupContentPlaces => DbContext.Set<PublishingGroupContentPlaceEntity>();

    public IQueryable<PromotionStoreEntity> PromotionStores => DbContext.Set<PromotionStoreEntity>();

    public IQueryable<DynamicContentItemDynamicPropertyObjectValueEntity> DynamicContentItemDynamicPropertyObjectValues => DbContext.Set<DynamicContentItemDynamicPropertyObjectValueEntity>();

    public virtual async Task<IList<PromotionEntity>> GetPromotionsByIdsAsync(IList<string> ids)
    {
        var promotions = await Promotions.Where(x => ids.Contains(x.Id)).ToListAsync();

        if (promotions.Count > 0)
        {
            var promotionIds = promotions.Select(x => x.Id).ToArray();

            await PromotionStores.Where(x => promotionIds.Contains(x.PromotionId)).LoadAsync();

            var promotionIdsWithCoupons = await Coupons
                .Where(x => promotionIds.Contains(x.PromotionId))
                .Select(x => x.PromotionId)
                .Distinct()
                .ToArrayAsync();

            foreach (var promotion in promotions)
            {
                promotion.HasCoupons = promotionIdsWithCoupons.ContainsIgnoreCase(promotion.Id);
            }
        }

        return promotions;
    }

    public async Task<IList<DynamicContentFolderEntity>> GetContentFoldersByIdsAsync(IList<string> ids)
    {
        var folders = await Folders.Where(x => ids.Contains(x.Id)).ToListAsync();
        if (folders.Count > 0)
        {
            var childFolders = folders.Where(x => x.ParentFolderId != null).ToList();
            if (childFolders.Count > 0)
            {
                var parentFolderIds = childFolders.Select(x => x.ParentFolderId).DistinctIgnoreCase().ToList();
                var parentFolders = await GetContentFoldersByIdsAsync(parentFolderIds);
                foreach (var childFolder in childFolders)
                {
                    childFolder.ParentFolder = parentFolders.FirstOrDefault(x => x.Id.EqualsIgnoreCase(childFolder.ParentFolderId));
                }
            }
        }
        return folders;
    }

    public async Task<IList<DynamicContentItemEntity>> GetContentItemsByIdsAsync(IList<string> ids)
    {
        var items = await Items.Where(x => ids.Contains(x.Id)).ToListAsync();
        if (items.Count > 0)
        {
            var itemsWithFolder = items.Where(x => x.FolderId != null).ToList();
            if (itemsWithFolder.Count > 0)
            {
                var folderIds = itemsWithFolder.Select(x => x.FolderId).DistinctIgnoreCase().ToList();
                var folders = await GetContentFoldersByIdsAsync(folderIds);
                foreach (var item in itemsWithFolder)
                {
                    item.Folder = folders.FirstOrDefault(x => x.Id.EqualsIgnoreCase(item.FolderId));
                }
            }
            await DynamicContentItemDynamicPropertyObjectValues.Where(x => ids.Contains(x.ObjectId)).LoadAsync();
        }
        return items;
    }

    public async Task<IList<DynamicContentPlaceEntity>> GetContentPlacesByIdsAsync(IList<string> ids)
    {
        var places = await Places.Where(x => ids.Contains(x.Id)).ToListAsync();
        if (places.Count > 0)
        {
            var placesWithFolder = places.Where(x => x.FolderId != null).ToList();
            if (placesWithFolder.Count > 0)
            {
                var folderIds = placesWithFolder.Select(x => x.FolderId).DistinctIgnoreCase().ToArray();
                var folders = await GetContentFoldersByIdsAsync(folderIds);
                foreach (var place in placesWithFolder)
                {
                    place.Folder = folders.FirstOrDefault(x => x.Id.EqualsIgnoreCase(place.FolderId));
                }
            }
        }
        return places;
    }

    public async Task<IList<DynamicContentPublishingGroupEntity>> GetContentPublicationsByIdsAsync(IList<string> ids)
    {
        var result = await PublishingGroups.Where(i => ids.Contains(i.Id))
            .Include(x => x.ContentItems).ThenInclude(y => y.ContentItem)
            .Include(x => x.ContentPlaces).ThenInclude(y => y.ContentPlace)
            .ToListAsync();

        var contentItemIds = result.SelectMany(x => x.ContentItems).Select(x => x.DynamicContentItemId).DistinctIgnoreCase().ToArray();

        if (!contentItemIds.IsNullOrEmpty())
        {
            await DynamicContentItemDynamicPropertyObjectValues.Where(x => contentItemIds.Contains(x.ObjectId)).LoadAsync();
        }

        return result;
    }

    public Task RemoveFoldersAsync(string[] ids)
    {
        return GenericMassRemove<DynamicContentFolderEntity>(ids);
    }

    public Task RemoveFoldersAsync(DynamicContentFolderEntity[] folders)
    {
        DbContext.RemoveRange(folders);

        return Task.CompletedTask;
    }

    public Task RemoveContentPublicationsAsync(string[] ids)
    {
        return GenericMassRemove<DynamicContentPublishingGroupEntity>(ids);
    }

    public Task RemovePlacesAsync(string[] ids)
    {
        return GenericMassRemove<DynamicContentPlaceEntity>(ids);
    }

    public async Task RemoveContentItemsAsync(string[] ids)
    {
        // VP-1945
        // GenericMassRemove does not delete DynamicContentItemDynamicPropertyObjectValueEntity DESPITE CascadeDelete specified, as DynamicPropertyValues are not loaded by EF in memory.
        // https://docs.microsoft.com/en-us/ef/core/saving/cascade-delete#delete-behaviors See comment. So need to delete loaded entities, not just parent ones attached by Ids.
        // Also, we do not specify ON CASCADE DELETE for the foreign key on database level in migration intentionally (onDelete: ReferentialAction.Restrict),
        // because it is self referencing table, which cannot have cascade delete FK's in MSSQL.

        var items = await GetContentItemsByIdsAsync(ids);
        foreach (var item in items)
        {
            Remove(item);
        }
    }

    public virtual Task RemovePromotionsAsync(string[] ids)
    {
        return GenericMassRemove<PromotionEntity>(ids);
    }

    public async Task<IList<CouponEntity>> GetCouponsByIdsAsync(IList<string> ids)
    {
        var coupons = await Coupons
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        if (coupons.Count > 0)
        {
            var couponCodes = coupons.Select(x => x.Code).ToArray();

            var couponUsages = await PromotionUsages
                .Where(x => couponCodes.Contains(x.CouponCode))
                .GroupBy(x => x.CouponCode)
                .Select(x => new { CouponCode = x.Key, TotalUsesCount = x.Count() })
                .ToArrayAsync();

            foreach (var usage in couponUsages)
            {
                var coupon = coupons.FirstOrDefault(x => x.Code.EqualsIgnoreCase(usage.CouponCode));
                if (coupon != null)
                {
                    coupon.TotalUsesCount = usage.TotalUsesCount;
                }
            }
        }

        return coupons;
    }

    public Task RemoveCouponsAsync(string[] ids)
    {
        return GenericMassRemove<CouponEntity>(ids);
    }

    public virtual async Task<IList<PromotionUsageEntity>> GetMarketingUsagesByIdsAsync(IList<string> ids)
    {
        return await PromotionUsages.Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public Task RemoveMarketingUsagesAsync(string[] ids)
    {
        return GenericMassRemove<PromotionUsageEntity>(ids);
    }

    public async Task<string[]> CheckCouponsForUniquenessAsync(Coupon[] coupons)
    {
        var result = new List<string>();
        var couponKeysToAdd = coupons.Select(x => x.Code + x.PromotionId).DistinctIgnoreCase().ToArray();

        for (var skip = 0; skip < couponKeysToAdd.Length; skip += _defaultPageSize)
        {
            var couponKeysBatch = couponKeysToAdd.Skip(skip).Take(_defaultPageSize).ToArray();
            var errors = await Coupons.Where(x => couponKeysBatch.Contains(x.Code + x.PromotionId))
                .Include(x => x.Promotion)
                .Select(x => $"Coupon with Name: '{x.Code}' for Promotion: '{x.Promotion.Name}' already exists.")
                .ToArrayAsync();
            result.AddRange(errors);
        }

        return result.ToArray();
    }


    private Task GenericMassRemove<T>(IList<string> ids)
        where T : IEntity
    {
        foreach (var id in ids ?? [])
        {
            var entity = AbstractTypeFactory<T>.TryCreateInstance();
            entity.Id = id;
            _dbContext.Attach(entity);
            _dbContext.Remove(entity);
        }

        return Task.CompletedTask;
    }
}
