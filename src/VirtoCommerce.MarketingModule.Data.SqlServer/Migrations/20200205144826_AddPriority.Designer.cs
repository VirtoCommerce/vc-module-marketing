// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtoCommerce.MarketingModule.Data.Repositories;

namespace VirtoCommerce.MarketingModule.Data.SqlServer.Migrations
{
    [DbContext(typeof(MarketingDbContext))]
    [Migration("20200205144826_AddPriority")]
    partial class AddPriority
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.CouponEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxUsesNumber")
                        .HasColumnType("int");

                    b.Property<int>("MaxUsesPerUser")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OuterId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PromotionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.HasIndex("Code", "PromotionId")
                        .IsUnique()
                        .HasName("IX_CodeAndPromotionId")
                        .HasFilter("[Code] IS NOT NULL");

                    b.ToTable("Coupon");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ParentFolderId")
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("DynamicContentFolder");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemDynamicPropertyObjectValueEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<bool?>("BooleanValue")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTimeValue")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("DecimalValue")
                        .HasColumnType("decimal(18,5)");

                    b.Property<string>("DictionaryItemId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int?>("IntegerValue")
                        .HasColumnType("int");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("LongTextValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ObjectType")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PropertyName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ShortTextValue")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("ObjectId");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasName("IX_ObjectType_ObjectId");

                    b.ToTable("DynamicContentItemDynamicPropertyObjectValue");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ContentTypeId")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("FolderId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<bool>("IsMultilingual")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("DynamicContentItem");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPlaceEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("FolderId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("DynamicContentPlace");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPublishingGroupEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ConditionExpression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("OuterId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PredicateVisualTreeSerialized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StoreId")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("DynamicContentPublishingGroup");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CatalogId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAllowCombiningWithSelf")
                        .HasColumnType("bit");

                    b.Property<bool>("IsExclusive")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("OuterId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("PerCustomerLimit")
                        .HasColumnType("int");

                    b.Property<string>("PredicateSerialized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PredicateVisualTreeSerialized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("RewardsSerialized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StoreId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<int>("TotalLimit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Promotion");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionStoreEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PromotionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.HasIndex("StoreId");

                    b.ToTable("PromotionStore");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionUsageEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CouponCode")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ObjectType")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PromotionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.ToTable("PromotionUsage");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PublishingGroupContentItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DynamicContentItemId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("DynamicContentPublishingGroupId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DynamicContentItemId");

                    b.HasIndex("DynamicContentPublishingGroupId");

                    b.ToTable("PublishingGroupContentItem");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PublishingGroupContentPlaceEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DynamicContentPlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("DynamicContentPublishingGroupId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DynamicContentPlaceId");

                    b.HasIndex("DynamicContentPublishingGroupId");

                    b.ToTable("PublishingGroupContentPlace");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.CouponEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", "ParentFolder")
                        .WithMany()
                        .HasForeignKey("ParentFolderId");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemDynamicPropertyObjectValueEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", "DynamicContentItem")
                        .WithMany("DynamicPropertyObjectValues")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", "Folder")
                        .WithMany("ContentItems")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPlaceEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", "Folder")
                        .WithMany("ContentPlaces")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionStoreEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", "Promotion")
                        .WithMany("Stores")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionUsageEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PublishingGroupContentItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", "ContentItem")
                        .WithMany()
                        .HasForeignKey("DynamicContentItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPublishingGroupEntity", "PublishingGroup")
                        .WithMany("ContentItems")
                        .HasForeignKey("DynamicContentPublishingGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PublishingGroupContentPlaceEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPlaceEntity", "ContentPlace")
                        .WithMany()
                        .HasForeignKey("DynamicContentPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPublishingGroupEntity", "PublishingGroup")
                        .WithMany("ContentPlaces")
                        .HasForeignKey("DynamicContentPublishingGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
