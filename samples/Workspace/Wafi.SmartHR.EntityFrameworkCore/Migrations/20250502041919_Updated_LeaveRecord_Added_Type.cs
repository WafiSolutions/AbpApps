using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wafi.SmartHR.Migrations
{
    /// <inheritdoc />
    public partial class Updated_LeaveRecord_Added_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "SmartHRLeaveRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "SmartHRLeaveRecords");
        }
    }
}
