using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class payrollGroupHearderAndRowAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payrollGroupHeaders",
                columns: table => new
                {
                    IntPayrollGroupHeaderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrPayrollGroupHeaderTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntPayrollPolicyId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollGroupHeaders", x => x.IntPayrollGroupHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "payrollGroupRows",
                columns: table => new
                {
                    IntPayrollGroupRowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntPayrollHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    IntPayrollElementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    StrPayrollElementName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumNumberOfPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollGroupRows", x => x.IntPayrollGroupRowId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payrollGroupHeaders");

            migrationBuilder.DropTable(
                name: "payrollGroupRows");
        }
    }
}
