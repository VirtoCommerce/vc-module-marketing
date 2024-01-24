using System.Linq;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CoreModule.Core.Conditions.Browse;
using VirtoCommerce.CoreModule.Core.Conditions.GeoConditions;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Conditions.CatalogConditions;

namespace VirtoCommerce.MarketingModule.Core.Model.DynamicContent
{
    public class DynamicContentConditionTreePrototype : ConditionTree
    {
        public DynamicContentConditionTreePrototype()
        {
            WithAvailConditions(
              new BlockContentCondition()
                  .WithAvailConditions(
                     new ConditionGeoTimeZone(),
                     new ConditionGeoZipCode(),
                     new ConditionStoreSearchedPhrase(),
                     new ConditionAgeIs(),
                     new ConditionGenderIs(),
                     new ConditionGeoCity(),
                     new ConditionGeoCountry(),
                     new ConditionGeoState(),
                     new ConditionLanguageIs(),
                     new UserGroupsContainsCondition(),
                     new UserGroupIsCondition(),
                     new DynamicContentConditionCategoryIs(),
                     new DynamicContentConditionProductIs()
                   )
             );
            Children = AvailableChildren.ToList();
        }
    }
}
