using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.MarketingModule.Data.ExportImport;

public class MarketingExportImport(
    JsonSerializer jsonSerializer,
    ICouponService couponService,
    ICouponSearchService couponSearchService,
    IPromotionService promotionService,
    IPromotionSearchService promotionSearchService,
    IPromotionUsageService promotionUsageService,
    IPromotionUsageSearchService promotionUsageSearchService,
    IDynamicContentFolderService dynamicContentFolderService,
    IDynamicContentFolderSearchService dynamicContentFolderSearchService,
    IDynamicContentItemService dynamicContentItemService,
    IDynamicContentItemSearchService dynamicContentItemSearchService,
    IDynamicContentPlaceService dynamicContentPlaceService,
    IDynamicContentPlaceSearchService dynamicContentPlaceSearchService,
    IDynamicContentPublicationService dynamicContentPublicationService,
    IDynamicContentPublicationSearchService dynamicContentPublicationSearchService)
{
    private const int _batchSize = 50;

    public virtual async Task DoExportAsync(
        Stream outStream, ExportImportOptions options,
        Action<ExportImportProgressInfo> progressCallback,
        ICancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var progressInfo = new ExportImportProgressInfo { Description = "loading data..." };
        progressCallback(progressInfo);

        await using var sw = new StreamWriter(outStream);
        await using var writer = new JsonTextWriter(sw);
        await writer.WriteStartObjectAsync();

        progressInfo.Description = "Promotions exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("Promotions");

        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (skip, take) =>
            (GenericSearchResult<Promotion>)await LoadPromotionsPageAsync(skip, take, options, progressCallback),
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} promotions have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        progressInfo.Description = "Dynamic content folders exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("DynamicContentFolders");
        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (_, _) =>
            {
                var searchResult = AbstractTypeFactory<DynamicContentFolderSearchResult>.TryCreateInstance();
                var result = await LoadFoldersRecursiveAsync();
                searchResult.Results = result;
                searchResult.TotalCount = result.Count;
                return (GenericSearchResult<DynamicContentFolder>)searchResult;
            },
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} dynamic content folders have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        progressInfo.Description = "Dynamic content items exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("DynamicContentItems");
        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchCriteria = AbstractTypeFactory<DynamicContentItemSearchCriteria>.TryCreateInstance();
                searchCriteria.Skip = skip;
                searchCriteria.Take = take;
                return (GenericSearchResult<DynamicContentItem>)await dynamicContentItemSearchService.SearchAsync(searchCriteria);
            },
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} dynamic content items have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        progressInfo.Description = "Dynamic content places exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("DynamicContentPlaces");
        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchCriteria = AbstractTypeFactory<DynamicContentPlaceSearchCriteria>.TryCreateInstance();
                searchCriteria.Skip = skip;
                searchCriteria.Take = take;
                return (GenericSearchResult<DynamicContentPlace>)await dynamicContentPlaceSearchService.SearchAsync(searchCriteria);
            },
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} dynamic content places have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        progressInfo.Description = "Dynamic content publications exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("DynamicContentPublications");
        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchCriteria = AbstractTypeFactory<DynamicContentPublicationSearchCriteria>.TryCreateInstance();
                searchCriteria.Skip = skip;
                searchCriteria.Take = take;
                var searchResult = await dynamicContentPublicationSearchService.SearchAsync(searchCriteria);
                return (GenericSearchResult<DynamicContentPublication>)searchResult;
            },
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} dynamic content publications have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        progressInfo.Description = "Coupons exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("Coupons");
        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchCriteria = AbstractTypeFactory<CouponSearchCriteria>.TryCreateInstance();
                searchCriteria.Skip = skip;
                searchCriteria.Take = take;
                return (GenericSearchResult<Coupon>)await couponSearchService.SearchAsync(searchCriteria);
            },
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} coupons have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        progressInfo.Description = "Usages exporting...";
        progressCallback(progressInfo);

        await writer.WritePropertyNameAsync("Usages");
        await writer.SerializeArrayWithPagingAsync(jsonSerializer, _batchSize, async (skip, take) =>
            {
                var searchCriteria = AbstractTypeFactory<PromotionUsageSearchCriteria>.TryCreateInstance();
                searchCriteria.Skip = skip;
                searchCriteria.Take = take;
                return (GenericSearchResult<PromotionUsage>)await promotionUsageSearchService.SearchAsync(searchCriteria);
            },
            (processedCount, totalCount) =>
            {
                progressInfo.Description = $"{processedCount} of {totalCount} usages have been exported";
                progressCallback(progressInfo);
            },
            cancellationToken);

        await writer.WriteEndObjectAsync();
        await writer.FlushAsync();
    }

    public virtual async Task DoImportAsync(
        Stream inputStream,
        ExportImportOptions options,
        Action<ExportImportProgressInfo> progressCallback,
        ICancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var progressInfo = new ExportImportProgressInfo();

        using var streamReader = new StreamReader(inputStream);
        await using var reader = new JsonTextReader(streamReader);

        while (await reader.ReadAsync())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                var readerValueString = reader.Value?.ToString();

                if (readerValueString == "Promotions")
                {
                    await reader.DeserializeArrayWithPagingAsync<Promotion>(jsonSerializer, _batchSize, items => SavePromotionsAsync(items, options, progressCallback), processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} promotions have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);

                }
                else if (readerValueString == "DynamicContentFolders")
                {
                    await reader.DeserializeArrayWithPagingAsync<DynamicContentFolder>(jsonSerializer, _batchSize, dynamicContentFolderService.SaveChangesAsync, processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} dynamic content items have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);
                }
                else if (readerValueString == "DynamicContentItems")
                {
                    await reader.DeserializeArrayWithPagingAsync<DynamicContentItem>(jsonSerializer, _batchSize, dynamicContentItemService.SaveChangesAsync, processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} dynamic content items have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);
                }
                else if (readerValueString == "DynamicContentPlaces")
                {
                    await reader.DeserializeArrayWithPagingAsync<DynamicContentPlace>(jsonSerializer, _batchSize, dynamicContentPlaceService.SaveChangesAsync, processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} dynamic content places have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);
                }
                else if (readerValueString == "DynamicContentPublications")
                {
                    await reader.DeserializeArrayWithPagingAsync<DynamicContentPublication>(jsonSerializer, _batchSize, dynamicContentPublicationService.SaveChangesAsync, processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} dynamic content publications have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);
                }
                else if (readerValueString == "Coupons")
                {
                    await reader.DeserializeArrayWithPagingAsync<Coupon>(jsonSerializer, _batchSize, couponService.SaveChangesAsync, processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} coupons have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);
                }
                else if (readerValueString == "Usages")
                {
                    await reader.DeserializeArrayWithPagingAsync<PromotionUsage>(jsonSerializer, _batchSize, promotionUsageService.SaveChangesAsync, processedCount =>
                    {
                        progressInfo.Description = $"{processedCount} usages have been imported";
                        progressCallback(progressInfo);
                    }, cancellationToken);
                }
            }
        }
    }

    protected virtual Task<PromotionSearchResult> LoadPromotionsPageAsync(int skip, int take, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback)
    {
        var searchCriteria = AbstractTypeFactory<PromotionSearchCriteria>.TryCreateInstance();
        searchCriteria.Skip = skip;
        searchCriteria.Take = take;

        return promotionSearchService.SearchAsync(searchCriteria);
    }

    protected virtual Task SavePromotionsAsync(IList<Promotion> promotions, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback)
    {
        return promotionService.SaveChangesAsync(promotions);
    }

    private async Task<IList<DynamicContentFolder>> LoadFoldersRecursiveAsync(DynamicContentFolder folder = null)
    {
        var result = new List<DynamicContentFolder>();

        var searchCriteria = AbstractTypeFactory<DynamicContentFolderSearchCriteria>.TryCreateInstance();
        searchCriteria.FolderId = folder?.Id;

        var childFolders = await dynamicContentFolderSearchService.SearchAllAsync(searchCriteria);

        foreach (var childFolder in childFolders)
        {
            result.Add(childFolder);
            result.AddRange(await LoadFoldersRecursiveAsync(childFolder));
        }

        return result;
    }
}
