using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPortfolio.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillIconColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Skills");
        }
    }
}
