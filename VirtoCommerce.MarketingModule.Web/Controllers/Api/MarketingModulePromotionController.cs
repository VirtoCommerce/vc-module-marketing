using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.MarketingModule.Web.Model.PushNotifications;
using VirtoCommerce.MarketingModule.Web.Security;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Core.Web.Security;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
    [RoutePrefix("api/marketing/promotions")]
    [CheckPermission(Permission = MarketingPredefinedPermissions.Read)]
    public class MarketingModulePromotionController : ApiController
    {
        private readonly IMarketingExtensionManager _marketingExtensionManager;
        private readonly IPromotionService _promotionService;
        private readonly IMarketingPromoEvaluator _promoEvaluator;
        private readonly IExpressionSerializer _expressionSerializer;
        private readonly IPromotionSearchService _promoSearchService;
        private readonly IUserNameResolver _userNameResolver;
        private readonly IPushNotificationManager _notifier;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly CsvCouponImporter _csvCouponImporter;

        public MarketingModulePromotionController(
            IPromotionService promotionService,
            IMarketingExtensionManager promotionManager,
            IMarketingPromoEvaluator promoEvaluator,
            IExpressionSerializer expressionSerializer,
            IPromotionSearchService promoSearchService,
            IUserNameResolver userNameResolver,
            IPushNotificationManager notifier,
            IBlobStorageProvider blobStorageProvider,
            CsvCouponImporter csvCouponImporter)
        {
            _marketingExtensionManager = promotionManager;
            _promotionService = promotionService;
            _promoEvaluator = promoEvaluator;
            _expressionSerializer = expressionSerializer;
            _promoSearchService = promoSearchService;
            _userNameResolver = userNameResolver;
            _notifier = notifier;
            _blobStorageProvider = blobStorageProvider;
            _csvCouponImporter = csvCouponImporter;
        }

        /// <summary>
        /// Search dynamic content places by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(GenericSearchResult<webModel.Promotion>))]
        public IHttpActionResult PromotionsSearch(PromotionSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<webModel.Promotion>();
            var promoSearchResult = _promoSearchService.SearchPromotions(criteria);
            retVal.TotalCount = promoSearchResult.TotalCount;
            retVal.Results = promoSearchResult.Results.Select(x => x.ToWebModel()).ToList();
            return Ok(retVal);
        }

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <param name="context">Promotion evaluation context</param>
        [HttpPost]
        [ResponseType(typeof(webModel.PromotionReward[]))]
        [Route("evaluate")]
        public IHttpActionResult EvaluatePromotions(coreModel.PromotionEvaluationContext context)
        {
            var retVal = _promoEvaluator.EvaluatePromotion(context);
            return Ok(retVal.Rewards.Select(x => x.ToWebModel()).ToArray());
        }

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>Return a single promotion (dynamic or custom) object </remarks>
        /// <param name="id">promotion id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("{id}")]
        public IHttpActionResult GetPromotionById(string id)
        {
            var retVal = _promotionService.GetPromotionById(id);
            if (retVal != null)
            {
                return Ok(retVal.ToWebModel(_marketingExtensionManager.PromotionDynamicExpressionTree));
            }
            return NotFound();
        }

        /// <summary>
        /// Get new dynamic promotion object 
        /// </summary>
        /// <remarks>Return a new dynamic promotion object with populated dynamic expression tree</remarks>
        [HttpGet]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("new")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult GetNewDynamicPromotion()
        {
            var retVal = new webModel.Promotion
            {
                Type = typeof(DynamicPromotion).Name,
                DynamicExpression = _marketingExtensionManager.PromotionDynamicExpressionTree,
                IsActive = true
            };
            return Ok(retVal);
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        [HttpPost]
        [ResponseType(typeof(webModel.Promotion))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreatePromotion(webModel.Promotion promotion)
        {
            var retVal = _promotionService.CreatePromotion(promotion.ToCoreModel(_expressionSerializer));
            return GetPromotionById(retVal.Id);
        }


        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <param name="promotion">>dynamic promotion object that needs to be updated in the marketing system</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdatePromotions(webModel.Promotion promotion)
        {
            _promotionService.UpdatePromotions(new[] { promotion.ToCoreModel(_expressionSerializer) });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///  Delete promotions objects
        /// </summary>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Delete)]
        public IHttpActionResult DeletePromotions([FromUri] string[] ids)
        {
            _promotionService.DeletePromotions(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("coupons/search")]
        [ResponseType(typeof(GenericSearchResult<coreModel.Coupon>))]
        public IHttpActionResult SearchCoupons(coreModel.Promotions.Search.CouponSearchCriteria criteria)
        {
            var searchResult = _promotionService.SearchCoupons(criteria);

            return Ok(searchResult);
        }

        [HttpDelete]
        [Route("coupons/delete")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteCoupons([FromUri] string[] ids)
        {
            _promotionService.DeleteCoupons(ids);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("coupons/clear")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ClearCoupons([FromUri] string promotionId)
        {
            _promotionService.ClearCoupons(promotionId);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("coupons/import")]
        [ResponseType(typeof(ImportNotification))]
        public IHttpActionResult ImportCoupons(ImportRequest request)
        {
            var notification = new ImportNotification(_userNameResolver.GetCurrentUserName())
            {
                Title = "Import coupons from CSV",
                Description = "Starting import..."
            };

            _notifier.Upsert(notification);

            BackgroundJob.Enqueue(() => BackgroundImport(request, notification));

            return Ok(notification);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void BackgroundImport(ImportRequest request, ImportNotification notification)
        {
            Action<ExportImportProgressInfo> progressCallback = c =>
            {
                notification.InjectFrom(c);
                _notifier.Upsert(notification);
            };

            using (var stream = _blobStorageProvider.OpenRead(request.FileUrl))
            {
                try
                {
                    _csvCouponImporter.DoImport(stream, request.Delimiter, request.PromotionId, request.ExpirationDate, progressCallback);
                }
                catch (Exception exception)
                {
                    notification.Description = "Import error";
                    notification.ErrorCount++;
                    notification.Errors.Add(exception.ToString());
                }
                finally
                {
                    notification.Finished = DateTime.UtcNow;
                    notification.Description = "Import finished" + (notification.Errors.Any() ? " with errors" : " successfully");
                    _notifier.Upsert(notification);
                }
            }
        }
    }
}