using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class salaryGenerateCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salaryGenerates",
                columns: table => new
                {
                    IntSalaryGenerateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntYearId = table.Column<long>(type: "bigint", nullable: false),
                    IntMonthId = table.Column<long>(type: "bigint", nullable: false),
                    NumTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryGenerates", x => x.IntSalaryGenerateId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaryGenerates");
        }
    }
}
