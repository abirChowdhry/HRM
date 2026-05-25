using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class salaryAdditionNDeduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salaryAdditionNDeductions",
                columns: table => new
                {
                    IntSalaryAdditionAndDeductionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IntYear = table.Column<long>(type: "bigint", nullable: true),
                    IntMonth = table.Column<long>(type: "bigint", nullable: true),
                    IsAddition = table.Column<bool>(type: "bit", nullable: true),
                    IsDeduction = table.Column<bool>(type: "bit", nullable: true),
                    IntAdditionNdeductionTypeId = table.Column<long>(type: "bigint", nullable: true),
                    NumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StrCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryAdditionNDeductions", x => x.IntSalaryAdditionAndDeductionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaryAdditionNDeductions");
        }
    }
}
