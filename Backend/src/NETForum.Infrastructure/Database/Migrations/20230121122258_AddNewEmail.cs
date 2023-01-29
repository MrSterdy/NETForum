using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETForum.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewEmail",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewEmail",
                table: "Users");
        }
    }
}
