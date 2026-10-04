using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPortfolio.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificatePreviewUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "Certificates");
        }
    }
}
