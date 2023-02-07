﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VirtoCommerce.MarketingModule.Data.Repositories;

#nullable disable

namespace VirtoCommerce.MarketingModule.Data.PostgreSql.Migrations
{
    [DbContext(typeof(MarketingDbContext))]
    partial class MarketingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.CouponEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MaxUsesNumber")
                        .HasColumnType("integer");

                    b.Property<int>("MaxUsesPerUser")
                        .HasColumnType("integer");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OuterId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PromotionId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.HasIndex("Code", "PromotionId")
                        .IsUnique()
                        .HasDatabaseName("IX_CodeAndPromotionId");

                    b.ToTable("Coupon", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ParentFolderId")
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("DynamicContentFolder", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemDynamicPropertyObjectValueEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool?>("BooleanValue")
                        .HasColumnType("boolean");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeValue")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("DecimalValue")
                        .HasColumnType("numeric(18,5)");

                    b.Property<string>("DictionaryItemId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int?>("IntegerValue")
                        .HasColumnType("integer");

                    b.Property<string>("Locale")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("LongTextValue")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ObjectType")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PropertyId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PropertyName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("ShortTextValue")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("ObjectId");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasDatabaseName("IX_ObjectType_ObjectId");

                    b.ToTable("DynamicContentItemDynamicPropertyObjectValue", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ContentTypeId")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FolderId")
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<bool>("IsMultilingual")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("DynamicContentItem", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPlaceEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FolderId")
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("DynamicContentPlace", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPublishingGroupEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ConditionExpression")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("OuterId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PredicateVisualTreeSerialized")
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("StoreId")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.ToTable("DynamicContentPublishingGroup", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CatalogId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAllowCombiningWithSelf")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsExclusive")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("OuterId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int>("PerCustomerLimit")
                        .HasColumnType("integer");

                    b.Property<string>("PredicateSerialized")
                        .HasColumnType("text");

                    b.Property<string>("PredicateVisualTreeSerialized")
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<string>("RewardsSerialized")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("StoreId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int>("TotalLimit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Promotion", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionStoreEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PromotionId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.HasIndex("StoreId");

                    b.ToTable("PromotionStore", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionUsageEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CouponCode")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ObjectType")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("PromotionId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("UserId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("UserName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.ToTable("PromotionUsage", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PublishingGroupContentItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DynamicContentItemId")
                        .HasColumnType("character varying(128)");

                    b.Property<string>("DynamicContentPublishingGroupId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DynamicContentItemId");

                    b.HasIndex("DynamicContentPublishingGroupId");

                    b.ToTable("PublishingGroupContentItem", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PublishingGroupContentPlaceEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DynamicContentPlaceId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("DynamicContentPublishingGroupId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DynamicContentPlaceId");

                    b.HasIndex("DynamicContentPublishingGroupId");

                    b.ToTable("PublishingGroupContentPlace", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.CouponEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", "ParentFolder")
                        .WithMany()
                        .HasForeignKey("ParentFolderId");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemDynamicPropertyObjectValueEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", "DynamicContentItem")
                        .WithMany("DynamicPropertyObjectValues")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("DynamicContentItem");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", "Folder")
                        .WithMany("ContentItems")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPlaceEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", "Folder")
                        .WithMany("ContentPlaces")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionStoreEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", "Promotion")
                        .WithMany("Stores")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionUsageEntity", b =>
                {
                    b.HasOne("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Promotion");
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

                    b.Navigation("ContentItem");

                    b.Navigation("PublishingGroup");
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

                    b.Navigation("ContentPlace");

                    b.Navigation("PublishingGroup");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentFolderEntity", b =>
                {
                    b.Navigation("ContentItems");

                    b.Navigation("ContentPlaces");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentItemEntity", b =>
                {
                    b.Navigation("DynamicPropertyObjectValues");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.DynamicContentPublishingGroupEntity", b =>
                {
                    b.Navigation("ContentItems");

                    b.Navigation("ContentPlaces");
                });

            modelBuilder.Entity("VirtoCommerce.MarketingModule.Data.Model.PromotionEntity", b =>
                {
                    b.Navigation("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
