using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class bonusSetupCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bonusSetups",
                columns: table => new
                {
                    IntBonusSetypId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntDepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    IntServiceLengthMonths = table.Column<long>(type: "bigint", nullable: true),
                    StrReligion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonusSetups", x => x.IntBonusSetypId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonusSetups");
        }
    }
}
