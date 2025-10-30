using System.Collections.Generic;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Core.Events;

public class DynamicContentItemChangedEvent(IEnumerable<GenericChangedEntry<DynamicContentItem>> changedEntries)
    : GenericChangedEntryEvent<DynamicContentItem>(changedEntries);
