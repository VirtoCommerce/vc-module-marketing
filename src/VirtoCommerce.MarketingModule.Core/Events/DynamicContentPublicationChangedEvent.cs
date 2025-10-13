using System.Collections.Generic;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Core.Events;

public class DynamicContentPublicationChangedEvent(IEnumerable<GenericChangedEntry<DynamicContentPublication>> changedEntries)
    : GenericChangedEntryEvent<DynamicContentPublication>(changedEntries);
