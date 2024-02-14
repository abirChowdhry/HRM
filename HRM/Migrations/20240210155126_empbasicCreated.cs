using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.Migrations
{
    /// <inheritdoc />
    public partial class empbasicCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "empBasicInfos",
                columns: table => new
                {
                    IntEmployeeBasicInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrEmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrEmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntDepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    IntDesignationId = table.Column<long>(type: "bigint", nullable: false),
                    StrGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrReligion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrMaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrBloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DteDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DteJoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DteInternCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DteProbationaryCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DteConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DteLastWorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntSupervisorId = table.Column<long>(type: "bigint", nullable: true),
                    IntLineManagerId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IntBusinessUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IntEmploymentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    DteCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntCreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DteUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntUpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empBasicInfos", x => x.IntEmployeeBasicInfoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "empBasicInfos");
        }
    }
}
