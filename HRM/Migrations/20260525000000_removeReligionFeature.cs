using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    [Migration("20260525000000_removeReligionFeature")]
    public partial class removeReligionFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StrReligion",
                table: "empBasicInfos");

            migrationBuilder.DropColumn(
                name: "StrReligion",
                table: "bonusSetups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StrReligion",
                table: "empBasicInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrReligion",
                table: "bonusSetups",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
