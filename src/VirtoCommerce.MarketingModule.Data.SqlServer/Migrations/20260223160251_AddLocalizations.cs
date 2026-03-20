using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.MarketingModule.Data.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Promotion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PromotionLocalizedDescription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentEntityId = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionLocalizedDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionLocalizedDescription_Promotion_ParentEntityId",
                        column: x => x.ParentEntityId,
                        principalTable: "Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionLocalizedDisplayName",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentEntityId = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionLocalizedDisplayName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionLocalizedDisplayName_Promotion_ParentEntityId",
                        column: x => x.ParentEntityId,
                        principalTable: "Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedDescription_LanguageCode_ParentEntityId",
                table: "PromotionLocalizedDescription",
                columns: new[] { "LanguageCode", "ParentEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedDescription_ParentEntityId",
                table: "PromotionLocalizedDescription",
                column: "ParentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedDisplayName_LanguageCode_ParentEntityId",
                table: "PromotionLocalizedDisplayName",
                columns: new[] { "LanguageCode", "ParentEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedDisplayName_ParentEntityId",
                table: "PromotionLocalizedDisplayName",
                column: "ParentEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionLocalizedDescription");

            migrationBuilder.DropTable(
                name: "PromotionLocalizedDisplayName");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Promotion");
        }
    }
}
