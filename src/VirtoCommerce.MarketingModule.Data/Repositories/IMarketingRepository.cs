using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
    public interface IMarketingRepository : IRepository
    {
        IQueryable<PromotionEntity> Promotions { get; }
        IQueryable<CouponEntity> Coupons { get; }
        IQueryable<PromotionUsageEntity> PromotionUsages { get; }
        IQueryable<DynamicContentFolderEntity> Folders { get; }
        IQueryable<DynamicContentItemEntity> Items { get; }
        IQueryable<DynamicContentPlaceEntity> Places { get; }
        IQueryable<DynamicContentPublishingGroupEntity> PublishingGroups { get; }
        IQueryable<PublishingGroupContentItemEntity> PublishingGroupContentItems { get; }
        IQueryable<PublishingGroupContentPlaceEntity> PublishingGroupContentPlaces { get; }
        IQueryable<PromotionStoreEntity> PromotionStores { get; }

        Task<IList<PromotionEntity>> GetPromotionsByIdsAsync(IList<string> ids);
        Task<IList<DynamicContentFolderEntity>> GetContentFoldersByIdsAsync(IList<string> ids);
        Task<IList<DynamicContentItemEntity>> GetContentItemsByIdsAsync(IList<string> ids);
        Task<IList<DynamicContentPlaceEntity>> GetContentPlacesByIdsAsync(IList<string> ids);
        Task<IList<DynamicContentPublishingGroupEntity>> GetContentPublicationsByIdsAsync(IList<string> ids);

        Task RemoveFoldersAsync(string[] ids);
        Task RemoveFoldersAsync(DynamicContentFolderEntity[] folders);
        Task RemoveContentPublicationsAsync(string[] ids);
        Task RemovePlacesAsync(string[] ids);
        Task RemoveContentItemsAsync(string[] ids);
        Task RemovePromotionsAsync(string[] ids);

        Task<IList<CouponEntity>> GetCouponsByIdsAsync(IList<string> ids);
        Task RemoveCouponsAsync(string[] ids);

        Task<IList<PromotionUsageEntity>> GetMarketingUsagesByIdsAsync(IList<string> ids);
        Task RemoveMarketingUsagesAsync(string[] ids);

        Task<string[]> CheckCouponsForUniquenessAsync(Coupon[] coupons);
    }
}
