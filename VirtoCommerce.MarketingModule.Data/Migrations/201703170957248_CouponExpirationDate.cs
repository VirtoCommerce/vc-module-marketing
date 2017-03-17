namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponExpirationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupon", "ExpirationDate", c => c.DateTime());
            DropColumn("dbo.Coupon", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupon", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Coupon", "ExpirationDate");
        }
    }
}
