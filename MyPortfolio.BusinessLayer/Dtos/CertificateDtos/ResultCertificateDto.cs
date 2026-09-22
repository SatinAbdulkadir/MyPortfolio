namespace MyPortfolio.BusinessLayer.Dtos.CertificateDtos
{
    public class ResultCertificateDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Issuer { get; set; }
        public DateTime IssueDate { get; set; }
        public string? Description { get; set; }
        public string? CredentialUrl { get; set; }
        public string? FileUrl { get; set; }
        public string? FileType { get; set; }
        public required int CategoryId { get; set; }
        public bool IsFeatured { get; set; }

        // Kategori bilgisi ayrı sorgudan birleştirilir (entity'de navigation property yok)
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryIcon { get; set; }

        // Kartta gösterilecek biçim: "Mart 2026"
        public string IssueDateText => IssueDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"));

        // Görsel mi PDF mi: arayüz lightbox mı açsın yoksa yeni sekme mi
        public bool IsPdf => string.Equals(FileType, "pdf", StringComparison.OrdinalIgnoreCase);
        public bool HasImage => !string.IsNullOrWhiteSpace(FileUrl) && !IsPdf;
    }
}
