using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class DynamicContentPlaceService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<DynamicContentPlace, DynamicContentPlaceEntity, DynamicContentPlaceChangingEvent, DynamicContentPlaceChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        IDynamicContentPlaceService
{
    protected override Task<IList<DynamicContentPlaceEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetContentPlacesByIdsAsync(ids);
    }
}
