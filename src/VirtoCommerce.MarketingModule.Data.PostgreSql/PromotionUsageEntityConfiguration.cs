using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.MarketingModule.Data.PostgreSql;

public class PromotionUsageEntityConfiguration : IEntityTypeConfiguration<PromotionUsageEntity>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<PromotionUsageEntity> builder)
    {
        // Configure the entity here
        builder.Property(x => x.CouponCode).UseCaseInsensitiveCollation();
    }
}
