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

public class DynamicContentPublicationService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<DynamicContentPublication, DynamicContentPublishingGroupEntity, DynamicContentPublicationChangingEvent, DynamicContentPublicationChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        IDynamicContentPublicationService
{
    protected override Task<IList<DynamicContentPublishingGroupEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetContentPublicationsByIdsAsync(ids);
    }
}
