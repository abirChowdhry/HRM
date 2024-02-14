using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class empTransferNPromotionUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntNewBusinessUnitId",
                table: "empTransferNPromotions");

            migrationBuilder.DropColumn(
                name: "IntNewDepartmentId",
                table: "empTransferNPromotions");

            migrationBuilder.RenameColumn(
                name: "IntOldBusinessUnitId",
                table: "empTransferNPromotions",
                newName: "IntNewDesignationId");

            migrationBuilder.AddColumn<long>(
                name: "IntBusinessUnitId",
                table: "empTransferNPromotions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntBusinessUnitId",
                table: "empTransferNPromotions");

            migrationBuilder.RenameColumn(
                name: "IntNewDesignationId",
                table: "empTransferNPromotions",
                newName: "IntOldBusinessUnitId");

            migrationBuilder.AddColumn<long>(
                name: "IntNewBusinessUnitId",
                table: "empTransferNPromotions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntNewDepartmentId",
                table: "empTransferNPromotions",
                type: "bigint",
                nullable: true);
        }
    }
}
