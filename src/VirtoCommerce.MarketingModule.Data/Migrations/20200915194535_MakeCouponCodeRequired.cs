using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.MarketingModule.Data.Migrations
{
    public partial class MakeCouponCodeRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodeAndPromotionId",
                table: "Coupon");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Coupon",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeAndPromotionId",
                table: "Coupon",
                columns: new[] { "Code", "PromotionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodeAndPromotionId",
                table: "Coupon");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Coupon",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.CreateIndex(
                name: "IX_CodeAndPromotionId",
                table: "Coupon",
                columns: new[] { "Code", "PromotionId" },
                unique: true,
                filter: "[Code] IS NOT NULL");
        }
    }
}
