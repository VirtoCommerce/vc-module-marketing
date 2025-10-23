using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.MarketingModule.Core;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.MarketingModule.Core.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
    [Route("api/marketing")]
    [Authorize]
    public class MarketingModuleDynamicContentController(
        IMarketingDynamicContentEvaluator dynamicContentEvaluator,
        IDynamicContentFolderService dynamicContentFolderService,
        IDynamicContentFolderSearchService dynamicContentFolderSearchService,
        IDynamicContentItemService dynamicContentItemService,
        IDynamicContentItemSearchService dynamicContentItemSearchService,
        IDynamicContentPlaceService dynamicContentPlaceService,
        IDynamicContentPlaceSearchService dynamicContentPlaceSearchService,
        IDynamicContentPublicationService dynamicContentPublicationService,
        IDynamicContentPublicationSearchService dynamicContentPublicationSearchService)
        : Controller
    {
        /// <summary>
        /// Search content places list entries by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [HttpPost]
        [Route("contentplaces/listentries/search")]
        public async Task<ActionResult<DynamicContentListEntrySearchResult>> DynamicContentPlaceListEntriesSearch([FromBody] coreModel.DynamicContentPlaceSearchCriteria criteria)
        {
            var result = AbstractTypeFactory<DynamicContentListEntrySearchResult>.TryCreateInstance();

            var folderSearchCriteria = AbstractTypeFactory<coreModel.DynamicContentFolderSearchCriteria>.TryCreateInstance();
            folderSearchCriteria.FolderId = criteria.FolderId;
            folderSearchCriteria.Keyword = criteria.Keyword;
            folderSearchCriteria.Sort = criteria.Sort;
            folderSearchCriteria.Skip = criteria.Skip;
            folderSearchCriteria.Take = criteria.Take;

            var foldersSearchResult = await dynamicContentFolderSearchService.SearchNoCloneAsync(folderSearchCriteria);
            var folderSkip = Math.Min(foldersSearchResult.TotalCount, criteria.Skip);
            var folderTake = Math.Min(criteria.Take, Math.Max(0, foldersSearchResult.TotalCount - criteria.Skip));
            result.TotalCount += foldersSearchResult.TotalCount;
            result.Results.AddRange(foldersSearchResult.Results.Skip(folderSkip).Take(folderTake));

            criteria.Skip -= folderSkip;
            criteria.Take -= folderTake;

            var placesSearchResult = await dynamicContentPlaceSearchService.SearchNoCloneAsync(criteria);
            result.TotalCount += placesSearchResult.TotalCount;
            result.Results.AddRange(placesSearchResult.Results);

            return Ok(result);
        }

        /// <summary>
        /// Search dynamic content places by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [Route("contentplaces/search")]
        public async Task<ActionResult<DynamicContentPlaceSearchResult>> DynamicContentPlacesSearch([FromBody] coreModel.DynamicContentPlaceSearchCriteria criteria)
        {
            var placesSearchResult = await dynamicContentPlaceSearchService.SearchNoCloneAsync(criteria);
            return Ok(placesSearchResult);
        }

        /// <summary>
        /// Search content places list entries by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [Route("contentitems/listentries/search")]
        public async Task<ActionResult<DynamicContentListEntrySearchResult>> DynamicContentItemsEntriesSearch([FromBody] coreModel.DynamicContentItemSearchCriteria criteria)
        {
            var result = AbstractTypeFactory<DynamicContentListEntrySearchResult>.TryCreateInstance();

            var folderSearchCriteria = AbstractTypeFactory<coreModel.DynamicContentFolderSearchCriteria>.TryCreateInstance();
            folderSearchCriteria.FolderId = criteria.FolderId;
            folderSearchCriteria.Keyword = criteria.Keyword;
            folderSearchCriteria.Sort = criteria.Sort;
            folderSearchCriteria.Skip = criteria.Skip;
            folderSearchCriteria.Take = criteria.Take;

            var foldersSearchResult = await dynamicContentFolderSearchService.SearchNoCloneAsync(folderSearchCriteria);
            var folderSkip = Math.Min(foldersSearchResult.TotalCount, criteria.Skip);
            var folderTake = Math.Min(criteria.Take, Math.Max(0, foldersSearchResult.TotalCount - criteria.Skip));
            result.TotalCount += foldersSearchResult.TotalCount;
            result.Results.AddRange(foldersSearchResult.Results.Skip(folderSkip).Take(folderTake));

            criteria.Skip -= folderSkip;
            criteria.Take -= folderTake;

            var itemsSearchResult = await dynamicContentItemSearchService.SearchNoCloneAsync(criteria);
            result.TotalCount += itemsSearchResult.TotalCount;
            result.Results.AddRange(itemsSearchResult.Results);

            return Ok(result);
        }

        /// <summary>
        /// Search dynamic content items by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [Route("contentitems/search")]
        public async Task<ActionResult<DynamicContentItemSearchResult>> DynamicContentItemsSearch([FromBody] coreModel.DynamicContentItemSearchCriteria criteria)
        {
            var itemsSearchResult = await dynamicContentItemSearchService.SearchNoCloneAsync(criteria);
            return Ok(itemsSearchResult);
        }

        /// <summary>
        /// Search dynamic content items by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [Route("contentpublications/search")]
        public async Task<ActionResult<DynamicContentPublicationSearchResult>> DynamicContentPublicationsSearch([FromBody] coreModel.DynamicContentPublicationSearchCriteria criteria)
        {
            var publicationSearchResult = await dynamicContentPublicationSearchService.SearchNoCloneAsync(criteria);
            return Ok(publicationSearchResult);
        }

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        [HttpPost]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [Route("contentitems/evaluate")]
        public async Task<ActionResult<coreModel.DynamicContentItem[]>> EvaluateDynamicContent([FromBody] coreModel.DynamicContentEvaluationContext evalContext)
        {
            var items = await dynamicContentEvaluator.EvaluateItemsAsync(evalContext);
            return Ok(items);
        }

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>Return a single dynamic content item object </remarks>
        /// <param name="id"> content item id</param>
        [HttpGet]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        [Route("contentitems/{id}")]
        public async Task<ActionResult<coreModel.DynamicContentItem>> GetDynamicContentById(string id)
        {
            var item = await dynamicContentItemService.GetByIdAsync(id);

            return item != null
                ? Ok(item)
                : NotFound();
        }

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        [HttpPost]
        [Route("contentitems")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public async Task<ActionResult<coreModel.DynamicContentItem>> CreateDynamicContent([FromBody] coreModel.DynamicContentItem contentItem)
        {
            await dynamicContentItemService.SaveChangesAsync([contentItem]);
            return await GetDynamicContentById(contentItem.Id);
        }

        /// <summary>
        ///  Update an existing dynamic content item object
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        [HttpPut]
        [Route("contentitems")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult UpdateDynamicContent([FromBody] coreModel.DynamicContentItem contentItem)
        {
            dynamicContentItemService.SaveChangesAsync([contentItem]);
            return NoContent();
        }

        /// <summary>
        ///  Delete a dynamic content item objects
        /// </summary>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        [HttpDelete]
        [Route("contentitems")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDynamicContents([FromQuery] string[] ids)
        {
            await dynamicContentItemService.DeleteAsync(ids);
            return NoContent();
        }

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>Return a single dynamic content place object </remarks>
        /// <param name="id">place id</param>
        [HttpGet]
        [Route("contentplaces/{id}")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<coreModel.DynamicContentPlace>> GetDynamicContentPlaceById(string id)
        {
            var place = await dynamicContentPlaceService.GetByIdAsync(id);

            return place != null
                ? Ok(place)
                : NotFound();
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        [HttpPost]
        [Route("contentplaces")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public async Task<ActionResult<coreModel.DynamicContentPlace>> CreateDynamicContentPlace([FromBody] coreModel.DynamicContentPlace contentPlace)
        {
            await dynamicContentPlaceService.SaveChangesAsync([contentPlace]);
            return await GetDynamicContentPlaceById(contentPlace.Id);
        }

        /// <summary>
        ///  Update a existing dynamic content place object
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        [HttpPut]
        [Route("contentplaces")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateDynamicContentPlace([FromBody] coreModel.DynamicContentPlace contentPlace)
        {
            await dynamicContentPlaceService.SaveChangesAsync([contentPlace]);
            return NoContent();
        }

        /// <summary>
        ///  Delete a dynamic content place objects
        /// </summary>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        [HttpDelete]
        [Route("contentplaces")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDynamicContentPlaces([FromQuery] string[] ids)
        {
            await dynamicContentPlaceService.DeleteAsync(ids);
            return NoContent();
        }

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        [HttpGet]
        [Route("contentpublications/new")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public ActionResult<coreModel.DynamicContentPublication> GetNewDynamicPublication()
        {
            var result = AbstractTypeFactory<coreModel.DynamicContentPublication>.TryCreateInstance();
            result.IsActive = true;
            result.ContentItems = new List<coreModel.DynamicContentItem>();
            result.ContentPlaces = new List<coreModel.DynamicContentPlace>();
            result.DynamicExpression = AbstractTypeFactory<DynamicContentConditionTree>.TryCreateInstance();
            result.DynamicExpression.MergeFromPrototype(AbstractTypeFactory<DynamicContentConditionTreePrototype>.TryCreateInstance());
            return Ok(result);
        }

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>Return a single dynamic content publication object </remarks>
        /// <param name="id">publication id</param>
        [HttpGet]
        [Route("contentpublications/{id}")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<coreModel.DynamicContentPublication>> GetDynamicContentPublicationById(string id)
        {
            var publication = await dynamicContentPublicationService.GetByIdAsync(id);
            if (publication == null)
            {
                return NotFound();
            }

            publication.DynamicExpression?.MergeFromPrototype(AbstractTypeFactory<DynamicContentConditionTreePrototype>.TryCreateInstance());
            return Ok(publication);
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        [HttpPost]
        [Route("contentpublications")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public async Task<ActionResult<coreModel.DynamicContentPublication>> CreateDynamicContentPublication([FromBody] coreModel.DynamicContentPublication publication)
        {
            await dynamicContentPublicationService.SaveChangesAsync([publication]);
            return await GetDynamicContentPublicationById(publication.Id);
        }

        /// <summary>
        ///  Update an existing dynamic content publication object
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        [HttpPut]
        [Route("contentpublications")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateDynamicContentPublication([FromBody] coreModel.DynamicContentPublication publication)
        {
            await dynamicContentPublicationService.SaveChangesAsync([publication]);
            return NoContent();
        }

        /// <summary>
        ///  Delete a dynamic content publication objects
        /// </summary>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        [HttpDelete]
        [Route("contentpublications")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDynamicContentPublications([FromQuery] string[] ids)
        {
            await dynamicContentPublicationService.DeleteAsync(ids);
            return NoContent();
        }

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>Return a single dynamic content folder</remarks>
        /// <param name="id">folder id</param>
        [HttpGet]
        [Route("contentfolders/{id}")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<coreModel.DynamicContentFolder>> GetDynamicContentFolderById(string id)
        {
            var folder = await dynamicContentFolderService.GetByIdAsync(id);

            return folder != null
                ? Ok(folder)
                : NotFound();
        }

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        [HttpPost]
        [Route("contentfolders")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public async Task<ActionResult<coreModel.DynamicContentFolder>> CreateDynamicContentFolder([FromBody] coreModel.DynamicContentFolder folder)
        {
            await dynamicContentFolderService.SaveChangesAsync([folder]);
            return await GetDynamicContentFolderById(folder.Id);
        }

        /// <summary>
        ///  Update an existing dynamic content folder
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        [HttpPut]
        [Route("contentfolders")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateDynamicContentFolder([FromBody] coreModel.DynamicContentFolder folder)
        {
            await dynamicContentFolderService.SaveChangesAsync([folder]);
            return NoContent();
        }

        /// <summary>
        ///  Delete a dynamic content folders
        /// </summary>
        /// <param name="ids">folders ids for delete</param>
        [HttpDelete]
        [Route("contentfolders")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDynamicContentFolders([FromQuery] string[] ids)
        {
            await dynamicContentFolderService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
