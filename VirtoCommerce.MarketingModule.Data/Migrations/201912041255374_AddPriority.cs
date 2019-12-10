namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublishingGroupContentItem", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PublishingGroupContentItem", "Priority");
        }
    }
}
