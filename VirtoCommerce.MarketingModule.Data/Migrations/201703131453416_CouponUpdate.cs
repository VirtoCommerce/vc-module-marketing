namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CouponUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupon", "MaxUsesNumber", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Coupon", "MaxUsesNumber");
        }
    }
}
