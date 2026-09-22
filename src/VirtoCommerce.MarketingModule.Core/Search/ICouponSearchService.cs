using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Core.Search;

public interface ICouponSearchService : ISearchService<CouponSearchCriteria, CouponSearchResult, Coupon>
{
}
