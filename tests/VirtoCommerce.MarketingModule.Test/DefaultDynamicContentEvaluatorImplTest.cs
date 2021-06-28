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
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.JsonConverters;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    public class DefaultDynamicContentEvaluatorImplTest
    {
        private readonly Mock<IMarketingRepository> _repositoryMock;
        private readonly Mock<IContentPublicationsSearchService> _dynamicContentSearchServiceMock;
        private readonly Mock<IDynamicContentService> _dynamicContentServiceMock;
        private readonly Mock<ILogger<DefaultDynamicContentEvaluator>> _loggerMock;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        static DefaultDynamicContentEvaluatorImplTest()
        {
            AbstractTypeFactory<IConditionTree>.RegisterType<DynamicContentConditionTree>();
            foreach (var conditionTree in ((IConditionTree)AbstractTypeFactory<DynamicContentConditionTree>.TryCreateInstance()).Traverse(x => x.AvailableChildren))
            {
                AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
            }
        }

        public DefaultDynamicContentEvaluatorImplTest()
        {
            _repositoryMock = new Mock<IMarketingRepository>();

            _dynamicContentSearchServiceMock = new Mock<IContentPublicationsSearchService>();
            _dynamicContentServiceMock = new Mock<IDynamicContentService>();
            _loggerMock = new Mock<ILogger<DefaultDynamicContentEvaluator>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _repositoryMock.Setup(ss => ss.UnitOfWork).Returns(_mockUnitOfWork.Object);
        }

        [Theory]
        [MemberData(nameof(GetDynamicContentExpressionData))]
        public async Task EvaluateItemsAsync(DynamicContentEvaluationContext context, DynamicContentConditionTree exression, bool expectedResult)
        {
            // Arrange
            var dynamicContentItem = new DynamicContentItem { Id = Guid.NewGuid().ToString() };
            var dynamicContentItems = new DynamicContentItem[] { dynamicContentItem };

            var evaluator = GetEvaluator(dynamicContentItem, dynamicContentItems, exression);

            // Act
            var results = await evaluator.EvaluateItemsAsync(context);

            // Assert
            Assert.Equal(expectedResult, dynamicContentItems.Equals(results));
        }

        private IMarketingDynamicContentEvaluator GetEvaluator(DynamicContentItem item, DynamicContentItem[] items, DynamicContentConditionTree expression)
        {
            var groups = new List<DynamicContentPublication>
            {
                new DynamicContentPublication
                {
                    DynamicExpression = expression,
                    IsActive = true,
                    ContentItems = new ObservableCollection<DynamicContentItem> { item },
                }
            };

            _dynamicContentSearchServiceMock.Setup(dcs => dcs.SearchContentPublicationsAsync(It.IsAny<DynamicContentPublicationSearchCriteria>()))
                .ReturnsAsync(new Core.Model.DynamicContent.Search.DynamicContentPublicationSearchResult { Results = groups.ToArray() });
            _dynamicContentServiceMock.Setup(dcs => dcs.GetContentItemsByIdsAsync(new[] { item.Id }))
                .ReturnsAsync(items);

            return new DefaultDynamicContentEvaluator(_dynamicContentSearchServiceMock.Object, _dynamicContentServiceMock.Object, _loggerMock.Object);
        }

        public static IEnumerable<object[]> GetDynamicContentExpressionData()
        {
            return new List<object[]>
            {
                new object[]
                {
                    // context
                    new DynamicContentEvaluationContext { CategoryId = "Category_1" },
                    // expression
                    GetExpressionTree(new DynamicContentConditionCategoryIs() { CategoryId = "Category_1" }),
                    // expected result
                    true
                },
                new object[]
                {
                    new DynamicContentEvaluationContext { CategoryId = "Category_1" },
                    GetExpressionTree(new DynamicContentConditionCategoryIs() { CategoryId = "Category_2" }),
                    false
                },
                new object[]
                {
                    new DynamicContentEvaluationContext { ProductId = "ProductId_1" },
                    GetExpressionTree(new DynamicContentConditionProductIs() { ProductIds = new string[] { "ProductId_1" } }),
                    true
                },
                new object[]
                {
                    new DynamicContentEvaluationContext { ProductId = "ProductId_1" },
                    GetExpressionTree(new DynamicContentConditionProductIs() { ProductIds = new string[] { "ProductId_1", "Product_2" } }),
                    true
                },
                new object[]
                {
                    new DynamicContentEvaluationContext { ProductId = "ProductId_2" },
                    GetExpressionTree(new DynamicContentConditionProductIs() { ProductIds = new string[] { "ProductId_1" } }),
                    false
                },
                new object[]
                {
                    new DynamicContentEvaluationContext { GeoCity = "NY" },
                    GetGeoPointCondition(),
                    true
                },
            };
        }

        private static DynamicContentConditionTree GetExpressionTree(params ConditionTree[] condition)
        {
            var blockCondition = new BlockContentCondition()
                .WithChildrens(condition);

            var expression = new DynamicContentConditionTree();
            expression.WithChildrens(blockCondition);

            return expression;
        }

        private static DynamicContentConditionTree GetGeoPointCondition()
        {
            var json = @"{""AvailableChildren"":null,""Children"":[{""All"":false,""Not"":false,""AvailableChildren"":null,""Children"":[{""Value"":""NY"",""MatchCondition"":""Contains"",""AvailableChildren"":null,""Children"":[],""Id"":""ConditionGeoCity""}],""Id"":""BlockContentCondition""}],""Id"":""DynamicContentConditionTree""}";

            return JsonConvert.DeserializeObject<DynamicContentConditionTree>(
                json,
                new ConditionJsonConverter(),
                new PolymorphJsonConverter());
        }
    }
}
