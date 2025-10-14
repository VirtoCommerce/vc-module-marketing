using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model;

namespace VirtoCommerce.MarketingModule.Core.Services;

[Obsolete("Use IDynamicContentFolderService, IDynamicContentItemService, IDynamicContentPlaceService, IDynamicContentPublicationService", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public interface IDynamicContentService
{
    [Obsolete("Use IDynamicContentFolderService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentFolder[]> GetFoldersByIdsAsync(string[] ids);

    [Obsolete("Use IDynamicContentFolderService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task SaveFoldersAsync(DynamicContentFolder[] folders);

    [Obsolete("Use IDynamicContentFolderService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task DeleteFoldersAsync(string[] ids);

    [Obsolete("Use IDynamicContentItemService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentItem[]> GetContentItemsByIdsAsync(string[] ids);

    [Obsolete("Use IDynamicContentItemService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task SaveContentItemsAsync(DynamicContentItem[] items);

    [Obsolete("Use IDynamicContentItemService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task DeleteContentItemsAsync(string[] ids);

    [Obsolete("Use IDynamicContentPlaceService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentPlace[]> GetPlacesByIdsAsync(string[] ids);

    [Obsolete("Use IDynamicContentPlaceService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task SavePlacesAsync(DynamicContentPlace[] places);

    [Obsolete("Use IDynamicContentPlaceService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task DeletePlacesAsync(string[] ids);

    [Obsolete("Use IDynamicContentPublicationService.GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<DynamicContentPublication[]> GetPublicationsByIdsAsync(string[] ids);

    [Obsolete("Use IDynamicContentPublicationService.SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task SavePublicationsAsync(DynamicContentPublication[] publications);

    [Obsolete("Use IDynamicContentPublicationService.DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task DeletePublicationsAsync(string[] ids);
}
