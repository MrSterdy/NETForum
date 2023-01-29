using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETForum.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameEnabledToBanned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Enabled",
                table: "Users",
                newName: "Banned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Banned",
                table: "Users",
                newName: "Enabled");
        }
    }
}
