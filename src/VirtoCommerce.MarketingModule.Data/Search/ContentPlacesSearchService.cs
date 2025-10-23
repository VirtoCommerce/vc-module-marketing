using System;
using System.Collections.Generic;
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
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Search;

[Obsolete("Use DynamicContentPlaceSearchService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class ContentPlacesSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentPlaceService crudService,
    IOptions<CrudOptions> crudOptions)
    : DynamicContentPlaceSearchService(repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IContentPlacesSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task<DynamicContentPlaceSearchResult> SearchContentPlacesAsync(DynamicContentPlaceSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }

    [Obsolete("Use BuildSortExpression()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IList<SortInfo> BuildSearchExpression(DynamicContentPlaceSearchCriteria criteria)
    {
        return BuildSortExpression(criteria);
    }

    [Obsolete("Use BuildQuery(IRepository repository, DynamicContentPlaceSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<DynamicContentPlaceEntity> BuildQuery(DynamicContentPlaceSearchCriteria criteria, IMarketingRepository repository)
    {
        return BuildQuery(repository, criteria);
    }
}
