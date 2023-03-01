using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.MarketingModule.Data.SqlServer.Migrations
{
    public partial class RenameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_ObjectType_ObjectId",
                table: "DynamicContentItemDynamicPropertyObjectValue",
                newName: "IX_DynamicContentItemDynamicProperty_ObjectType_ObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_DynamicContentItemDynamicProperty_ObjectType_ObjectId",
                table: "DynamicContentItemDynamicPropertyObjectValue",
                newName: "IX_ObjectType_ObjectId");
        }
    }
}
