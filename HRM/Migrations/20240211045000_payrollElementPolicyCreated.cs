using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class payrollElementPolicyCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payrollElements",
                columns: table => new
                {
                    IntPayrollElementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrPayrollElementName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsBasicElement = table.Column<bool>(type: "bit", nullable: true),
                    IsSalaryElement = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollElements", x => x.IntPayrollElementId);
                });

            migrationBuilder.CreateTable(
                name: "payrollPolicies",
                columns: table => new
                {
                    IntPayrollPolicyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    StrPayrollPolicyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSalaryDivideByActualMonthDays = table.Column<bool>(type: "bit", nullable: true),
                    IntGrossSalaryDevidedByDays = table.Column<long>(type: "bigint", nullable: true),
                    IntGrossSalaryRoundDigits = table.Column<long>(type: "bigint", nullable: true),
                    IsGrossSalaryRoundUp = table.Column<bool>(type: "bit", nullable: true),
                    IsGrossSalaryRoundDown = table.Column<bool>(type: "bit", nullable: true),
                    IntNetPayableSalaryRoundDigits = table.Column<long>(type: "bigint", nullable: true),
                    IsNetPayableSalaryRoundUp = table.Column<bool>(type: "bit", nullable: true),
                    IsNetPayableSalaryRoundDown = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollPolicies", x => x.IntPayrollPolicyId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payrollElements");

            migrationBuilder.DropTable(
                name: "payrollPolicies");
        }
    }
}
