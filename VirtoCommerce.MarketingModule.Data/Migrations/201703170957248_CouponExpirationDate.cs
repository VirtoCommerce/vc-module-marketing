namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CouponExpirationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupon", "ExpirationDate", c => c.DateTime());
        }

        public override void Down()
        {
            AddColumn("dbo.Coupon", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
