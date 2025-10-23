using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;

namespace VirtoCommerce.MarketingModule.Core.Search;

[Obsolete("Use IDynamicContentFolderSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public interface IFolderSearchService : IDynamicContentFolderSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentFolderSearchResult> SearchFoldersAsync(DynamicContentFolderSearchCriteria criteria);
}
