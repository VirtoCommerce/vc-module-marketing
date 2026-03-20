using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.MarketingModule.Data.PostgreSql.Migrations
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
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ParentEntityId = table.Column<string>(type: "character varying(128)", nullable: false)
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
                });

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
