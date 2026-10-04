using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPortfolio.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificateCategoryHierarchyAndLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CertificateCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CertificateCategoryLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateCategoryLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateCategoryLinks_CertificateCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CertificateCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CertificateCategoryLinks_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateCategories_ParentId",
                table: "CertificateCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateCategoryLinks_CategoryId",
                table: "CertificateCategoryLinks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateCategoryLinks_CertificateId_CategoryId",
                table: "CertificateCategoryLinks",
                columns: new[] { "CertificateId", "CategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateCategories_CertificateCategories_ParentId",
                table: "CertificateCategories",
                column: "ParentId",
                principalTable: "CertificateCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // VERİ TAŞIMA: EF varsayılan olarak CategoryId'yi en başta siliyordu ve mevcut
            // sertifika-kategori bağları kaybolurdu. Önce ara tabloya kopyalanıyor, sonra siliniyor.
            // Var olmayan kategoriye işaret eden satır olursa FK patlamasın diye o satırlar atlanır.
            migrationBuilder.Sql(@"
                INSERT INTO CertificateCategoryLinks (CertificateId, CategoryId, CreatedDate, IsActive)
                SELECT c.Id, c.CategoryId, GETDATE(), 1
                FROM Certificates c
                WHERE c.CategoryId IN (SELECT Id FROM CertificateCategories);");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Certificates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alırken de veri kaybolmasın: tek kategoriye dönülürken her sertifika
            // bağlı olduğu kategorilerden en küçük Id'liyi alır (ara tablo silinmeden önce)
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE c SET CategoryId = ISNULL(
                    (SELECT MIN(l.CategoryId) FROM CertificateCategoryLinks l WHERE l.CertificateId = c.Id), 0)
                FROM Certificates c;");

            migrationBuilder.DropForeignKey(
                name: "FK_CertificateCategories_CertificateCategories_ParentId",
                table: "CertificateCategories");

            migrationBuilder.DropTable(
                name: "CertificateCategoryLinks");

            migrationBuilder.DropIndex(
                name: "IX_CertificateCategories_ParentId",
                table: "CertificateCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CertificateCategories");
        }
    }
}
