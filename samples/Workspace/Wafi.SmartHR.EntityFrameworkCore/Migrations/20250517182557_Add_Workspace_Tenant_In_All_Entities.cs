using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wafi.SmartHR.Migrations
{
    /// <inheritdoc />
    public partial class Add_Workspace_Tenant_In_All_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SmartHRLeaveRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "SmartHRLeaveRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SmartHREmployees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "SmartHREmployees",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SmartHRLeaveRecords");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "SmartHRLeaveRecords");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SmartHREmployees");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "SmartHREmployees");
        }
    }
}
