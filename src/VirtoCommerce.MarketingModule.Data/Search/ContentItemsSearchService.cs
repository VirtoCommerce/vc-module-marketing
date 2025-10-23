using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Search;

[Obsolete("Use DynamicContentItemSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class ContentItemsSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentItemService crudService,
    IOptions<CrudOptions> crudOptions)
    : DynamicContentItemSearchService(repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IContentItemsSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task<DynamicContentItemSearchResult> SearchContentItemsAsync(DynamicContentItemSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }

    [Obsolete("Use BuildQuery(IRepository repository, DynamicContentItemSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<DynamicContentItemEntity> BuildQuery(DynamicContentItemSearchCriteria criteria, IMarketingRepository repository)
    {
        return BuildQuery(repository, criteria);
    }
}
