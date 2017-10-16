namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsAllowCombiningWithSelf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotion", "IsAllowCombiningWithSelf", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotion", "IsAllowCombiningWithSelf");
        }
    }
}
