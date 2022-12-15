using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.Schemas;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;

namespace VirtoCommerce.MarketingModule.ExperienceApi.Schemas
{
    public class EvaluateDynamicContentResultType : ExtendableGraphType<EvaluateDynamicContentResult>
    {
        public EvaluateDynamicContentResultType()
        {
            Field(x => x.TotalCount);

            ExtendableField<ListGraphType<DynamicContentItemType>>(nameof(EvaluateDynamicContentResult.Items), resolve: context => context.Source.Items);
        }
    }
}
