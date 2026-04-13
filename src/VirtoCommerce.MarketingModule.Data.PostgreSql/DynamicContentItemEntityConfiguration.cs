using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.MarketingModule.Data.PostgreSql;

public class DynamicContentItemEntityConfiguration : IEntityTypeConfiguration<DynamicContentItemEntity>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DynamicContentItemEntity> builder)
    {
        // Configure the entity here
        builder.Property(x => x.Name).UseCaseInsensitiveCollation();
    }
}
