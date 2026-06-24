using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class CouponService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher,
    ISettingsManager settingsManager)
    : CrudService<Coupon, CouponEntity, CouponChangingEvent, CouponChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        ICouponService
{
    private static readonly TimeSpan _regexMatchTimeout = TimeSpan.FromSeconds(1);

    [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task<Coupon[]> GetByIdsAsync(string[] ids)
    {
        return (await GetAsync(ids)).ToArray();
    }

    [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task SaveCouponsAsync(Coupon[] coupons)
    {
        return SaveChangesAsync(coupons);
    }

    [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task DeleteCouponsAsync(string[] ids)
    {
        return DeleteAsync(ids);
    }


    protected override async Task BeforeSaveChanges(IList<Coupon> models)
    {
        await base.BeforeSaveChanges(models);

        if (models.Any(x => x.Code.IsNullOrEmpty()))
        {
            throw new InvalidOperationException("Coupon cannot have empty code.");
        }

        await ValidateCouponCodesAsync(models);

        using var repository = repositoryFactory();
        var nonUniqueCouponErrors = await repository.CheckCouponsForUniquenessAsync(models.Where(x => x.IsTransient()).ToArray());

        if (!nonUniqueCouponErrors.IsNullOrEmpty())
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, nonUniqueCouponErrors));
        }
    }

    protected override Task<IList<CouponEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetCouponsByIdsAsync(ids);
    }

    protected override void ClearCache(IList<Coupon> models)
    {
        base.ClearCache(models);
        ClearPromotionCache(models);
    }

    protected override void ClearSearchCache(IList<Coupon> models)
    {
        base.ClearSearchCache(models);
        ClearPromotionSearchCache();
    }

    private async Task ValidateCouponCodesAsync(IList<Coupon> models)
    {
        var pattern = await settingsManager.GetValueAsync<string>(ModuleConstants.Settings.General.CouponCodeValidationPattern);
        if (string.IsNullOrWhiteSpace(pattern))
        {
            return;
        }

        Regex regex;
        try
        {
            regex = new Regex(pattern, RegexOptions.None, _regexMatchTimeout);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(
                $"Coupon code validation is misconfigured: '{pattern}' is not a valid regular expression. " +
                $"Fix or clear the '{ModuleConstants.Settings.General.CouponCodeValidationPattern.Name}' setting.", ex);
        }

        var invalidCodes = models
            .Select(x => x.Code)
            .Where(code => !IsMatch(regex, code))
            .Distinct()
            .ToArray();

        if (invalidCodes.Length > 0)
        {
            throw new InvalidOperationException(
                $"Coupon codes do not match the validation pattern '{pattern}': {string.Join(", ", invalidCodes)}");
        }
    }

    private static bool IsMatch(Regex regex, string code)
    {
        try
        {
            return regex.IsMatch(code);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private static void ClearPromotionCache(IList<Coupon> models)
    {
        foreach (var promotionId in models.Select(x => x.PromotionId))
        {
            GenericCachingRegion<Promotion>.ExpireTokenForKey(promotionId);
        }
    }

    private static void ClearPromotionSearchCache()
    {
        GenericSearchCachingRegion<Promotion>.ExpireRegion();
    }
}
