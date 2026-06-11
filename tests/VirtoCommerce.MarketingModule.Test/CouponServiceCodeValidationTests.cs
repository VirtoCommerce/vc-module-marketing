using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.MarketingModule.Core;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test;

public class CouponServiceCodeValidationTests
{
    private readonly Mock<IMarketingRepository> _repositoryMock = new();
    private readonly Mock<ISettingsManager> _settingsManagerMock = new();

    [Theory]
    [InlineData("SUMMER2026")]
    [InlineData("sale10")]
    public async Task SaveChangesAsync_CodeMatchesDefaultPattern_Saves(string code)
    {
        // Arrange
        var service = GetCouponService();

        // Act
        var action = () => service.SaveChangesAsync([new Coupon { Code = code, PromotionId = "promotion-id" }]);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData("SUMMER_2026")]
    [InlineData("sale 10")]
    [InlineData("promo!")]
    public async Task SaveChangesAsync_CodeViolatesPattern_Throws(string code)
    {
        // Arrange
        var service = GetCouponService();

        // Act
        var action = () => service.SaveChangesAsync([new Coupon { Code = code, PromotionId = "promotion-id" }]);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage($"*{code}*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SaveChangesAsync_EmptyPattern_SkipsValidation(string pattern)
    {
        // Arrange
        var service = GetCouponService(pattern);

        // Act
        var action = () => service.SaveChangesAsync([new Coupon { Code = "any code is valid !@#", PromotionId = "promotion-id" }]);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveChangesAsync_CustomPattern_ValidatesAgainstIt()
    {
        // Arrange
        var service = GetCouponService(@"^[A-Z]{3}-\d{4}$");

        // Act
        var validAction = () => service.SaveChangesAsync([new Coupon { Code = "ABC-2026", PromotionId = "promotion-id" }]);
        var invalidAction = () => service.SaveChangesAsync([new Coupon { Code = "ABC2026", PromotionId = "promotion-id" }]);

        // Assert
        await validAction.Should().NotThrowAsync();
        await invalidAction.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SaveChangesAsync_InvalidRegexPattern_ThrowsMeaningfulError()
    {
        // Arrange
        var service = GetCouponService("[unclosed");

        // Act
        var action = () => service.SaveChangesAsync([new Coupon { Code = "SUMMER2026", PromotionId = "promotion-id" }]);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{ModuleConstants.Settings.General.CouponCodeValidationPattern.Name}*");
    }

    private CouponService GetCouponService(string codeValidationPattern = null)
    {
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), Mock.Of<ILogger<PlatformMemoryCache>>());

        _repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(Mock.Of<IUnitOfWork>());

        var descriptor = ModuleConstants.Settings.General.CouponCodeValidationPattern;
        _settingsManagerMock
            .Setup(x => x.GetObjectSettingAsync(descriptor.Name, null, null))
            .ReturnsAsync(new ObjectSettingEntry(descriptor) { Value = codeValidationPattern ?? descriptor.DefaultValue });

        return new CouponService(
            () => _repositoryMock.Object,
            platformMemoryCache,
            Mock.Of<IEventPublisher>(),
            _settingsManagerMock.Object);
    }
}
