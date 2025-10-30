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

[Obsolete("Use DynamicContentPublicationSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class ContentPublicationsSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentPublicationService crudService,
    IOptions<CrudOptions> crudOptions)
    : DynamicContentPublicationSearchService(repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IContentPublicationsSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task<DynamicContentPublicationSearchResult> SearchContentPublicationsAsync(DynamicContentPublicationSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }

    [Obsolete("Use BuildQuery(IRepository repository, DynamicContentPublicationSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<DynamicContentPublishingGroupEntity> BuildQuery(DynamicContentPublicationSearchCriteria criteria, IMarketingRepository repository)
    {
        return BuildQuery(repository, criteria);
    }
}
