namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaxUsesPerUserToCoupon_AddUserIdToPromotionUsage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupon", "MaxUsesPerUser", c => c.Int(nullable: false));
            AddColumn("dbo.PromotionUsage", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.PromotionUsage", "UserName", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromotionUsage", "UserName");
            DropColumn("dbo.PromotionUsage", "UserId");
            DropColumn("dbo.Coupon", "MaxUsesPerUser");
        }
    }
}
