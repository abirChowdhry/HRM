using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class empTransferNPromotionCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaryGenerates");

            migrationBuilder.CreateTable(
                name: "empTransferNPromotions",
                columns: table => new
                {
                    IntEmpTransferNPromotionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IntOldBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntNewBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntOldDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    IntNewDepartmenId = table.Column<long>(type: "bigint", nullable: false),
                    IntOldDesignationId = table.Column<long>(type: "bigint", nullable: false),
                    IntNewDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    IsTransfer = table.Column<bool>(type: "bit", nullable: true),
                    IsPromotion = table.Column<bool>(type: "bit", nullable: true),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empTransferNPromotions", x => x.IntEmpTransferNPromotionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "empTransferNPromotions");

            migrationBuilder.CreateTable(
                name: "salaryGenerates",
                columns: table => new
                {
                    IntSalaryGenerateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    IntMonthId = table.Column<long>(type: "bigint", nullable: false),
                    IntYearId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NumTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryGenerates", x => x.IntSalaryGenerateId);
                });
        }
    }
}
