using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;

namespace VirtoCommerce.MarketingModule.Core.Search;

[Obsolete("Use IDynamicContentPublicationSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public interface IContentPublicationsSearchService : IDynamicContentPublicationSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentPublicationSearchResult> SearchContentPublicationsAsync(DynamicContentPublicationSearchCriteria criteria);
}
