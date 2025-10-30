using System.Collections.Generic;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Core.Events;

public class DynamicContentPlaceChangingEvent(IEnumerable<GenericChangedEntry<DynamicContentPlace>> changedEntries)
    : GenericChangedEntryEvent<DynamicContentPlace>(changedEntries);
