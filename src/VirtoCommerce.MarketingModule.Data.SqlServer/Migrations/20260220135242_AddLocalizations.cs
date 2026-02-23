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
                name: "PromotionLocalizedDescriptionEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentEntityId = table.Column<string>(type: "nvarchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionLocalizedDescriptionEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionLocalizedDescriptionEntity_Promotion_ParentEntityId",
                        column: x => x.ParentEntityId,
                        principalTable: "Promotion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PromotionLocalizedDisplayNameEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentEntityId = table.Column<string>(type: "nvarchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionLocalizedDisplayNameEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionLocalizedDisplayNameEntity_Promotion_ParentEntityId",
                        column: x => x.ParentEntityId,
                        principalTable: "Promotion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedDescriptionEntity_ParentEntityId",
                table: "PromotionLocalizedDescriptionEntity",
                column: "ParentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionLocalizedDisplayNameEntity_ParentEntityId",
                table: "PromotionLocalizedDisplayNameEntity",
                column: "ParentEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionLocalizedDescriptionEntity");

            migrationBuilder.DropTable(
                name: "PromotionLocalizedDisplayNameEntity");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Promotion");
        }
    }
}
