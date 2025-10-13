using System.Collections.Generic;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Core.Events;

public class PromotionUsageChangedEvent(IEnumerable<GenericChangedEntry<PromotionUsage>> changedEntries)
    : GenericChangedEntryEvent<PromotionUsage>(changedEntries);
