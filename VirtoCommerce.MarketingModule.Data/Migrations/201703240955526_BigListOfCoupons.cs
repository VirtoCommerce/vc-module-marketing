namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BigListOfCoupons : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM dbo.Coupon WHERE PromotionId is NULL");
            Sql("DELETE FROM dbo.PublishingGroupContentPlace WHERE DynamicContentPublishingGroupId is NULL");
            Sql("DELETE FROM dbo.PublishingGroupContentItem WHERE DynamicContentPublishingGroupId is NULL");


            DropForeignKey("dbo.PromotionUsage", "PromotionId", "dbo.Promotion");
            DropTable("dbo.PromotionUsage");
            CreateTable(
              "dbo.PromotionUsage",
              c => new
              {
                  Id = c.String(nullable: false, maxLength: 128),
                  ObjectId = c.String(maxLength: 128),
                  ObjectType = c.String(maxLength: 128),
                  CouponCode = c.String(maxLength: 64),
                  PromotionId = c.String(nullable: false, maxLength: 128),
                  CreatedDate = c.DateTime(nullable: false),
                  ModifiedDate = c.DateTime(),
                  CreatedBy = c.String(maxLength: 64),
                  ModifiedBy = c.String(maxLength: 64),
              })
              .PrimaryKey(t => t.Id)
              .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true);

            AddColumn("dbo.Coupon", "MaxUsesNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Coupon", "ExpirationDate", c => c.DateTime());
            DropForeignKey("dbo.Coupon", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.Coupon", new[] { "PromotionId" });
            AlterColumn("dbo.Coupon", "PromotionId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Coupon", new[] { "Code", "PromotionId" }, unique: true, name: "IX_CodeAndPromotionId");         
            AddForeignKey("dbo.Coupon", "PromotionId", "dbo.Promotion", "Id", cascadeDelete: true);

            DropColumn("dbo.Promotion", "CouponCode");

            DropForeignKey("dbo.DynamicContentItem", "FolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.DynamicContentPlace", "FolderId", "dbo.DynamicContentFolder");
            DropForeignKey("dbo.PublishingGroupContentItem", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup");
            DropForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup");          
            DropIndex("dbo.PublishingGroupContentItem", new[] { "DynamicContentPublishingGroupId" });
            DropIndex("dbo.PublishingGroupContentPlace", new[] { "DynamicContentPublishingGroupId" });      
            AlterColumn("dbo.PublishingGroupContentItem", "DynamicContentPublishingGroupId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PublishingGroupContentPlace", "DynamicContentPublishingGroupId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.PublishingGroupContentItem", "DynamicContentPublishingGroupId");
            CreateIndex("dbo.PublishingGroupContentPlace", "DynamicContentPublishingGroupId");         
            AddForeignKey("dbo.DynamicContentItem", "FolderId", "dbo.DynamicContentFolder", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DynamicContentPlace", "FolderId", "dbo.DynamicContentFolder", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PublishingGroupContentItem", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PublishingGroupContentPlace", "DynamicContentPublishingGroupId", "dbo.DynamicContentPublishingGroup", "Id", cascadeDelete: true);
        
        }
        
        public override void Down()
        {
            
        }
    }
}
