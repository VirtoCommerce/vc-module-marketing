using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
    public class MarketingRepository : DbContextRepositoryBase<MarketingDbContext>, IMarketingRepository
    {
        private readonly MarketingDbContext _dbContext;
        private const int DefaultPageSize = 50;
        public MarketingRepository(MarketingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #region IMarketingRepository Members

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

        public virtual async Task<PromotionEntity[]> GetPromotionsByIdsAsync(string[] ids)
        {
            var propmotions = await Promotions.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            await PromotionStores.Where(x => ids.Contains(x.PromotionId)).LoadAsync();
            var promotionsIdsWithCoupons = await Coupons.Where(x => ids.Contains(x.PromotionId)).Select(x => x.PromotionId).Distinct().ToArrayAsync();
            foreach (var promotion in propmotions)
            {
                promotion.HasCoupons = promotionsIdsWithCoupons.Contains(promotion.Id);
            }
            return propmotions;
        }

        public async Task<DynamicContentFolderEntity[]> GetContentFoldersByIdsAsync(string[] ids)
        {
            var retVal = await Folders.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            if (retVal.Any())
            {
                var allParentFoldersIds = retVal.Where(x => x.ParentFolderId != null).Select(x => x.ParentFolderId).ToArray();
                var allParentFolders = await GetContentFoldersByIdsAsync(allParentFoldersIds);
                foreach (var folder in retVal.Where(x => x.ParentFolderId != null))
                {
                    folder.ParentFolder = allParentFolders.FirstOrDefault(x => x.Id == folder.ParentFolderId);
                }
            }
            return retVal;
        }

        public async Task<DynamicContentItemEntity[]> GetContentItemsByIdsAsync(string[] ids)
        {
            var retVal = await Items.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            if (retVal != null)
            {
                var allFoldersIds = retVal.Where(x => x.FolderId != null).Select(x => x.FolderId).Distinct().ToArray();
                var allFolders = await GetContentFoldersByIdsAsync(allFoldersIds);
                foreach (var item in retVal.Where(x => x.FolderId != null))
                {
                    item.Folder = allFolders.FirstOrDefault(x => x.Id == item.FolderId);
                }

                await DynamicContentItemDynamicPropertyObjectValues.Where(x => ids.Contains(x.ObjectId)).LoadAsync();
            }
            return retVal;
        }

        public async Task<DynamicContentPlaceEntity[]> GetContentPlacesByIdsAsync(string[] ids)
        {
            var retVal = await Places.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            if (retVal != null)
            {
                var allFoldersIds = retVal.Where(x => x.FolderId != null).Select(x => x.FolderId).Distinct().ToArray();
                var allFolders = await GetContentFoldersByIdsAsync(allFoldersIds);
                foreach (var place in retVal.Where(x => x.FolderId != null))
                {
                    place.Folder = allFolders.FirstOrDefault(x => x.Id == place.FolderId);
                }
            }
            return retVal;
        }

        public async Task<DynamicContentPublishingGroupEntity[]> GetContentPublicationsByIdsAsync(string[] ids)
        {
            var result = await PublishingGroups.Where(i => ids.Contains(i.Id))
                .Include(x => x.ContentItems).ThenInclude(y => y.ContentItem)
                .Include(x => x.ContentPlaces).ThenInclude(y => y.ContentPlace)
                                    .ToArrayAsync();

            var contentItemIds = result.SelectMany(x => x.ContentItems).Select(x => x.DynamicContentItemId).Distinct().ToArray();

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

        public async Task<CouponEntity[]> GetCouponsByIdsAsync(string[] ids)
        {
            var retVal = await Coupons.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            var couponCodes = retVal.Select(x => x.Code).ToArray();
            var couponUsagesTotals = await PromotionUsages.Where(x => couponCodes.Contains(x.CouponCode)).GroupBy(x => x.CouponCode)
                   .Select(x => new { CouponCode = x.Key, TotalUsesCount = x.Count() }).ToArrayAsync();
            foreach (var totalsUses in couponUsagesTotals)
            {
                var coupon = retVal.FirstOrDefault(x => x.Code.EqualsInvariant(totalsUses.CouponCode));
                if (coupon != null)
                {
                    coupon.TotalUsesCount = totalsUses.TotalUsesCount;
                }
            }
            return retVal;
        }

        public Task RemoveCouponsAsync(string[] ids)
        {
            return GenericMassRemove<CouponEntity>(ids);
        }

        public Task<PromotionUsageEntity[]> GetMarketingUsagesByIdsAsync(string[] ids)
        {
            return PromotionUsages.Where(x => ids.Contains(x.Id)).ToArrayAsync();
        }

        public Task RemoveMarketingUsagesAsync(string[] ids)
        {
            return GenericMassRemove<PromotionUsageEntity>(ids);
        }
        #endregion

        public async Task<string[]> CheckCouponsForUniquenessAsync(Coupon[] coupons)
        {
            var result = new List<string>();
            var couponKeysToAdd = coupons.Select(x => x.Code + x.PromotionId).Distinct().ToArray();

            for (var skip = 0; skip < couponKeysToAdd.Length; skip += DefaultPageSize)
            {
                var couponKeysBatch = couponKeysToAdd.Skip(skip).Take(DefaultPageSize).ToArray();
                var errors = await Coupons.Where(x => couponKeysBatch.Contains(x.Code + x.PromotionId))
                    .Include(x => x.Promotion)
                    .Select(x => $"Coupon with Name: '{x.Code}' for Promotion: '{x.Promotion.Name}' already exists.")
                    .ToArrayAsync();
                result.AddRange(errors);
            }

            return result.ToArray();
        }

        private Task GenericMassRemove<T>(string[] ids) where T : IEntity
        {

            foreach (var id in ids ?? Array.Empty<string>())
            {
                var toRemove = AbstractTypeFactory<T>.TryCreateInstance();
                toRemove.Id = id;
                _dbContext.Attach(toRemove);
                _dbContext.Remove(toRemove);
            }
            return Task.CompletedTask;
        }

    }
}
