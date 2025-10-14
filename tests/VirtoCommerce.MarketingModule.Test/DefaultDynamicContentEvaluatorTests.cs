using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Conditions.CatalogConditions;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.JsonConverters;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test;

public class DefaultDynamicContentEvaluatorTests
{
    static DefaultDynamicContentEvaluatorTests()
    {
        AbstractTypeFactory<IConditionTree>.RegisterType<DynamicContentConditionTree>();

        foreach (var conditionTree in ((IConditionTree)AbstractTypeFactory<DynamicContentConditionTree>.TryCreateInstance()).Traverse(x => x.AvailableChildren))
        {
            AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
        }
    }

    [Theory]
    [MemberData(nameof(GetDynamicContentExpressionData))]
    public async Task EvaluateItemsAsync(DynamicContentEvaluationContext context, DynamicContentConditionTree expression, bool expectedResult)
    {
        // Arrange
        var dynamicContentItem = new DynamicContentItem { Id = Guid.NewGuid().ToString() };
        var dynamicContentItems = new[] { dynamicContentItem };

        var evaluator = GetEvaluator(dynamicContentItem, dynamicContentItems, expression);

        // Act
        var results = await evaluator.EvaluateItemsAsync(context);

        // Assert
        Assert.Equal(expectedResult, dynamicContentItems.Equals(results));
    }

    private static IMarketingDynamicContentEvaluator GetEvaluator(DynamicContentItem item, DynamicContentItem[] items, DynamicContentConditionTree expression)
    {
        var groups = new List<DynamicContentPublication>
        {
            new()
            {
                DynamicExpression = expression,
                IsActive = true,
                ContentItems = new ObservableCollection<DynamicContentItem> { item },
            },
        };

        var dynamicContentPublicationSearchServiceMock = new Mock<IDynamicContentPublicationSearchService>();
        dynamicContentPublicationSearchServiceMock
            .Setup(x => x.SearchAsync(It.IsAny<DynamicContentPublicationSearchCriteria>(), It.IsAny<bool>()))
            .ReturnsAsync(new DynamicContentPublicationSearchResult { Results = groups.ToArray() });

        var dynamicContentItemServiceMock = new Mock<IDynamicContentItemService>();
        dynamicContentItemServiceMock
            .Setup(x => x.GetAsync(new[] { item.Id }, It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(items);

        return new DefaultDynamicContentEvaluator(
            dynamicContentItemServiceMock.Object,
            dynamicContentPublicationSearchServiceMock.Object,
            Mock.Of<ILogger<DefaultDynamicContentEvaluator>>());
    }

    public static IEnumerable<object[]> GetDynamicContentExpressionData()
    {
        return new List<object[]>
        {
            new object[]
            {
                // Context
                new DynamicContentEvaluationContext { CategoryId = "Category_1" },
                // Expression
                GetExpressionTree(new DynamicContentConditionCategoryIs { CategoryId = "Category_1" }),
                // Expected result
                true,
            },
            new object[]
            {
                new DynamicContentEvaluationContext { CategoryId = "Category_1" },
                GetExpressionTree(new DynamicContentConditionCategoryIs { CategoryId = "Category_2" }),
                false,
            },
            new object[]
            {
                new DynamicContentEvaluationContext { ProductId = "ProductId_1" },
                GetExpressionTree(new DynamicContentConditionProductIs { ProductIds = ["ProductId_1"] }),
                true,
            },
            new object[]
            {
                new DynamicContentEvaluationContext { ProductId = "ProductId_1" },
                GetExpressionTree(new DynamicContentConditionProductIs { ProductIds = ["ProductId_1", "Product_2"] }),
                true,
            },
            new object[]
            {
                new DynamicContentEvaluationContext { ProductId = "ProductId_2" },
                GetExpressionTree(new DynamicContentConditionProductIs { ProductIds = ["ProductId_1"] }),
                false,
            },
            new object[]
            {
                new DynamicContentEvaluationContext { GeoCity = "NY" },
                GetGeoPointCondition(),
                true,
            },
        };
    }

    private static DynamicContentConditionTree GetExpressionTree(ConditionTree condition)
    {
        var blockCondition = new BlockContentCondition()
            .WithChildren(condition);

        var expression = new DynamicContentConditionTree();
        expression.WithChildren(blockCondition);

        return expression;
    }

    private static DynamicContentConditionTree GetGeoPointCondition()
    {
        const string json = """{"AvailableChildren":null,"Children":[{"All":false,"Not":false,"AvailableChildren":null,"Children":[{"Value":"NY","MatchCondition":"Contains","AvailableChildren":null,"Children":[],"Id":"ConditionGeoCity"}],"Id":"BlockContentCondition"}],"Id":"DynamicContentConditionTree"}""";

        return JsonConvert.DeserializeObject<DynamicContentConditionTree>(
            json,
            new ConditionJsonConverter(),
            new PolymorphJsonConverter());
    }
}
