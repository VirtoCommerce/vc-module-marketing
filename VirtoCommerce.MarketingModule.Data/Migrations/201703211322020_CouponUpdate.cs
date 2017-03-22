namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupon", "MaxUsesNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Coupon", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coupon", "ExpirationDate");
            DropColumn("dbo.Coupon", "MaxUsesNumber");
        }
    }
}
