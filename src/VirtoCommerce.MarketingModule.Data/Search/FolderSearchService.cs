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

[Obsolete("Use DynamicContentFolderSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class FolderSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentFolderService crudService,
    IOptions<CrudOptions> crudOptions)
    : DynamicContentFolderSearchService(repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IFolderSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task<DynamicContentFolderSearchResult> SearchFoldersAsync(DynamicContentFolderSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }

    [Obsolete("Use BuildQuery(IRepository repository, DynamicContentFolderSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<DynamicContentFolderEntity> BuildQuery(DynamicContentFolderSearchCriteria criteria, IMarketingRepository repository)
    {
        return BuildQuery(repository, criteria);
    }
}
