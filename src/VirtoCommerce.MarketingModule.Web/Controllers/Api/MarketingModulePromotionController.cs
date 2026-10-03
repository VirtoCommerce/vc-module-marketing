using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Model.PushNotifications;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Authorization;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Web.Authorization;
using VirtoCommerce.MarketingModule.Web.BackgroundJobs;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using Permissions = VirtoCommerce.MarketingModule.Core.ModuleConstants.Security.Permissions;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Controllers.Api;

[Route("api/marketing/promotions")]
[Authorize]
public class MarketingModulePromotionController(
    IPromotionService promotionService,
    ICouponService couponService,
    IMarketingPromoEvaluator promoEvaluator,
    IPromotionSearchService promoSearchService,
    IUserNameResolver userNameResolver,
    IPushNotificationManager notifier,
    Func<IMarketingRepository> repositoryFactory,
    ICouponSearchService couponSearchService,
    IAuthorizationService authorizationService)
    : Controller
{
    /// <summary>
    /// Search dynamic content places by given criteria
    /// </summary>
    /// <param name="criteria">criteria</param>
    [HttpPost]
    [Route("search")]
    public async Task<ActionResult<PromotionSearchResult>> PromotionsSearch([FromBody] PromotionSearchCriteria criteria)
    {
        //Scope bound ACL filtration
        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, criteria, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: true));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var searchResult = await promoSearchService.SearchNoCloneAsync(criteria);

        return Ok(searchResult);
    }

    /// <summary>
    /// Evaluate promotions
    /// </summary>
    /// <param name="context">Promotion evaluation context</param>
    [HttpPost]
    [Route("evaluate")]
    public async Task<ActionResult<webModel.PromotionReward[]>> EvaluatePromotions([FromBody] PromotionEvaluationContext context)
    {
        var promotionResult = await promoEvaluator.EvaluatePromotionAsync(context);

        //This dynamic casting is used here for duck-type casting class hierarchy into flat type
        //because OpenAPI and code generation like AutoRest tools don't work with inheritances
        var result = promotionResult.Rewards.Select(dynamic (x) => x).ToArray();

        return Ok(result);
    }

    /// <summary>
    /// Find promotion object by id
    /// </summary>
    /// <remarks>Return a single promotion (dynamic or custom) object </remarks>
    /// <param name="id">promotion id</param>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Promotion>> GetPromotionById(string id)
    {
        var promotion = await promotionService.GetByIdAsync(id);
        if (promotion is null)
        {
            return NotFound();
        }

        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, promotion, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: false));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        if (promotion is DynamicPromotion dynamicPromotion)
        {
            dynamicPromotion.DynamicExpression?.MergeFromPrototype(AbstractTypeFactory<PromotionConditionAndRewardTreePrototype>.TryCreateInstance());
        }

        return Ok(promotion);
    }

    /// <summary>
    /// Get new dynamic promotion object 
    /// </summary>
    /// <remarks>Return a new dynamic promotion object with populated dynamic expression tree</remarks>
    [HttpGet]
    [Route("new")]
    [Authorize(Permissions.Create)]
    public ActionResult<Promotion> GetNewDynamicPromotion()
    {
        var promotion = AbstractTypeFactory<Promotion>.TryCreateInstance();
        promotion.IsActive = true;

        if (promotion is DynamicPromotion dynamicPromotion)
        {
            dynamicPromotion.DynamicExpression = AbstractTypeFactory<PromotionConditionAndRewardTree>.TryCreateInstance();
            dynamicPromotion.DynamicExpression.MergeFromPrototype(AbstractTypeFactory<PromotionConditionAndRewardTreePrototype>.TryCreateInstance());
        }

        return Ok(promotion);
    }

    /// <summary>
    /// Add new dynamic promotion object to marketing system
    /// </summary>
    /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
    [HttpPost]
    [Route("")]
    [Authorize(Permissions.Create)]
    public async Task<ActionResult<Promotion>> CreatePromotion([FromBody] Promotion promotion)
    {
        await promotionService.SaveChangesAsync([promotion]);

        return await GetPromotionById(promotion.Id);
    }

    /// <summary>
    /// Update an existing dynamic promotion object in marketing system
    /// </summary>
    /// <param name="promotion">>dynamic promotion object that needs to be updated in the marketing system</param>
    [HttpPut]
    [Route("")]
    [Authorize(Permissions.Update)]
    public async Task<ActionResult> UpdatePromotions([FromBody] Promotion promotion)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, promotion, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: true));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        await promotionService.SaveChangesAsync([promotion]);

        return NoContent();
    }

    /// <summary>
    ///  Delete promotions objects
    /// </summary>
    /// <param name="ids">promotions object ids for delete in the marketing system</param>
    [HttpDelete]
    [Route("")]
    [Authorize(Permissions.Delete)]
    public async Task<ActionResult> DeletePromotions([FromQuery] string[] ids)
    {
        var permissionResource = new PermissionResourceModel { PromotionIds = ids };
        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, permissionResource, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: true));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        await promotionService.DeleteAsync(ids);

        return NoContent();
    }

    [HttpPost]
    [Route("coupons/search")]
    public async Task<ActionResult<CouponSearchResult>> SearchCoupons([FromBody] CouponSearchCriteria criteria)
    {
        var searchResult = await couponSearchService.SearchAsync(criteria);

        // Actualize TotalUsage field 
        using (var repository = repositoryFactory())
        {
            var ids = searchResult.Results.Select(x => x.Id).ToArray();
            var couponEntities = await repository.GetCouponsByIdsAsync(ids);

            foreach (var coupon in searchResult.Results)
            {
                coupon.TotalUsesCount = couponEntities.First(x => x.Id == coupon.Id).TotalUsesCount;
            }
        }

        return Ok(searchResult);
    }

    [HttpGet]
    [Route("coupons/{id}")]
    public async Task<ActionResult<Coupon>> GetCoupon(string id)
    {
        var coupon = await couponService.GetNoCloneAsync(id);

        return Ok(coupon);
    }

    [HttpPost]
    [Route("coupons/add")]
    [Authorize(Permissions.Update)]
    public async Task<ActionResult> AddCoupons([FromBody] Coupon[] coupons)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, coupons, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: true));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        await couponService.SaveChangesAsync(coupons);

        return NoContent();
    }

    [HttpDelete]
    [Route("coupons/delete")]
    [Authorize(Permissions.Delete)]
    public async Task<ActionResult> DeleteCoupons([FromQuery] string[] ids)
    {
        var permissionResource = new PermissionResourceModel { CouponIds = ids };
        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, permissionResource, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: true));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        await couponService.DeleteAsync(ids);

        return NoContent();
    }

    [HttpPost]
    [Route("coupons/import")]
    [Authorize(Permissions.Update)]
    public async Task<ActionResult<ImportNotification>> ImportCouponsAsync([FromBody] ImportRequest request)
    {
        var permissionResource = new PermissionResourceModel { PromotionIds = [request.PromotionId] };
        var authorizationResult = await authorizationService.AuthorizeAsync(
            User, permissionResource, new MarketingAuthorizationRequirement(Permissions.Read, checkAllScopes: true));
        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var notification = new ImportNotification(userNameResolver.GetCurrentUserName())
        {
            Title = "Import coupons from CSV file",
            Description = "Starting import...",
        };

        await notifier.SendAsync(notification);

        await EnqueueImport(request, notification);

        return Ok(notification);
    }

    /// <summary>
    /// Kept for background jobs enqueued by an earlier version, which reference this method by name.
    /// Hands the work to <see cref="ImportCouponsJob"/>; remove this once no such job can still be pending.
    /// </summary>
    // Signature is byte-identical on purpose: Hangfire persists a queued job as type name + method name +
    // parameter types + serialized args, so changing any of them would strand already-queued entries as Failed.
    [ApiExplorerSettings(IgnoreApi = true)]
    [Obsolete("Enqueued indirectly by legacy Hangfire jobs only; new work uses ImportCouponsJob.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task BackgroundImportAsync(ImportRequest request, ImportNotification notification)
    {
        return EnqueueImport(request, notification);
    }

    // The static facade rather than an injected IBackgroundJob avoids another constructor parameter, and it also works
    // when Hangfire activates this controller outside a request to run a legacy job.
    private static Task EnqueueImport(ImportRequest request, ImportNotification notification)
    {
        var payload = AbstractTypeFactory<ImportCouponsJobPayload>.TryCreateInstance();
        payload.Request = request;
        payload.Notification = notification;

        return BackgroundJob.Enqueue<ImportCouponsJob>(payload);
    }
}
