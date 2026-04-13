using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.MarketingModule.Data.PostgreSql;

public class DynamicContentPlaceEntityConfiguration : IEntityTypeConfiguration<DynamicContentPlaceEntity>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DynamicContentPlaceEntity> builder)
    {
        // Configure the entity here
        builder.Property(x => x.Name).UseCaseInsensitiveCollation();
    }
}
