using System.Collections.Generic;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Core.Events;

public class PromotionChangingEvent(IEnumerable<GenericChangedEntry<Promotion>> changedEntries)
    : GenericChangedEntryEvent<Promotion>(changedEntries);
