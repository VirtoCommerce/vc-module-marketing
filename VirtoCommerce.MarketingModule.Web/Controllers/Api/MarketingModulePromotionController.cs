using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.MarketingModule.Web.Model.PushNotifications;
using VirtoCommerce.MarketingModule.Web.Security;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
    [RoutePrefix("api/marketing/promotions")]
    [CheckPermission(Permission = MarketingPredefinedPermissions.Access)]
    public class MarketingModulePromotionController : ApiController
    {
        private readonly IPromotionService _promotionService;
        private readonly ICouponService _couponService;
        private readonly IMarketingPromoEvaluator _promoEvaluator;
        private readonly IPromotionSearchService _promoSearchService;
        private readonly IUserNameResolver _userNameResolver;
        private readonly IPushNotificationManager _notifier;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly CsvCouponImporter _csvCouponImporter;
        private readonly ISecurityService _securityService;
        private readonly IPermissionScopeService _permissionScopeService;

        public MarketingModulePromotionController(
            IPromotionService promotionService,
            ICouponService couponService,
            IMarketingPromoEvaluator promoEvaluator,
            IPromotionSearchService promoSearchService,
            IUserNameResolver userNameResolver,
            IPushNotificationManager notifier,
            IBlobStorageProvider blobStorageProvider,
            CsvCouponImporter csvCouponImporter,
            ISecurityService securityService,
            IPermissionScopeService permissionScopeService)
        {
            _securityService = securityService;
            _promotionService = promotionService;
            _couponService = couponService;
            _promoEvaluator = promoEvaluator;
            _promoSearchService = promoSearchService;
            _userNameResolver = userNameResolver;
            _notifier = notifier;
            _blobStorageProvider = blobStorageProvider;
            _csvCouponImporter = csvCouponImporter;
            _permissionScopeService = permissionScopeService;
        }

        /// <summary>
        /// Search dynamic content places by given criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        [HttpPost]
        [Route("search")]
        [ResponseType(typeof(GenericSearchResult<DynamicPromotion>))]
        public IHttpActionResult PromotionsSearch(PromotionSearchCriteria criteria)
        {
            var retVal = new GenericSearchResult<Promotion>();
            //Scope bound ACL filtration
            criteria = FilterPromotionSearchCriteria(User.Identity.Name, criteria);

            var promoSearchResult = _promoSearchService.SearchPromotions(criteria);
            foreach (var promotion in promoSearchResult.Results.OfType<DynamicPromotion>())
            {
                promotion.PredicateVisualTreeSerialized = null;
                promotion.PredicateSerialized = null;
                promotion.RewardsSerialized = null;
            }

            retVal.TotalCount = promoSearchResult.TotalCount;
            retVal.Results = promoSearchResult.Results.ToList();
            return Ok(retVal);
        }

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <param name="context">Promotion evaluation context</param>
        [HttpPost]
        [ResponseType(typeof(webModel.PromotionReward[]))]
        [Route("evaluate")]
        public IHttpActionResult EvaluatePromotions(PromotionEvaluationContext context)
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
        [ResponseType(typeof(Promotion))]
        [Route("{id}")]
        public IHttpActionResult GetPromotionById(string id)
        {
            var retVal = _promotionService.GetPromotionsByIds(new[] { id }).FirstOrDefault();
            if (retVal != null)
            {
                var scopes = _permissionScopeService.GetObjectPermissionScopeStrings(retVal).ToArray();
                if (!_securityService.UserHasAnyPermission(User.Identity.Name, scopes, MarketingPredefinedPermissions.Read))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                return Ok(retVal);
            }
            return NotFound();
        }

        /// <summary>
        /// Get new dynamic promotion object 
        /// </summary>
        /// <remarks>Return a new dynamic promotion object with populated dynamic expression tree</remarks>
        [HttpGet]
        [ResponseType(typeof(Promotion))]
        [Route("new")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult GetNewDynamicPromotion()
        {
            var retVal = AbstractTypeFactory<DynamicPromotion>.TryCreateInstance();
            retVal.IsActive = true;
            return Ok(retVal);
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        [HttpPost]
        [ResponseType(typeof(Promotion))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Create)]
        public IHttpActionResult CreatePromotion(Promotion promotion)
        {
            var scopes = _permissionScopeService.GetObjectPermissionScopeStrings(promotion).ToArray();
            if (!_securityService.UserHasAnyPermission(User.Identity.Name, scopes, MarketingPredefinedPermissions.Create))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            _promotionService.SavePromotions(new[] { promotion });
            return GetPromotionById(promotion.Id);
        }


        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <param name="promotion">>dynamic promotion object that needs to be updated in the marketing system</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = MarketingPredefinedPermissions.Update)]
        public IHttpActionResult UpdatePromotions(Promotion promotion)
        {
            var scopes = _permissionScopeService.GetObjectPermissionScopeStrings(promotion).ToArray();
            if (!_securityService.UserHasAnyPermission(User.Identity.Name, scopes, MarketingPredefinedPermissions.Update))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            _promotionService.SavePromotions(new[] { promotion });
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
        [ResponseType(typeof(GenericSearchResult<Coupon>))]
        public IHttpActionResult SearchCoupons(CouponSearchCriteria criteria)
        {
            var searchResult = _couponService.SearchCoupons(criteria);

            return Ok(searchResult);
        }

        [HttpGet]
        [Route("coupons/{id}")]
        [ResponseType(typeof(Coupon))]
        public IHttpActionResult GetCoupon(string id)
        {
            var coupon = _couponService.GetByIds(new[] { id }).FirstOrDefault();

            return Ok(coupon);
        }

        [HttpPost]
        [Route("coupons/add")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddCoupons(Coupon[] coupons)
        {
            _couponService.SaveCoupons(coupons);

            return Ok();
        }

        [HttpDelete]
        [Route("coupons/delete")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteCoupons([FromUri] string[] ids)
        {
            _couponService.DeleteCoupons(ids);

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


        private PromotionSearchCriteria FilterPromotionSearchCriteria(string userName, PromotionSearchCriteria criteria)
        {
            if (!_securityService.UserHasAnyPermission(userName, null, MarketingPredefinedPermissions.Read))
            {
                //Get defined user 'read' permission scopes
                var readPermissionScopes = _securityService.GetUserPermissions(userName)
                                                      .Where(x => x.Id.StartsWith(MarketingPredefinedPermissions.Read))
                                                      .SelectMany(x => x.AssignedScopes)
                                                      .ToList();

                //Check user has a scopes
                //Store
                criteria.Store = readPermissionScopes.OfType<MarketingSelectedStoreScope>()
                                                         .Select(x => x.Scope)
                                                         .Where(x => !String.IsNullOrEmpty(x))
                                                         .FirstOrDefault();
            }
            return criteria;
        }
    }
}