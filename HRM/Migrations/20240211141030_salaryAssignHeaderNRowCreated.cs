using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class salaryAssignHeaderNRowCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salaryAssignHeaders",
                columns: table => new
                {
                    IntSalaryAssignHeaderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntPayrollGroupHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    StrPayrollGroupHeaderTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    NumGrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumNetGrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IntCreateBy = table.Column<long>(type: "bigint", nullable: true),
                    DteCreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdateBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryAssignHeaders", x => x.IntSalaryAssignHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "salaryAssignRows",
                columns: table => new
                {
                    IntSalaryAssignRowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntSalaryAssignHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    IntEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IntPayrollElementId = table.Column<long>(type: "bigint", nullable: false),
                    StrPayrollElement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumNumberOfPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IntCreateBy = table.Column<long>(type: "bigint", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryAssignRows", x => x.IntSalaryAssignRowId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaryAssignHeaders");

            migrationBuilder.DropTable(
                name: "salaryAssignRows");
        }
    }
}
