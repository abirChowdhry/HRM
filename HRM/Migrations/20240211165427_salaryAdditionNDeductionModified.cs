using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class salaryAdditionNDeductionModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StrCreatedBy",
                table: "salaryAdditionNDeductions");

            migrationBuilder.AddColumn<long>(
                name: "IntCreatedBy",
                table: "salaryAdditionNDeductions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntCreatedBy",
                table: "salaryAdditionNDeductions");

            migrationBuilder.AddColumn<string>(
                name: "StrCreatedBy",
                table: "salaryAdditionNDeductions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
