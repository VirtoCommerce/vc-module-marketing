using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Core.Services;

public interface ICouponService : ICrudService<Coupon>
{
    [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<Coupon[]> GetByIdsAsync(string[] ids);

    [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task SaveCouponsAsync(Coupon[] coupons);

    [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task DeleteCouponsAsync(string[] ids);
}
