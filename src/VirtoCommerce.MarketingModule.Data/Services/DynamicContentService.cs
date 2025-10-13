using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Services;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class DynamicContentService(
    IDynamicContentFolderService dynamicContentFolderService,
    IDynamicContentItemService dynamicContentItemService,
    IDynamicContentPlaceService dynamicContentPlaceService,
    IDynamicContentPublicationService dynamicContentPublicationService)
    : IDynamicContentService
{
    #region DynamicContentItem methods

    [Obsolete("Use IDynamicContentItemService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task<DynamicContentItem[]> GetContentItemsByIdsAsync(string[] ids)
    {
        return (await dynamicContentItemService.GetAsync(ids)).ToArray();
    }

    [Obsolete("Use IDynamicContentItemService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task SaveContentItemsAsync(DynamicContentItem[] items)
    {
        return dynamicContentItemService.SaveChangesAsync(items);
    }

    [Obsolete("Use IDynamicContentItemService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task DeleteContentItemsAsync(string[] ids)
    {
        return dynamicContentItemService.DeleteAsync(ids);
    }

    #endregion

    #region DynamicContentPlace methods

    [Obsolete("Use IDynamicContentPlaceService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task<DynamicContentPlace[]> GetPlacesByIdsAsync(string[] ids)
    {
        return (await dynamicContentPlaceService.GetAsync(ids)).ToArray();
    }

    [Obsolete("Use IDynamicContentPlaceService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task SavePlacesAsync(DynamicContentPlace[] places)
    {
        return dynamicContentPlaceService.SaveChangesAsync(places);
    }

    [Obsolete("Use IDynamicContentPlaceService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task DeletePlacesAsync(string[] ids)
    {
        return dynamicContentPlaceService.DeleteAsync(ids);
    }

    #endregion

    #region DynamicContentPublication methods

    [Obsolete("Use IDynamicContentPublicationService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task<DynamicContentPublication[]> GetPublicationsByIdsAsync(string[] ids)
    {
        return (await dynamicContentPublicationService.GetAsync(ids)).ToArray();
    }

    [Obsolete("Use IDynamicContentPublicationService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task SavePublicationsAsync(DynamicContentPublication[] publications)
    {
        return dynamicContentPublicationService.SaveChangesAsync(publications);
    }

    [Obsolete("Use IDynamicContentPublicationService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task DeletePublicationsAsync(string[] ids)
    {
        return dynamicContentPublicationService.DeleteAsync(ids);
    }

    #endregion

    #region DynamicContentFolder methods

    [Obsolete("Use IDynamicContentFolderService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task<DynamicContentFolder[]> GetFoldersByIdsAsync(string[] ids)
    {
        return (await dynamicContentFolderService.GetAsync(ids)).ToArray();
    }

    [Obsolete("Use IDynamicContentFolderService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task SaveFoldersAsync(DynamicContentFolder[] folders)
    {
        return dynamicContentFolderService.SaveChangesAsync(folders);
    }

    [Obsolete("Use IDynamicContentFolderService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task DeleteFoldersAsync(string[] ids)
    {
        return dynamicContentFolderService.DeleteAsync(ids);
    }

    #endregion
}
