using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.MarketingModule.Data.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionLabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromotionLocalizedLabel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LanguageCode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentEntityId = table.Column<string>(type: "varchar(128)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionLocalizedLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionLocalizedLabel_Promotion_ParentEntityId",
                        column: x => x.ParentEntityId,
                        principalTable: "Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedLabel_LanguageCode_ParentEntityId",
                table: "PromotionLocalizedLabel",
                columns: new[] { "LanguageCode", "ParentEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedLabel_ParentEntityId",
                table: "PromotionLocalizedLabel",
                column: "ParentEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionLocalizedLabel");
        }
    }
}
