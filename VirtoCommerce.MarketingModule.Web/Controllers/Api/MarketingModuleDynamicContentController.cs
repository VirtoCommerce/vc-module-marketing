using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.MarketingModule.Web.Security;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Core.Web.Security;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
    [RoutePrefix("api/marketing")]
    [CheckPermission(Permission = MarketingPredefinedPermissions.Read)]
    public class MarketingModuleDynamicContentController : ApiController
    {
        private readonly IMarketingExtensionManager _marketingExtensionManager;
        private readonly IDynamicContentService _dynamicContentService;
        private readonly IMarketingDynamicContentEvaluator _dynamicContentEvaluator;
        private readonly IExpressionSerializer _expressionSerializer;
        private readonly IDynamicContentSearchService _dynamicConentSearchService;

        public MarketingModuleDynamicContentController(IDynamicContentService dynamicContentService, IMarketingExtensionManager marketingExtensionManager,
            IMarketingDynamicContentEvaluator dynamicContentEvaluator, IExpressionSerializer expressionSerializer, IDynamicContentSearchService dynamicConentSearchService)
        {
            _dynamicContentService = dynamicContentService;
            _marketingExtensionManager = marketingExtensionManager;
            _dynamicContentEvaluator = dynamicContentEvaluator;
            _expressionSerializer = expressionSerializer;
            _dynamicConentSearchService = dynamicConentSearchService;
        }

        /// <summary>
        /// Search content places list entries by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("contentplaces/listentries/search")]
        [ResponseType(typeof(GenericSearchResult<webModel.DynamicContentListEntry>))]
        public IHttpActionResult DynamicContentPlaceListEntriesSearch(DynamicContentPlaceSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<webModel.DynamicContentListEntry>();
            retVal.Results = new List<webModel.DynamicContentListEntry>();
            var foldersSearchResult = _dynamicConentSearchService.SearchFolders(new DynamicContentFolderSearchCriteria { FolderId = criteria.FolderId, Keyword = criteria.Keyword, Take = criteria.Take, Skip = criteria.Skip, Sort = criteria.Sort });
            retVal.Results.AddRange(foldersSearchResult.Results.Select(x => x.ToWebModel()));
            retVal.TotalCount = foldersSearchResult.TotalCount;

            criteria.Skip = Math.Max(0, criteria.Skip - retVal.TotalCount);
            criteria.Take = Math.Max(0, criteria.Take - retVal.Results.Count());

            var placesSearchResult = _dynamicConentSearchService.SearchContentPlaces(criteria);
            retVal.TotalCount += placesSearchResult.TotalCount;
            retVal.Results.AddRange(placesSearchResult.Results.Select(x=>x.ToWebModel()));

            return Ok(retVal);
        }

        /// <summary>
        /// Search dynamic content places by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("contentplaces/search")]
        [ResponseType(typeof(GenericSearchResult<webModel.DynamicContentPlace>))]
        public IHttpActionResult DynamicContentPlacesSearch(DynamicContentPlaceSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<webModel.DynamicContentPlace>();          
            var placesSearchResult = _dynamicConentSearchService.SearchContentPlaces(criteria);
            retVal.TotalCount = placesSearchResult.TotalCount;
            retVal.Results = placesSearchResult.Results.Select(x => x.ToWebModel()).ToList();
            return Ok(retVal);
        }

        /// <summary>
        /// Search content places list entries by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("contentitems/listentries/search")]
        [ResponseType(typeof(GenericSearchResult<webModel.DynamicContentListEntry>))]
        public IHttpActionResult DynamicContentItemsEntriesSearch(DynamicContentItemSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<webModel.DynamicContentListEntry>();
            retVal.Results = new List<webModel.DynamicContentListEntry>();
            var foldersSearchResult = _dynamicConentSearchService.SearchFolders(new DynamicContentFolderSearchCriteria { FolderId = criteria.FolderId, Keyword = criteria.Keyword, Take = criteria.Take, Skip = criteria.Skip, Sort = criteria.Sort });
            retVal.Results.AddRange(foldersSearchResult.Results.Select(x => x.ToWebModel()));
            retVal.TotalCount = foldersSearchResult.TotalCount;

            criteria.Skip = Math.Max(0, criteria.Skip - retVal.TotalCount);
            criteria.Take = Math.Max(0, criteria.Take - retVal.Results.Count());

            var itemsSearchResult = _dynamicConentSearchService.SearchContentItems(criteria);
            retVal.TotalCount += itemsSearchResult.TotalCount;
            retVal.Results.AddRange(itemsSearchResult.Results.Select(x => x.ToWebModel()));

            return Ok(retVal);
        }

        /// <summary>
        /// Search dynamic content items by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("contentitems/search")]
        [ResponseType(typeof(GenericSearchResult<webModel.DynamicContentItem>))]
        public IHttpActionResult DynamicContentItemsSearch(DynamicContentItemSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<webModel.DynamicContentItem>();
            var itemsSearchResult = _dynamicConentSearchService.SearchContentItems(criteria);
            retVal.TotalCount = itemsSearchResult.TotalCount;
            retVal.Results = itemsSearchResult.Results.Select(x => x.ToWebModel()).ToList();
            return Ok(retVal);
        }

        /// <summary>
        /// Search dynamic content items by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("contentpublications/search")]
        [ResponseType(typeof(GenericSearchResult<webModel.DynamicContentPublication>))]
        public IHttpActionResult DynamicContentPublicationsSearch(DynamicContentPublicationSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<webModel.DynamicContentPublication>();
            var publicationSearchResult = _dynamicConentSearchService.SearchContentPublications(criteria);
            retVal.TotalCount = publicationSearchResult.TotalCount;
            retVal.Results = publicationSearchResult.Results.Select(x => x.ToWebModel()).ToList();
            return Ok(retVal);
        }


        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        [HttpPost]
        [ResponseType(typeof(webModel.DynamicContentItem[]))]
        [Route("contentitems/evaluate")]
        public IHttpActionResult EvaluateDynamicContent(coreModel.DynamicContent.DynamicContentEvaluationContext evalContext)
        {
            var retVal = _dynamicContentEvaluator.EvaluateItems(evalContext).Select(x => x.ToWebModel()).ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>Return a single dynamic content item object </remarks>
        /// <param name="id"> content item id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.DynamicContentItem))]
        [Route("contentitems/{id}")]
        public IHttpActionResult GetDynamicContentById(string id)
        {
            var retVal = _dynamicContentService.GetContentItemById(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel());
            }
            return NotFound();
        }


        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        [HttpPost]
        [ResponseType(typeof(webModel.DynamicContentItem))]
        [Route("contentitems")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreateDynamicContent(webModel.DynamicContentItem contentItem)
        {
            var retVal = _dynamicContentService.CreateContent(contentItem.ToCoreModel());
            return GetDynamicContentById(retVal.Id);
        }


        /// <summary>
        ///  Update a existing dynamic content item object
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("contentitems")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdateDynamicContent(webModel.DynamicContentItem contentItem)
        {
            _dynamicContentService.UpdateContents(new[] { contentItem.ToCoreModel() });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///  Delete a dynamic content item objects
        /// </summary>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("contentitems")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteDynamicContents([FromUri] string[] ids)
        {
            _dynamicContentService.DeleteContents(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>Return a single dynamic content place object </remarks>
        /// <param name="id">place id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.DynamicContentPlace))]
        [Route("contentplaces/{id}")]
        public IHttpActionResult GetDynamicContentPlaceById(string id)
        {
            var retVal = _dynamicContentService.GetPlaceById(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel());
            }
            return NotFound();
        }


        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        [HttpPost]
        [ResponseType(typeof(webModel.DynamicContentPlace))]
        [Route("contentplaces")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreateDynamicContentPlace(webModel.DynamicContentPlace contentPlace)
        {
            var retVal = _dynamicContentService.CreatePlace(contentPlace.ToCoreModel());
            return GetDynamicContentPlaceById(retVal.Id);
        }


        /// <summary>
        ///  Update a existing dynamic content place object
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("contentplaces")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdateDynamicContentPlace(webModel.DynamicContentPlace contentPlace)
        {
            _dynamicContentService.UpdatePlace(contentPlace.ToCoreModel());
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///  Delete a dynamic content place objects
        /// </summary>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("contentplaces")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteDynamicContentPlaces([FromUri] string[] ids)
        {
            _dynamicContentService.DeletePlaces(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(webModel.DynamicContentPublication))]
        [Route("contentpublications/new")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult GetNewDynamicPublication()
        {
            var retVal = new webModel.DynamicContentPublication
            {
                ContentItems = new webModel.DynamicContentItem[] { },
                ContentPlaces = new webModel.DynamicContentPlace[] { },
                DynamicExpression = _marketingExtensionManager.DynamicContentExpressionTree,
                IsActive = true
            };
            return Ok(retVal);
        }

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>Return a single dynamic content publication object </remarks>
        /// <param name="id">publication id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.DynamicContentPublication))]
        [Route("contentpublications/{id}")]
        public IHttpActionResult GetDynamicContentPublicationById(string id)
        {
            var retVal = _dynamicContentService.GetPublicationById(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel(_marketingExtensionManager.DynamicContentExpressionTree));
            }
            return NotFound();
        }


        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        [HttpPost]
        [ResponseType(typeof(webModel.DynamicContentPublication))]
        [Route("contentpublications")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreateDynamicContentPublication(webModel.DynamicContentPublication publication)
        {
            var retVal = _dynamicContentService.CreatePublication(publication.ToCoreModel(_expressionSerializer));
            return GetDynamicContentPublicationById(retVal.Id);
        }


        /// <summary>
        ///  Update a existing dynamic content publication object
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("contentpublications")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdateDynamicContentPublication(webModel.DynamicContentPublication publication)
        {
            _dynamicContentService.UpdatePublications(new[] { publication.ToCoreModel(_expressionSerializer) });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///  Delete a dynamic content publication objects
        /// </summary>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("contentpublications")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteDynamicContentPublications([FromUri] string[] ids)
        {
            _dynamicContentService.DeletePublications(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>Return a single dynamic content folder</remarks>
        /// <param name="id">folder id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.DynamicContentFolder))]
        [Route("contentfolders/{id}")]
        public IHttpActionResult GetDynamicContentFolderById(string id)
        {
            var retVal = _dynamicContentService.GetFolderById(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel());
            }
            return NotFound();
        }


        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        [HttpPost]
        [ResponseType(typeof(webModel.DynamicContentFolder))]
        [Route("contentfolders")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreateDynamicContentFolder(webModel.DynamicContentFolder folder)
        {
            var retVal = _dynamicContentService.CreateFolder(folder.ToCoreModel());
            return GetDynamicContentFolderById(retVal.Id);
        }

        /// <summary>
        ///  Update a existing dynamic content folder
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("contentfolders")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdateDynamicContentFolder(webModel.DynamicContentFolder folder)
        {
            _dynamicContentService.UpdateFolder(folder.ToCoreModel());
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///  Delete a dynamic content folders
        /// </summary>
        /// <param name="ids">folders ids for delete</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("contentfolders")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteDynamicContentFolders([FromUri] string[] ids)
        {
            _dynamicContentService.DeleteFolder(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }


    }
}
