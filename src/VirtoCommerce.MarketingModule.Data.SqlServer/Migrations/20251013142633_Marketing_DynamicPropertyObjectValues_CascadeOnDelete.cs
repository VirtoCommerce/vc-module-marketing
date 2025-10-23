using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.MarketingModule.Data.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Marketing_DynamicPropertyObjectValues_CascadeOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicContentItemDynamicPropertyObjectValue_DynamicContentItem_ObjectId",
                table: "DynamicContentItemDynamicPropertyObjectValue");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicContentItemDynamicPropertyObjectValue_DynamicContentItem_ObjectId",
                table: "DynamicContentItemDynamicPropertyObjectValue",
                column: "ObjectId",
                principalTable: "DynamicContentItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicContentItemDynamicPropertyObjectValue_DynamicContentItem_ObjectId",
                table: "DynamicContentItemDynamicPropertyObjectValue");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicContentItemDynamicPropertyObjectValue_DynamicContentItem_ObjectId",
                table: "DynamicContentItemDynamicPropertyObjectValue",
                column: "ObjectId",
                principalTable: "DynamicContentItem",
                principalColumn: "Id");
        }
    }
}
