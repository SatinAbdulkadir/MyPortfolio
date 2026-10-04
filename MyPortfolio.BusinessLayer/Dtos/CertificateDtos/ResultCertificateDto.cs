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
        public bool IsFeatured { get; set; }

        // Bağlı olduğu tüm kategoriler; ara tablodan manager doldurur
        public List<CertificateCategoryLabelDto> Categories { get; set; } = new();

        // Kartta gösterilecek biçim: "Mart 2026"
        public string IssueDateText => IssueDate.ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"));

        // Görsel mi PDF mi: arayüz lightbox mı açsın yoksa yeni sekme mi
        public bool IsPdf => string.Equals(FileType, "pdf", StringComparison.OrdinalIgnoreCase);
        public bool HasImage => !string.IsNullOrWhiteSpace(FileUrl) && !IsPdf;
    }
}
