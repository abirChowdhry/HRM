using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class bonusSetupUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IntEmployementTypeId",
                table: "bonusSetups",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrBonusSetupName",
                table: "bonusSetups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntEmployementTypeId",
                table: "bonusSetups");

            migrationBuilder.DropColumn(
                name: "StrBonusSetupName",
                table: "bonusSetups");
        }
    }
}
