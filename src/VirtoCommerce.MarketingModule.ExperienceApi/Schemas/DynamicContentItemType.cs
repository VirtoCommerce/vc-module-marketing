using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Extensions;
using VirtoCommerce.ExperienceApiModule.Core.Helpers;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.ExperienceApiModule.Core.Services;
using VirtoCommerce.MarketingModule.Core.Model;

namespace VirtoCommerce.MarketingModule.ExperienceApi.Schemas
{
    public class DynamicContentItemType : ExtendableGraphType<DynamicContentItem>
    {
        public DynamicContentItemType(IDynamicPropertyResolverService dynamicPropertyResolverService)
        {
            Field(x => x.Id);
            Field(x => x.ContentType);
            Field(x => x.Name);
            Field(x => x.Description);
            Field(x => x.Priority);

            ExtendableField<ListGraphType<DynamicPropertyValueType>>(
            "dynamicProperties",
            "Dynamic content dynamic property values",
                QueryArgumentPresets.GetArgumentForDynamicProperties(),
                context => dynamicPropertyResolverService.LoadDynamicPropertyValues(context.Source, context.GetArgumentOrValue<string>("cultureName")));
        }
    }
}
