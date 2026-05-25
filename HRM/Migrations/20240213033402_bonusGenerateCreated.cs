using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class bonusGenerateCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bonusGenerates",
                columns: table => new
                {
                    IntBonusGenerateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntBonusSetupId = table.Column<long>(type: "bigint", nullable: false),
                    IntYearId = table.Column<long>(type: "bigint", nullable: false),
                    IntMonthId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonusGenerates", x => x.IntBonusGenerateId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonusGenerates");
        }
    }
}
