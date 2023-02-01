using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETForum.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddModifiedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Threads",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Comments");
        }
    }
}
