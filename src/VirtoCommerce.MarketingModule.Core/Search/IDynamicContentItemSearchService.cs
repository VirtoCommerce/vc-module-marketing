using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Core.Search;

public interface IDynamicContentItemSearchService : ISearchService<DynamicContentItemSearchCriteria, DynamicContentItemSearchResult, DynamicContentItem>;
