using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Core.Services
{
    public interface IPromotionService : ICrudService<Promotion>
    {
        [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        Task<Promotion[]> GetPromotionsByIdsAsync(string[] ids);

        [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        Task SavePromotionsAsync(Promotion[] promotions);

        [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        Task DeletePromotionsAsync(string[] ids);
    }
}
