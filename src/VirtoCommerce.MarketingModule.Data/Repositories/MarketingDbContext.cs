using System.Reflection;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
    public class MarketingDbContext : DbContextBase
    {
        public MarketingDbContext(DbContextOptions<MarketingDbContext> options)
            : base(options)
        {
        }

        protected MarketingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Coupon

            modelBuilder.Entity<CouponEntity>().ToTable("Coupon");
            modelBuilder.Entity<CouponEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<CouponEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<CouponEntity>().HasOne(x => x.Promotion).WithMany()
                .HasForeignKey(x => x.PromotionId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            modelBuilder.Entity<CouponEntity>().HasIndex(i => new { i.Code, i.PromotionId }).IsUnique().HasDatabaseName("IX_CodeAndPromotionId");

            #endregion

            #region Promotion

            modelBuilder.Entity<PromotionEntity>().ToTable("Promotion");
            modelBuilder.Entity<PromotionEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<PromotionEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

            #endregion

            #region PromotionUsage

            modelBuilder.Entity<PromotionUsageEntity>().ToTable("PromotionUsage");
            modelBuilder.Entity<PromotionUsageEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<PromotionUsageEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<PromotionUsageEntity>().HasOne(x => x.Promotion).WithMany()
                .HasForeignKey(x => x.PromotionId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            #endregion

            #region DynamicContentItem

            modelBuilder.Entity<DynamicContentItemEntity>().ToTable("DynamicContentItem");
            modelBuilder.Entity<DynamicContentItemEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<DynamicContentItemEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicContentItemEntity>().HasOne(x => x.Folder).WithMany(x => x.ContentItems)
                .HasForeignKey(x => x.FolderId).OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region DynamicContentPlace

            modelBuilder.Entity<DynamicContentPlaceEntity>().ToTable("DynamicContentPlace");
            modelBuilder.Entity<DynamicContentPlaceEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<DynamicContentPlaceEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicContentPlaceEntity>().HasOne(x => x.Folder).WithMany(x => x.ContentPlaces)
                .HasForeignKey(x => x.FolderId).OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region DynamicContentPublishingGroup

            modelBuilder.Entity<DynamicContentPublishingGroupEntity>().ToTable("DynamicContentPublishingGroup");
            modelBuilder.Entity<DynamicContentPublishingGroupEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<DynamicContentPublishingGroupEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

            #endregion

            #region PublishingGroupContentItem

            modelBuilder.Entity<PublishingGroupContentItemEntity>().ToTable("PublishingGroupContentItem");
            modelBuilder.Entity<PublishingGroupContentItemEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<PublishingGroupContentItemEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<PublishingGroupContentItemEntity>().HasOne(p => p.ContentItem).WithMany()
                .HasForeignKey(x => x.DynamicContentItemId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PublishingGroupContentItemEntity>().HasOne(p => p.PublishingGroup)
                .WithMany(x => x.ContentItems).HasForeignKey(x => x.DynamicContentPublishingGroupId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            #endregion

            #region PublishingGroupContentPlace

            modelBuilder.Entity<PublishingGroupContentPlaceEntity>().ToTable("PublishingGroupContentPlace");
            modelBuilder.Entity<PublishingGroupContentPlaceEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<PublishingGroupContentPlaceEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<PublishingGroupContentPlaceEntity>().HasOne(p => p.ContentPlace).WithMany()
                .HasForeignKey(x => x.DynamicContentPlaceId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
            modelBuilder.Entity<PublishingGroupContentPlaceEntity>().HasOne(p => p.PublishingGroup)
                .WithMany(x => x.ContentPlaces).HasForeignKey(x => x.DynamicContentPublishingGroupId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            #endregion

            #region DynamicContentFolder

            modelBuilder.Entity<DynamicContentFolderEntity>().ToTable("DynamicContentFolder");
            modelBuilder.Entity<DynamicContentFolderEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<DynamicContentFolderEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

            #endregion

            #region PromotionStore

            modelBuilder.Entity<PromotionStoreEntity>().ToTable("PromotionStore");
            modelBuilder.Entity<PromotionStoreEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<PromotionStoreEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<PromotionStoreEntity>().HasOne(x => x.Promotion)
                .WithMany(x => x.Stores).HasForeignKey(x => x.PromotionId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
            modelBuilder.Entity<PromotionStoreEntity>().HasIndex(i => i.StoreId);

            #endregion

            #region DynamicProperty

            modelBuilder.Entity<DynamicContentItemDynamicPropertyObjectValueEntity>().ToTable("DynamicContentItemDynamicPropertyObjectValue").HasKey(x => x.Id);
            modelBuilder.Entity<DynamicContentItemDynamicPropertyObjectValueEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicContentItemDynamicPropertyObjectValueEntity>().Property(x => x.DecimalValue).HasColumnType("decimal(18,5)");
            modelBuilder.Entity<DynamicContentItemDynamicPropertyObjectValueEntity>().HasOne(p => p.DynamicContentItem)
                .WithMany(s => s.DynamicPropertyObjectValues).HasForeignKey(k => k.ObjectId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicContentItemDynamicPropertyObjectValueEntity>().HasIndex(x => new { x.ObjectType, x.ObjectId })
                .IsUnique(false)
                .HasDatabaseName("IX_DynamicContentItemDynamicProperty_ObjectType_ObjectId");

            #endregion

            base.OnModelCreating(modelBuilder);


            // Allows configuration for an entity type for different database types.
            // Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" in VirtoCommerce.MarketingModule.Data.XXX project. /> 
            switch (this.Database.ProviderName)
            {
                case "Pomelo.EntityFrameworkCore.MySql":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.MarketingModule.Data.MySql"));
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.MarketingModule.Data.PostgreSql"));
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.MarketingModule.Data.SqlServer"));
                    break;
            }
        }
    }
}
