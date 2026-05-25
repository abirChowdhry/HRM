using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    [Migration("20260526000000_addUserAccountsAndOwnership")]
    public partial class addUserAccountsAndOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appUsers",
                columns: table => new
                {
                    IntUserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appUsers", x => x.IntUserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appUsers_StrEmail",
                table: "appUsers",
                column: "StrEmail",
                unique: true);

            AddOwnerColumn(migrationBuilder, "bonusGenerates");
            AddOwnerColumn(migrationBuilder, "bonusSetups");
            AddOwnerColumn(migrationBuilder, "businessUnits");
            AddOwnerColumn(migrationBuilder, "departments");
            AddOwnerColumn(migrationBuilder, "designations");
            AddOwnerColumn(migrationBuilder, "empBasicInfos");
            AddOwnerColumn(migrationBuilder, "empTransferNPromotions");
            AddOwnerColumn(migrationBuilder, "employementTypes");
            AddOwnerColumn(migrationBuilder, "payrollElements");
            AddOwnerColumn(migrationBuilder, "payrollGroupHeaders");
            AddOwnerColumn(migrationBuilder, "payrollGroupRows");
            AddOwnerColumn(migrationBuilder, "payrollPolicies");
            AddOwnerColumn(migrationBuilder, "salaryAdditionNDeductions");
            AddOwnerColumn(migrationBuilder, "salaryAssignHeaders");
            AddOwnerColumn(migrationBuilder, "salaryAssignRows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DropOwnerColumn(migrationBuilder, "salaryAssignRows");
            DropOwnerColumn(migrationBuilder, "salaryAssignHeaders");
            DropOwnerColumn(migrationBuilder, "salaryAdditionNDeductions");
            DropOwnerColumn(migrationBuilder, "payrollPolicies");
            DropOwnerColumn(migrationBuilder, "payrollGroupRows");
            DropOwnerColumn(migrationBuilder, "payrollGroupHeaders");
            DropOwnerColumn(migrationBuilder, "payrollElements");
            DropOwnerColumn(migrationBuilder, "employementTypes");
            DropOwnerColumn(migrationBuilder, "empTransferNPromotions");
            DropOwnerColumn(migrationBuilder, "empBasicInfos");
            DropOwnerColumn(migrationBuilder, "designations");
            DropOwnerColumn(migrationBuilder, "departments");
            DropOwnerColumn(migrationBuilder, "businessUnits");
            DropOwnerColumn(migrationBuilder, "bonusSetups");
            DropOwnerColumn(migrationBuilder, "bonusGenerates");

            migrationBuilder.DropTable(name: "appUsers");
        }

        private static void AddOwnerColumn(MigrationBuilder migrationBuilder, string tableName)
        {
            migrationBuilder.AddColumn<long>(
                name: "IntUserId",
                table: tableName,
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        private static void DropOwnerColumn(MigrationBuilder migrationBuilder, string tableName)
        {
            migrationBuilder.DropColumn(
                name: "IntUserId",
                table: tableName);
        }
    }
}
