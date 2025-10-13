using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;

namespace VirtoCommerce.MarketingModule.Core.Search;

[Obsolete("Use IDynamicContentPlaceSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public interface IContentPlacesSearchService : IDynamicContentPlaceSearchService
{
    [Obsolete("Use IDynamicContentPlaceSearchService.SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentPlaceSearchResult> SearchContentPlacesAsync(DynamicContentPlaceSearchCriteria criteria);
}
