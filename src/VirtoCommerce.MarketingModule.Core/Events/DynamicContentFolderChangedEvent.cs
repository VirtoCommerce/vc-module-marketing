using System.Collections.Generic;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.MarketingModule.Core.Events;

public class DynamicContentFolderChangedEvent(IEnumerable<GenericChangedEntry<DynamicContentFolder>> changedEntries)
    : GenericChangedEntryEvent<DynamicContentFolder>(changedEntries);
