using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VmarmyshTest.Migrations
{
    /// <inheritdoc />
    public partial class MigrationJournal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedAt",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
