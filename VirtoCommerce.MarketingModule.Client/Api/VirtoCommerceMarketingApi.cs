using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using VirtoCommerce.MarketingModule.Client.Client;
using VirtoCommerce.MarketingModule.Client.Model;

namespace VirtoCommerce.MarketingModule.Client.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IVirtoCommerceMarketingApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>DynamicContentItem</returns>
        DynamicContentItem MarketingModuleDynamicContentCreateDynamicContent(DynamicContentItem contentItem);

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of DynamicContentItem</returns>
        ApiResponse<DynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo(DynamicContentItem contentItem);
        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>DynamicContentFolder</returns>
        DynamicContentFolder MarketingModuleDynamicContentCreateDynamicContentFolder(DynamicContentFolder folder);

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>ApiResponse of DynamicContentFolder</returns>
        ApiResponse<DynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo(DynamicContentFolder folder);
        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>DynamicContentPlace</returns>
        DynamicContentPlace MarketingModuleDynamicContentCreateDynamicContentPlace(DynamicContentPlace contentPlace);

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of DynamicContentPlace</returns>
        ApiResponse<DynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo(DynamicContentPlace contentPlace);
        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>DynamicContentPublication</returns>
        DynamicContentPublication MarketingModuleDynamicContentCreateDynamicContentPublication(DynamicContentPublication publication);

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of DynamicContentPublication</returns>
        ApiResponse<DynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo(DynamicContentPublication publication);
        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentFolders(List<string> ids);

        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo(List<string> ids);
        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentPlaces(List<string> ids);

        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo(List<string> ids);
        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentPublications(List<string> ids);

        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo(List<string> ids);
        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContents(List<string> ids);

        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo(List<string> ids);
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;DynamicContentItem&gt;</returns>
        List<DynamicContentItem> MarketingModuleDynamicContentEvaluateDynamicContent(DynamicContentEvaluationContext evalContext);

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;DynamicContentItem&gt;</returns>
        ApiResponse<List<DynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo(DynamicContentEvaluationContext evalContext);
        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>DynamicContentItem</returns>
        DynamicContentItem MarketingModuleDynamicContentGetDynamicContentById(string id);

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>ApiResponse of DynamicContentItem</returns>
        ApiResponse<DynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo(string id);
        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>DynamicContentFolder</returns>
        DynamicContentFolder MarketingModuleDynamicContentGetDynamicContentFolderById(string id);

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>ApiResponse of DynamicContentFolder</returns>
        ApiResponse<DynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo(string id);
        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>DynamicContentPlace</returns>
        DynamicContentPlace MarketingModuleDynamicContentGetDynamicContentPlaceById(string id);

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>ApiResponse of DynamicContentPlace</returns>
        ApiResponse<DynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo(string id);
        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>DynamicContentPublication</returns>
        DynamicContentPublication MarketingModuleDynamicContentGetDynamicContentPublicationById(string id);

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>ApiResponse of DynamicContentPublication</returns>
        ApiResponse<DynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo(string id);
        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>DynamicContentPublication</returns>
        DynamicContentPublication MarketingModuleDynamicContentGetNewDynamicPublication();

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of DynamicContentPublication</returns>
        ApiResponse<DynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo();
        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContent(DynamicContentItem contentItem);

        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo(DynamicContentItem contentItem);
        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentFolder(DynamicContentFolder folder);

        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo(DynamicContentFolder folder);
        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentPlace(DynamicContentPlace contentPlace);

        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo(DynamicContentPlace contentPlace);
        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentPublication(DynamicContentPublication publication);

        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo(DynamicContentPublication publication);
        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Promotion</returns>
        Promotion MarketingModulePromotionCreatePromotion(Promotion promotion);

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>ApiResponse of Promotion</returns>
        ApiResponse<Promotion> MarketingModulePromotionCreatePromotionWithHttpInfo(Promotion promotion);
        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns></returns>
        void MarketingModulePromotionDeletePromotions(List<string> ids);

        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModulePromotionDeletePromotionsWithHttpInfo(List<string> ids);
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>List&lt;PromotionReward&gt;</returns>
        List<PromotionReward> MarketingModulePromotionEvaluatePromotions(PromotionEvaluationContext context);

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>ApiResponse of List&lt;PromotionReward&gt;</returns>
        ApiResponse<List<PromotionReward>> MarketingModulePromotionEvaluatePromotionsWithHttpInfo(PromotionEvaluationContext context);
        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Promotion</returns>
        Promotion MarketingModulePromotionGetNewDynamicPromotion();

        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Promotion</returns>
        ApiResponse<Promotion> MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo();
        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Promotion</returns>
        Promotion MarketingModulePromotionGetPromotionById(string id);

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>ApiResponse of Promotion</returns>
        ApiResponse<Promotion> MarketingModulePromotionGetPromotionByIdWithHttpInfo(string id);
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns></returns>
        void MarketingModulePromotionUpdatePromotions(Promotion promotion);

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModulePromotionUpdatePromotionsWithHttpInfo(Promotion promotion);
        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>MarketingSearchResult</returns>
        MarketingSearchResult MarketingModuleSearch(MarketingSearchCriteria criteria);

        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of MarketingSearchResult</returns>
        ApiResponse<MarketingSearchResult> MarketingModuleSearchWithHttpInfo(MarketingSearchCriteria criteria);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of DynamicContentItem</returns>
        System.Threading.Tasks.Task<DynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentAsync(DynamicContentItem contentItem);

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (DynamicContentItem)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentItem>> MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo(DynamicContentItem contentItem);
        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of DynamicContentFolder</returns>
        System.Threading.Tasks.Task<DynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderAsync(DynamicContentFolder folder);

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of ApiResponse (DynamicContentFolder)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentFolder>> MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo(DynamicContentFolder folder);
        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of DynamicContentPlace</returns>
        System.Threading.Tasks.Task<DynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceAsync(DynamicContentPlace contentPlace);

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (DynamicContentPlace)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentPlace>> MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo(DynamicContentPlace contentPlace);
        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of DynamicContentPublication</returns>
        System.Threading.Tasks.Task<DynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationAsync(DynamicContentPublication publication);

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (DynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentPublication>> MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo(DynamicContentPublication publication);
        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentFoldersAsync(List<string> ids);

        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPlacesAsync(List<string> ids);

        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsync(List<string> ids);

        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentsAsync(List<string> ids);

        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;DynamicContentItem&gt;</returns>
        System.Threading.Tasks.Task<List<DynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentAsync(DynamicContentEvaluationContext evalContext);

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;DynamicContentItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<DynamicContentItem>>> MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo(DynamicContentEvaluationContext evalContext);
        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of DynamicContentItem</returns>
        System.Threading.Tasks.Task<DynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdAsync(string id);

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of ApiResponse (DynamicContentItem)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentItem>> MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of DynamicContentFolder</returns>
        System.Threading.Tasks.Task<DynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsync(string id);

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of ApiResponse (DynamicContentFolder)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentFolder>> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of DynamicContentPlace</returns>
        System.Threading.Tasks.Task<DynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsync(string id);

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of ApiResponse (DynamicContentPlace)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentPlace>> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of DynamicContentPublication</returns>
        System.Threading.Tasks.Task<DynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsync(string id);

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of ApiResponse (DynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentPublication>> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of DynamicContentPublication</returns>
        System.Threading.Tasks.Task<DynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationAsync();

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (DynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<DynamicContentPublication>> MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo();
        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentAsync(DynamicContentItem contentItem);

        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo(DynamicContentItem contentItem);
        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentFolderAsync(DynamicContentFolder folder);

        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo(DynamicContentFolder folder);
        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPlaceAsync(DynamicContentPlace contentPlace);

        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo(DynamicContentPlace contentPlace);
        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPublicationAsync(DynamicContentPublication publication);

        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo(DynamicContentPublication publication);
        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of Promotion</returns>
        System.Threading.Tasks.Task<Promotion> MarketingModulePromotionCreatePromotionAsync(Promotion promotion);

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of ApiResponse (Promotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<Promotion>> MarketingModulePromotionCreatePromotionAsyncWithHttpInfo(Promotion promotion);
        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModulePromotionDeletePromotionsAsync(List<string> ids);

        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo(List<string> ids);
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of List&lt;PromotionReward&gt;</returns>
        System.Threading.Tasks.Task<List<PromotionReward>> MarketingModulePromotionEvaluatePromotionsAsync(PromotionEvaluationContext context);

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;PromotionReward&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<PromotionReward>>> MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo(PromotionEvaluationContext context);
        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of Promotion</returns>
        System.Threading.Tasks.Task<Promotion> MarketingModulePromotionGetNewDynamicPromotionAsync();

        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (Promotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<Promotion>> MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo();
        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of Promotion</returns>
        System.Threading.Tasks.Task<Promotion> MarketingModulePromotionGetPromotionByIdAsync(string id);

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of ApiResponse (Promotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<Promotion>> MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo(string id);
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModulePromotionUpdatePromotionsAsync(Promotion promotion);

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo(Promotion promotion);
        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of MarketingSearchResult</returns>
        System.Threading.Tasks.Task<MarketingSearchResult> MarketingModuleSearchAsync(MarketingSearchCriteria criteria);

        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (MarketingSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<MarketingSearchResult>> MarketingModuleSearchAsyncWithHttpInfo(MarketingSearchCriteria criteria);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class VirtoCommerceMarketingApi : IVirtoCommerceMarketingApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceMarketingApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="apiClient">An instance of ApiClient.</param>
        /// <returns></returns>
        public VirtoCommerceMarketingApi(ApiClient apiClient)
        {
            ApiClient = apiClient;
            Configuration = apiClient.Configuration;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the API client object
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient { get; set; }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>DynamicContentItem</returns>
        public DynamicContentItem MarketingModuleDynamicContentCreateDynamicContent(DynamicContentItem contentItem)
        {
             ApiResponse<DynamicContentItem> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo(contentItem);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of DynamicContentItem</returns>
        public ApiResponse<DynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo(DynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentItem)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentItem)));
            
        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of DynamicContentItem</returns>
        public async System.Threading.Tasks.Task<DynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentAsync(DynamicContentItem contentItem)
        {
             ApiResponse<DynamicContentItem> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo(contentItem);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (DynamicContentItem)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentItem>> MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo(DynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentItem)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentItem)));
            
        }
        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>DynamicContentFolder</returns>
        public DynamicContentFolder MarketingModuleDynamicContentCreateDynamicContentFolder(DynamicContentFolder folder)
        {
             ApiResponse<DynamicContentFolder> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo(folder);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>ApiResponse of DynamicContentFolder</returns>
        public ApiResponse<DynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo(DynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentFolder)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentFolder)));
            
        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of DynamicContentFolder</returns>
        public async System.Threading.Tasks.Task<DynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderAsync(DynamicContentFolder folder)
        {
             ApiResponse<DynamicContentFolder> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo(folder);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of ApiResponse (DynamicContentFolder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentFolder>> MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo(DynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentFolder)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentFolder)));
            
        }
        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>DynamicContentPlace</returns>
        public DynamicContentPlace MarketingModuleDynamicContentCreateDynamicContentPlace(DynamicContentPlace contentPlace)
        {
             ApiResponse<DynamicContentPlace> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo(contentPlace);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of DynamicContentPlace</returns>
        public ApiResponse<DynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo(DynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPlace)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPlace)));
            
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of DynamicContentPlace</returns>
        public async System.Threading.Tasks.Task<DynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceAsync(DynamicContentPlace contentPlace)
        {
             ApiResponse<DynamicContentPlace> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo(contentPlace);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (DynamicContentPlace)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentPlace>> MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo(DynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPlace)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPlace)));
            
        }
        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>DynamicContentPublication</returns>
        public DynamicContentPublication MarketingModuleDynamicContentCreateDynamicContentPublication(DynamicContentPublication publication)
        {
             ApiResponse<DynamicContentPublication> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo(publication);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of DynamicContentPublication</returns>
        public ApiResponse<DynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo(DynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPublication)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPublication)));
            
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of DynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<DynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationAsync(DynamicContentPublication publication)
        {
             ApiResponse<DynamicContentPublication> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo(publication);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (DynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentPublication>> MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo(DynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentCreateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPublication)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPublication)));
            
        }
        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentFolders(List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContentFolders");

            var localVarPath = "/api/marketing/contentfolders";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentFoldersAsync(List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContentFolders");

            var localVarPath = "/api/marketing/contentfolders";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentPlaces(List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContentPlaces");

            var localVarPath = "/api/marketing/contentplaces";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPlacesAsync(List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContentPlaces");

            var localVarPath = "/api/marketing/contentplaces";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentPublications(List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContentPublications");

            var localVarPath = "/api/marketing/contentpublications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsync(List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContentPublications");

            var localVarPath = "/api/marketing/contentpublications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContents(List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContents");

            var localVarPath = "/api/marketing/contentitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentsAsync(List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentDeleteDynamicContents");

            var localVarPath = "/api/marketing/contentitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;DynamicContentItem&gt;</returns>
        public List<DynamicContentItem> MarketingModuleDynamicContentEvaluateDynamicContent(DynamicContentEvaluationContext evalContext)
        {
             ApiResponse<List<DynamicContentItem>> localVarResponse = MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo(evalContext);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;DynamicContentItem&gt;</returns>
        public ApiResponse<List<DynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo(DynamicContentEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentEvaluateDynamicContent");

            var localVarPath = "/api/marketing/contentitems/evaluate";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (evalContext.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                localVarPostBody = evalContext; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<DynamicContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<DynamicContentItem>)ApiClient.Deserialize(localVarResponse, typeof(List<DynamicContentItem>)));
            
        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;DynamicContentItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<DynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentAsync(DynamicContentEvaluationContext evalContext)
        {
             ApiResponse<List<DynamicContentItem>> localVarResponse = await MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo(evalContext);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;DynamicContentItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<DynamicContentItem>>> MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo(DynamicContentEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentEvaluateDynamicContent");

            var localVarPath = "/api/marketing/contentitems/evaluate";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (evalContext.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                localVarPostBody = evalContext; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<DynamicContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<DynamicContentItem>)ApiClient.Deserialize(localVarResponse, typeof(List<DynamicContentItem>)));
            
        }
        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>DynamicContentItem</returns>
        public DynamicContentItem MarketingModuleDynamicContentGetDynamicContentById(string id)
        {
             ApiResponse<DynamicContentItem> localVarResponse = MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>ApiResponse of DynamicContentItem</returns>
        public ApiResponse<DynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentById");

            var localVarPath = "/api/marketing/contentitems/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentItem)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentItem)));
            
        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of DynamicContentItem</returns>
        public async System.Threading.Tasks.Task<DynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdAsync(string id)
        {
             ApiResponse<DynamicContentItem> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of ApiResponse (DynamicContentItem)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentItem>> MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentById");

            var localVarPath = "/api/marketing/contentitems/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentItem)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentItem)));
            
        }
        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>DynamicContentFolder</returns>
        public DynamicContentFolder MarketingModuleDynamicContentGetDynamicContentFolderById(string id)
        {
             ApiResponse<DynamicContentFolder> localVarResponse = MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>ApiResponse of DynamicContentFolder</returns>
        public ApiResponse<DynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentFolderById");

            var localVarPath = "/api/marketing/contentfolders/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentFolder)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentFolder)));
            
        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of DynamicContentFolder</returns>
        public async System.Threading.Tasks.Task<DynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsync(string id)
        {
             ApiResponse<DynamicContentFolder> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of ApiResponse (DynamicContentFolder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentFolder>> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentFolderById");

            var localVarPath = "/api/marketing/contentfolders/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentFolder)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentFolder)));
            
        }
        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>DynamicContentPlace</returns>
        public DynamicContentPlace MarketingModuleDynamicContentGetDynamicContentPlaceById(string id)
        {
             ApiResponse<DynamicContentPlace> localVarResponse = MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>ApiResponse of DynamicContentPlace</returns>
        public ApiResponse<DynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentPlaceById");

            var localVarPath = "/api/marketing/contentplaces/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPlace)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPlace)));
            
        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of DynamicContentPlace</returns>
        public async System.Threading.Tasks.Task<DynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsync(string id)
        {
             ApiResponse<DynamicContentPlace> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of ApiResponse (DynamicContentPlace)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentPlace>> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentPlaceById");

            var localVarPath = "/api/marketing/contentplaces/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPlace)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPlace)));
            
        }
        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>DynamicContentPublication</returns>
        public DynamicContentPublication MarketingModuleDynamicContentGetDynamicContentPublicationById(string id)
        {
             ApiResponse<DynamicContentPublication> localVarResponse = MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>ApiResponse of DynamicContentPublication</returns>
        public ApiResponse<DynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentPublicationById");

            var localVarPath = "/api/marketing/contentpublications/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPublication)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPublication)));
            
        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of DynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<DynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsync(string id)
        {
             ApiResponse<DynamicContentPublication> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of ApiResponse (DynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentPublication>> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentGetDynamicContentPublicationById");

            var localVarPath = "/api/marketing/contentpublications/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPublication)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPublication)));
            
        }
        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>DynamicContentPublication</returns>
        public DynamicContentPublication MarketingModuleDynamicContentGetNewDynamicPublication()
        {
             ApiResponse<DynamicContentPublication> localVarResponse = MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of DynamicContentPublication</returns>
        public ApiResponse<DynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo()
        {

            var localVarPath = "/api/marketing/contentpublications/new";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPublication)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPublication)));
            
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of DynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<DynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationAsync()
        {
             ApiResponse<DynamicContentPublication> localVarResponse = await MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (DynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<DynamicContentPublication>> MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo()
        {

            var localVarPath = "/api/marketing/contentpublications/new";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<DynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (DynamicContentPublication)ApiClient.Deserialize(localVarResponse, typeof(DynamicContentPublication)));
            
        }
        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContent(DynamicContentItem contentItem)
        {
             MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo(contentItem);
        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo(DynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentAsync(DynamicContentItem contentItem)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo(contentItem);

        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo(DynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentFolder(DynamicContentFolder folder)
        {
             MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo(folder);
        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo(DynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentFolderAsync(DynamicContentFolder folder)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo(folder);

        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo(DynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentPlace(DynamicContentPlace contentPlace)
        {
             MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo(contentPlace);
        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo(DynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPlaceAsync(DynamicContentPlace contentPlace)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo(contentPlace);

        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo(DynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentPublication(DynamicContentPublication publication)
        {
             MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo(publication);
        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo(DynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPublicationAsync(DynamicContentPublication publication)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo(publication);

        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo(DynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling VirtoCommerceMarketingApi->MarketingModuleDynamicContentUpdateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Promotion</returns>
        public Promotion MarketingModulePromotionCreatePromotion(Promotion promotion)
        {
             ApiResponse<Promotion> localVarResponse = MarketingModulePromotionCreatePromotionWithHttpInfo(promotion);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>ApiResponse of Promotion</returns>
        public ApiResponse<Promotion> MarketingModulePromotionCreatePromotionWithHttpInfo(Promotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling VirtoCommerceMarketingApi->MarketingModulePromotionCreatePromotion");

            var localVarPath = "/api/marketing/promotions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Promotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Promotion)ApiClient.Deserialize(localVarResponse, typeof(Promotion)));
            
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of Promotion</returns>
        public async System.Threading.Tasks.Task<Promotion> MarketingModulePromotionCreatePromotionAsync(Promotion promotion)
        {
             ApiResponse<Promotion> localVarResponse = await MarketingModulePromotionCreatePromotionAsyncWithHttpInfo(promotion);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of ApiResponse (Promotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Promotion>> MarketingModulePromotionCreatePromotionAsyncWithHttpInfo(Promotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling VirtoCommerceMarketingApi->MarketingModulePromotionCreatePromotion");

            var localVarPath = "/api/marketing/promotions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Promotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Promotion)ApiClient.Deserialize(localVarResponse, typeof(Promotion)));
            
        }
        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns></returns>
        public void MarketingModulePromotionDeletePromotions(List<string> ids)
        {
             MarketingModulePromotionDeletePromotionsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModulePromotionDeletePromotionsWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModulePromotionDeletePromotions");

            var localVarPath = "/api/marketing/promotions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModulePromotionDeletePromotionsAsync(List<string> ids)
        {
             await MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo(List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling VirtoCommerceMarketingApi->MarketingModulePromotionDeletePromotions");

            var localVarPath = "/api/marketing/promotions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>List&lt;PromotionReward&gt;</returns>
        public List<PromotionReward> MarketingModulePromotionEvaluatePromotions(PromotionEvaluationContext context)
        {
             ApiResponse<List<PromotionReward>> localVarResponse = MarketingModulePromotionEvaluatePromotionsWithHttpInfo(context);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>ApiResponse of List&lt;PromotionReward&gt;</returns>
        public ApiResponse<List<PromotionReward>> MarketingModulePromotionEvaluatePromotionsWithHttpInfo(PromotionEvaluationContext context)
        {
            // verify the required parameter 'context' is set
            if (context == null)
                throw new ApiException(400, "Missing required parameter 'context' when calling VirtoCommerceMarketingApi->MarketingModulePromotionEvaluatePromotions");

            var localVarPath = "/api/marketing/promotions/evaluate";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (context.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(context); // http body (model) parameter
            }
            else
            {
                localVarPostBody = context; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<PromotionReward>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<PromotionReward>)ApiClient.Deserialize(localVarResponse, typeof(List<PromotionReward>)));
            
        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of List&lt;PromotionReward&gt;</returns>
        public async System.Threading.Tasks.Task<List<PromotionReward>> MarketingModulePromotionEvaluatePromotionsAsync(PromotionEvaluationContext context)
        {
             ApiResponse<List<PromotionReward>> localVarResponse = await MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo(context);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;PromotionReward&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<PromotionReward>>> MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo(PromotionEvaluationContext context)
        {
            // verify the required parameter 'context' is set
            if (context == null)
                throw new ApiException(400, "Missing required parameter 'context' when calling VirtoCommerceMarketingApi->MarketingModulePromotionEvaluatePromotions");

            var localVarPath = "/api/marketing/promotions/evaluate";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (context.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(context); // http body (model) parameter
            }
            else
            {
                localVarPostBody = context; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<PromotionReward>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<PromotionReward>)ApiClient.Deserialize(localVarResponse, typeof(List<PromotionReward>)));
            
        }
        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Promotion</returns>
        public Promotion MarketingModulePromotionGetNewDynamicPromotion()
        {
             ApiResponse<Promotion> localVarResponse = MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of Promotion</returns>
        public ApiResponse<Promotion> MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo()
        {

            var localVarPath = "/api/marketing/promotions/new";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Promotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Promotion)ApiClient.Deserialize(localVarResponse, typeof(Promotion)));
            
        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of Promotion</returns>
        public async System.Threading.Tasks.Task<Promotion> MarketingModulePromotionGetNewDynamicPromotionAsync()
        {
             ApiResponse<Promotion> localVarResponse = await MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (Promotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Promotion>> MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo()
        {

            var localVarPath = "/api/marketing/promotions/new";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Promotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Promotion)ApiClient.Deserialize(localVarResponse, typeof(Promotion)));
            
        }
        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Promotion</returns>
        public Promotion MarketingModulePromotionGetPromotionById(string id)
        {
             ApiResponse<Promotion> localVarResponse = MarketingModulePromotionGetPromotionByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>ApiResponse of Promotion</returns>
        public ApiResponse<Promotion> MarketingModulePromotionGetPromotionByIdWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModulePromotionGetPromotionById");

            var localVarPath = "/api/marketing/promotions/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Promotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Promotion)ApiClient.Deserialize(localVarResponse, typeof(Promotion)));
            
        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of Promotion</returns>
        public async System.Threading.Tasks.Task<Promotion> MarketingModulePromotionGetPromotionByIdAsync(string id)
        {
             ApiResponse<Promotion> localVarResponse = await MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of ApiResponse (Promotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Promotion>> MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo(string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling VirtoCommerceMarketingApi->MarketingModulePromotionGetPromotionById");

            var localVarPath = "/api/marketing/promotions/{id}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<Promotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (Promotion)ApiClient.Deserialize(localVarResponse, typeof(Promotion)));
            
        }
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns></returns>
        public void MarketingModulePromotionUpdatePromotions(Promotion promotion)
        {
             MarketingModulePromotionUpdatePromotionsWithHttpInfo(promotion);
        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MarketingModulePromotionUpdatePromotionsWithHttpInfo(Promotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling VirtoCommerceMarketingApi->MarketingModulePromotionUpdatePromotions");

            var localVarPath = "/api/marketing/promotions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModulePromotionUpdatePromotionsAsync(Promotion promotion)
        {
             await MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo(promotion);

        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo(Promotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling VirtoCommerceMarketingApi->MarketingModulePromotionUpdatePromotions");

            var localVarPath = "/api/marketing/promotions";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>MarketingSearchResult</returns>
        public MarketingSearchResult MarketingModuleSearch(MarketingSearchCriteria criteria)
        {
             ApiResponse<MarketingSearchResult> localVarResponse = MarketingModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of MarketingSearchResult</returns>
        public ApiResponse<MarketingSearchResult> MarketingModuleSearchWithHttpInfo(MarketingSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommerceMarketingApi->MarketingModuleSearch");

            var localVarPath = "/api/marketing/search";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<MarketingSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (MarketingSearchResult)ApiClient.Deserialize(localVarResponse, typeof(MarketingSearchResult)));
            
        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of MarketingSearchResult</returns>
        public async System.Threading.Tasks.Task<MarketingSearchResult> MarketingModuleSearchAsync(MarketingSearchCriteria criteria)
        {
             ApiResponse<MarketingSearchResult> localVarResponse = await MarketingModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.MarketingModule.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (MarketingSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<MarketingSearchResult>> MarketingModuleSearchAsyncWithHttpInfo(MarketingSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling VirtoCommerceMarketingApi->MarketingModuleSearch");

            var localVarPath = "/api/marketing/search";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<MarketingSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (MarketingSearchResult)ApiClient.Deserialize(localVarResponse, typeof(MarketingSearchResult)));
            
        }
    }
}
