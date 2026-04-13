using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.MarketingModule.Data.PostgreSql;

public class CouponEntityConfiguration : IEntityTypeConfiguration<CouponEntity>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CouponEntity> builder)
    {
        // Configure the entity here
        builder.Property(x => x.Code).UseCaseInsensitiveCollation();
    }
}
