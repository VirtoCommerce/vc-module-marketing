using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using VirtoCommerce.ExperienceApiModule.Core.BaseQueries;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;

namespace VirtoCommerce.MarketingModule.ExperienceApi.Queries
{
    public class EvaluateDynamicContentQuery : Query<EvaluateDynamicContentResult>
    {
        public string StoreId { get; set; }
        public string PlaceName { get; set; }
        public string CategoryId { get; set; }
        public string ProductId { get; set; }
        public string CultureName { get; set; }

        public DateTime ToDate { get; set; }

        public string[] Tags { get; set; }
        public string[] UserGroups { get; set; }

        public override IEnumerable<QueryArgument> GetArguments()
        {
            yield return Argument<StringGraphType>(nameof(StoreId));
            yield return Argument<StringGraphType>(nameof(PlaceName));
            yield return Argument<StringGraphType>(nameof(CategoryId));
            yield return Argument<StringGraphType>(nameof(ProductId));
            yield return Argument<StringGraphType>(nameof(CultureName));

            yield return Argument<DateGraphType>(nameof(ToDate));

            yield return Argument<ListGraphType<StringGraphType>>(nameof(Tags));
            yield return Argument<ListGraphType<StringGraphType>>(nameof(UserGroups));
        }

        public override void Map(IResolveFieldContext context)
        {
            StoreId = context.GetArgument<string>(nameof(StoreId));
            PlaceName = context.GetArgument<string>(nameof(PlaceName));
            CategoryId = context.GetArgument<string>(nameof(CategoryId));
            ProductId = context.GetArgument<string>(nameof(ProductId));
            CultureName = context.GetArgument<string>(nameof(CultureName));

            ToDate = context.GetArgument<DateTime>(nameof(ToDate));

            Tags = context.GetArgument<string[]>(nameof(Tags));
            UserGroups = context.GetArgument<string[]>(nameof(UserGroups));
        }
    }
}
