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
            IConditionTree[] children =
            [
                new BlockContentCondition()
                    .WithAvailableChildren(
                        new ConditionGeoTimeZone(),
                        new ConditionGeoZipCode(),
                        new ConditionStoreSearchedPhrase(),
                        new ConditionAgeIs(),
                        new ConditionGenderIs(),
                        new ConditionGeoCity(),
                        new ConditionGeoCountry(),
                        new ConditionGeoState(),
                        new ConditionLanguageIs(),
                        new UserGroupIsCondition(),
                        new UserGroupsContainsCondition(),
                        new DynamicContentConditionCategoryIs(),
                        new DynamicContentConditionProductIs()
                    ),
            ];

            WithChildren(children);
            WithAvailableChildren(children);
        }
    }
}
