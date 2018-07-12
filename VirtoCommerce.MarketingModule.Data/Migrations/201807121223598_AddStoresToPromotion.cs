namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoresToPromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotionStore",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PromotionId = c.String(nullable: false, maxLength: 128),
                        StoreId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.PromotionId)
                .Index(t => t.StoreId);

            Sql("INSERT INTO dbo.PromotionStore SELECT Id, Id, StoreId FROM dbo.Promotion WHERE StoreId is not null");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionStore", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.PromotionStore", new[] { "StoreId" });
            DropIndex("dbo.PromotionStore", new[] { "PromotionId" });
            DropTable("dbo.PromotionStore");
        }
    }
}
