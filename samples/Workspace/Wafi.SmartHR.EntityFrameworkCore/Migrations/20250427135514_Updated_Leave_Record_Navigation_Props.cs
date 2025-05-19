using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wafi.SmartHR.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Leave_Record_Navigation_Props : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRecords_Employees_EmployeeId",
                table: "LeaveRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveRecords",
                table: "LeaveRecords");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRecords_EmployeeId",
                table: "LeaveRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "LeaveRecords",
                newName: "SmartHRLeaveRecords");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "SmartHREmployees");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "SmartHRLeaveRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "SmartHREmployees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "SmartHREmployees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "SmartHREmployees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "SmartHREmployees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmartHRLeaveRecords",
                table: "SmartHRLeaveRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmartHREmployees",
                table: "SmartHREmployees",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmartHRLeaveRecords",
                table: "SmartHRLeaveRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SmartHREmployees",
                table: "SmartHREmployees");

            migrationBuilder.RenameTable(
                name: "SmartHRLeaveRecords",
                newName: "LeaveRecords");

            migrationBuilder.RenameTable(
                name: "SmartHREmployees",
                newName: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "LeaveRecords",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveRecords",
                table: "LeaveRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRecords_EmployeeId",
                table: "LeaveRecords",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRecords_Employees_EmployeeId",
                table: "LeaveRecords",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
