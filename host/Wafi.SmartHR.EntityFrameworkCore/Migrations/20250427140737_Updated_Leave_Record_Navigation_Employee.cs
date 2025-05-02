using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wafi.SmartHR.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Leave_Record_Navigation_Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SmartHRLeaveRecords_EmployeeId",
                table: "SmartHRLeaveRecords",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartHRLeaveRecords_SmartHREmployees_EmployeeId",
                table: "SmartHRLeaveRecords",
                column: "EmployeeId",
                principalTable: "SmartHREmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartHRLeaveRecords_SmartHREmployees_EmployeeId",
                table: "SmartHRLeaveRecords");

            migrationBuilder.DropIndex(
                name: "IX_SmartHRLeaveRecords_EmployeeId",
                table: "SmartHRLeaveRecords");
        }
    }
}
